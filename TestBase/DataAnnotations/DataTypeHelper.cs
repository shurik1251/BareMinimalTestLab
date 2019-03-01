using System.Linq;
using Dapper;

namespace TestBase.DataAnnotations
{
	public static class DataTypeHelper
	{
		public static string GetSqlTableName<TData>()
		{
			var tableAttr =
				typeof(TData)
					.GetCustomAttributes(typeof(TableAttribute), true)
					.First() as TableAttribute;

			return tableAttr.Name;
		}
	}
}
