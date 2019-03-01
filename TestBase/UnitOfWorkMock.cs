using DataAccess;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheApp.Tests.Exceptions;

namespace TestBase
{
	public class UnitOfWorkMock
	{
		public IUnitOfWork UnitOfWork => UnitOfWorkMoq.Object;

		public Mock<IUnitOfWork> UnitOfWorkMoq { get; }

		public MockRepository MockRepository { get; }

		public UnitOfWorkMock()
		{
			MockRepository = new MockRepository(MockBehavior.Default) { CallBase = true };
			UnitOfWorkMoq = MockRepository.Create<IUnitOfWork>();
		}

		public TRepo InitializeEmptyRepository<TRepo, TDo>() where TRepo : class, IRepository<TDo>
		{
			return InitializeRepository<TRepo, TDo>(new List<TDo>());
		}

		public TRep InitializeRepository<TRep, TDo>(IEnumerable<TDo> all) where TRep : class, IRepository<TDo>
		{
			var inMemoryRepositoryMoq = createInMemoryRepositoryMock<TRep, TDo>(all);

			var concreteInMemoryRepository = inMemoryRepositoryMoq.As<TRep>().Object;

			UnitOfWorkMoq.Setup(_makeRepositoryGetter<TRep>()).Returns(concreteInMemoryRepository);

			return concreteInMemoryRepository;
		}

		public void SetRepository<TRep, TDo>(TRep repositoryInstace) where TRep : class, IRepository<TDo>
		{
			UnitOfWorkMoq.Setup(_makeRepositoryGetter<TRep>()).Returns(repositoryInstace);
		}

		private Mock createInMemoryRepositoryMock<TRep, TDo>(IEnumerable<TDo> all) where TRep : class, IRepository<TDo>
		{
			if (typeof(ITableRepository<TDo>).IsAssignableFrom(typeof(TRep)))
			{
				return MockRepository.Create<InMemoryTableRepositoryBase<TDo>>(all);
			}
			if (typeof(IReadonlyRepository<TDo>).IsAssignableFrom(typeof(TRep)))
			{
				return MockRepository.Create<InMemoryReadonlyRepositoryBase<TDo>>(all);
			}
			if (typeof(IRepository<TDo>).IsAssignableFrom(typeof(TRep)))
			{
				return MockRepository.Create<InMemoryRepositoryBase<TDo>>();
			}
			throw new ArgumentException(string.Format("{0} type must be assignable from either {1}, {2} or {3} types.",
				typeof(TRep),
				typeof(InMemoryTableRepositoryBase<TDo>),
				typeof(InMemoryReadonlyRepositoryBase<TDo>),
				typeof(InMemoryRepositoryBase<TDo>)));
		}

		public void InitializeRepositoryWithAppend<TRep, TDo>(TDo appendObject) where TRep : class, IRepository<TDo>
		{
			InitializeRepositoryWithAppend<TRep, TDo>(new List<TDo> { appendObject });
		}

		public void InitializeRepositoryWithAppend<TRep, TDo>(IEnumerable<TDo> appendList) where TRep : class, IRepository<TDo>
		{
			try
			{
				var r = GetInMemoryReadonlyRepository<TRep, TDo>();
				appendList = r.GetAll().Union(appendList).ToList();
			}
			catch (RepositoryNotInitializedException)
			{
				// Suppress exception if this was a trying to make append to not initialized repository.
			}

			InitializeRepository<TRep, TDo>(appendList);
		}

		public TRepo GetRepository<TRepo>() where TRepo : class
		{
			var concreteInMemoryRepository = _makeRepositoryGetter<TRepo>().Compile().Invoke(UnitOfWork);

			if (concreteInMemoryRepository == null)
				throw new RepositoryNotInitializedException();

			return concreteInMemoryRepository;
		}

		public Mock<TRepo> GetRepositoryMock<TRepo>() where TRepo : class
		{
			var repo = GetRepository<TRepo>();
			return Mock.Get(repo).As<TRepo>();
		}

		public IInMemoryReadonlyRepository<TDo> GetInMemoryReadonlyRepository<TRepo, TDo>() where TRepo : class
		{
			var repo = GetRepository<TRepo>();
			var memRepo = repo as IInMemoryReadonlyRepository<TDo>;
			if (memRepo != null)
			{
				return memRepo;
			}

			throw new CantCastToInMemoryRepoException<IInMemoryReadonlyRepository<TDo>>();
		}

		public InMemoryTableRepositoryBase<TDo> GetInMemoryTableRepository<TRepo, TDo>() where TRepo : class, ITableRepository<TDo>
		{
			var repo = GetRepository<TRepo>();
			var memRepo = repo as InMemoryTableRepositoryBase<TDo>;
			if (memRepo == null)
			{
				throw new CantCastToInMemoryRepoException<InMemoryTableRepositoryBase<TDo>>();
			}
			return memRepo;
		}

		private Expression<Func<IUnitOfWork, TResult>> _makeRepositoryGetter<TResult>()
		{
			string repositoryPropertyName = typeof(TResult).Name.Substring(1);
			var repositoryGetterExpression = _makeGetter<IUnitOfWork, TResult>(repositoryPropertyName);
			return repositoryGetterExpression;
		}

		private Expression<Func<T, TResult>> _makeGetter<T, TResult>(string propertyName)
		{
			ParameterExpression input = Expression.Parameter(typeof(T));
			var expr = Expression.Property(input, typeof(T).GetProperty(propertyName));
			return Expression.Lambda<Func<T, TResult>>(expr, input);
		}
	}
}
