using System.Data.Entity;
using System.Linq;

namespace DataAccess
{
	public abstract class ReadonlyRepository<TEntity> : IReadonlyRepository<TEntity> where TEntity : class
	{
		private readonly DbContext _dbContext;

		protected ReadonlyRepository(DbContext dbContext) : base()
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
	}
}
