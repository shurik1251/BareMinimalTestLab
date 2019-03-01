using System.Data.Entity;
using System.Linq;

namespace DataAccess
{
	public abstract class TableRepository<TEntity> : ITableRepository<TEntity> where TEntity : class
	{
		private readonly DbContext _dbContext;

		protected TableRepository(DbContext dbContext) : base()
		{
			_dbContext = dbContext;
		}

		public IQueryable<TEntity> GetAll()
		{
			return Table;
		}

		protected DbSet<TEntity> Table
		{
			get { return _dbContext.Set<TEntity>(); }
		}

		public void InsertOnCommit(TEntity entity)
		{
			Table.Add(entity);
		}

		public void DeleteOnCommit(TEntity entity)
		{
			Table.Remove(entity);
		}
	}
}
