using System;

namespace TestBase.DbTest
{
	public class TableNameResolver
	{
		private readonly string _databaseName;
		private readonly string _schemaName;

		public TableNameResolver(string databaseName, string schemaName)
		{
			if (!String.IsNullOrEmpty(databaseName)
				&& String.IsNullOrWhiteSpace(schemaName))
			{
				throw new ArgumentException("Database name cannot be used without a qualified schema name.");
			}

			_databaseName = databaseName;
			_schemaName = schemaName;
		}

		public string GetName(string tableName)
		{
			return string.IsNullOrEmpty(_databaseName)
				? tableName
				: string.Concat(_databaseName, ".", _schemaName, ".", tableName);
		}
	}
}
