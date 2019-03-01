using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
	public interface IInMemoryReadonlyRepository<TEntity> : IReadonlyRepository<TEntity>
	{
	}

	public abstract class InMemoryReadonlyRepositoryBase<TEntity> : IInMemoryReadonlyRepository<TEntity>
	{
		private readonly IEnumerable<TEntity> _source;

		protected InMemoryReadonlyRepositoryBase()
		{
			_source = new List<TEntity>();
		}

		protected InMemoryReadonlyRepositoryBase(IEnumerable<TEntity> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			_source = source;
		}

		#region IReadonlyRepository<TEntity> Members

		public virtual IQueryable<TEntity> GetAll()
		{
			return _source.AsQueryable();
		}

		#endregion
	}
}
