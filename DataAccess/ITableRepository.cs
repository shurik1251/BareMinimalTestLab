
namespace DataAccess
{
	public interface ITableRepository<TEntity> : IReadonlyRepository<TEntity>
	{
		void InsertOnCommit(TEntity entity);

		void DeleteOnCommit(TEntity entity);
	}
}
