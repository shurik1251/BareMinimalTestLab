using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBase
{
	public static class InMemoryRepositoryTestAssertExtensions
	{
		public static void Check<TEntity>(
			this ITableRepository<TEntity> repository,
			Action<IEnumerable<TEntity>> inserted = null,
			Action<IEnumerable<TEntity>> updated = null,
			Action<IEnumerable<TEntity>> deleted = null)
		{
			if (repository == null)
			{
				throw new ArgumentNullException(nameof(repository));
			}

			var inMemoryRepository = (InMemoryTableRepositoryBase<TEntity>)repository;

			#region inserted
			{
				var insertedColl = inMemoryRepository.GetInserted() ?? new TEntity[0];

				if (inserted != null)
				{
					inserted(insertedColl);
				}
				else
				{
					if (insertedColl.Count() != 0)
					{
						Assert.Fail("The repository '{0}' should not have inserted items. But it has {1} inserted items.", repository.GetType().Name, insertedColl.Count());
					}
				}
			}
			#endregion inserted

			#region updated
			{
				var updatedColl = inMemoryRepository.GetUpdated() ?? new TEntity[0];

				if (updated != null)
				{
					updated(updatedColl);
				}
				else
				{
					if (updatedColl.Count() != 0)
					{
						Assert.Fail("The repository '{0}' should not have updated items. But it has {1} updated items.", repository.GetType().Name, updatedColl.Count());
					}
				}
			}
			#endregion updated

			#region deleted
			{
				var deletedColl = inMemoryRepository.GetDeleted() ?? new TEntity[0];

				if (deleted != null)
				{
					deleted(deletedColl);
				}
				else
				{
					if (deletedColl.Count() != 0)
					{
						Assert.Fail("The repository '{0}' should not have deleted items. But it has {1} deleted items.", repository.GetType().Name, deletedColl.Count());
					}
				}
			}
			#endregion deleted
		}
	}
}
