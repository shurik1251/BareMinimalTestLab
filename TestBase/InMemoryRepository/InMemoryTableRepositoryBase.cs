using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
	public abstract class InMemoryTableRepositoryBase<TEntity> : ITableRepository<TEntity>, IInMemoryReadonlyRepository<TEntity>
	{
		private readonly Dictionary<TEntity, ObjectStateManager> _source;
		private readonly List<TEntity> _insertedUncommited = new List<TEntity>();
		private readonly List<TEntity> _deletedUncommited = new List<TEntity>();
		public Action<TEntity> OnInsertAction { get; set; }
		public Action<TEntity> OnDeleteAction { get; set; }

		protected InMemoryTableRepositoryBase()
		{
			_source = new Dictionary<TEntity, ObjectStateManager>();
		}

		public InMemoryTableRepositoryBase(IEnumerable<TEntity> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			_source = source.ToDictionary(o => o, o => new ObjectStateManager(o));
		}

		public IEnumerable<TEntity> GetUpdated()
		{
			return _source.Where(kv => kv.Value.IsModified()).Select(kv => kv.Key);
		}

		public IEnumerable<TEntity> GetInserted()
		{
			return _insertedUncommited;
		}

		public IEnumerable<TEntity> GetDeleted()
		{
			return _deletedUncommited;
		}

		#region ITableRepository<TEntity> Members

		public virtual IQueryable<TEntity> GetAll()
		{
			return _source.Keys.AsQueryable();
		}

		public virtual void InsertOnCommit(TEntity entity)
		{
			_insertedUncommited.Add(entity);

			OnInsertAction?.Invoke(entity);
		}

		public virtual void DeleteOnCommit(TEntity entity)
		{
			_deletedUncommited.Add(entity);
			OnDeleteAction?.Invoke(entity);
		}

		public virtual void DeleteOnCommit(IEnumerable<TEntity> entities)
		{
			foreach (var entity in entities)
			{
				DeleteOnCommit(entity);
			}
		}

		#endregion
	}
}
