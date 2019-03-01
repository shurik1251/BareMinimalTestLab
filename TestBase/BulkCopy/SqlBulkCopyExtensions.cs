using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TestBase.DataAnnotations;
using TestBase.DbTest;

namespace TestBase.BulkCopy
{
	internal static class SqlBulkCopyExtensions
	{
		public static void SetupBulkCopyByDataType<TData>(this SqlBulkCopy bulkCopy, TableNameResolver nameResolver)
		{
			bulkCopy.MapAllPropertiesAsColumns<TData>();
			bulkCopy.DestinationTableName = nameResolver == null
				? DataTypeHelper.GetSqlTableName<TData>()
				: nameResolver.GetName(DataTypeHelper.GetSqlTableName<TData>());
		}

		public static void MapAllPropertiesAsColumns<TData>(this SqlBulkCopy bulkCopy)
		{
			var propNames = typeof(TData)
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => !p.GetCustomAttributes(typeof(ComputedColumnAttribute), false).Any())
				.Select(p => p.Name);

			foreach (var pName in propNames)
			{
				bulkCopy.ColumnMappings.Add(
					pName,
					pName.StartsWith("_") ? pName.Substring(1) : pName);
			}
		}
	}
}
