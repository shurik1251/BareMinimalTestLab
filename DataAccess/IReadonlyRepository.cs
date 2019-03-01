using System.Linq;

namespace DataAccess
{
	public interface IReadonlyRepository<TEntity> : IRepository<TEntity>
	{
		IQueryable<TEntity> GetAll();
	}
}
