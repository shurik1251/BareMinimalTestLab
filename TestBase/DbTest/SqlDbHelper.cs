using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TestBase.BulkCopy;
using TestBase.DataAnnotations;

namespace TestBase.DbTest
{
	public class SqlDbHelper
	{
		private const int IdentitySeed = 0;
		private readonly string _defaultConnectionString;
		private SqlConnection _defaultConnection;

		public SqlDbHelper(SqlConnection connection)
		{
			_defaultConnection = connection;
		}

		public SqlConnection GetDefaultConnection()
		{
			return _defaultConnection;
		}

		public void DisposeDefaultConnection()
		{
			if (_defaultConnection != null)
			{
				_defaultConnection.Dispose();
				_defaultConnection = null;
			}
		}

		public void ResetTableIdentity<TData>()
		{
			var tableName = DataTypeHelper.GetSqlTableName<TData>();

			const string sql = @"
IF ((SELECT OBJECTPROPERTY(OBJECT_ID(@tableName), 'TableHasIdentity')) = 1)
DBCC CHECKIDENT (@tableName, RESEED, @IdentitySeed) WITH NO_INFOMSGS";

			GetDefaultConnection().Execute(sql, new { tableName, IdentitySeed });
		}

		public void ResetAllTablesIdentity()
		{
			const string sql = @"
SET NOCOUNT ON;
DECLARE @tableName nvarchar(256)

DECLARE table_cursor CURSOR FORWARD_ONLY STATIC FOR
SELECT '[' + s.name + '].[' + o.name + ']'
FROM sys.objects o 
JOIN sys.identity_columns ic ON o.object_id = ic.object_id
JOIN sys.schemas s ON s.schema_id = o.schema_id
WHERE o.type = 'U' AND ic.last_value IS NOT NULL
FOR READ ONLY

OPEN table_cursor

FETCH NEXT FROM table_cursor 
INTO @tableName

WHILE @@FETCH_STATUS = 0
BEGIN
	DBCC CHECKIDENT (@tableName, RESEED, @IdentitySeed) WITH NO_INFOMSGS

	FETCH NEXT FROM table_cursor 
	INTO @tableName
END 
CLOSE table_cursor;
DEALLOCATE table_cursor;";

			GetDefaultConnection().Execute(sql, new { IdentitySeed });
		}

		public void ResetAllTablesData()
		{
			const string sql = @"
EXEC sp_MSForEachTable '
IF EXISTS(SELECT * FROM ?) 
BEGIN
 ALTER TABLE ? NOCHECK CONSTRAINT ALL
 ALTER TABLE ? DISABLE TRIGGER ALL
END'
EXEC sp_MSForEachTable '
IF EXISTS(SELECT * FROM ?) AND ''?'' NOT IN (''[dbo].[tc01_Clusters]'', ''[dbo].[tc02_ConnectionStrings]'')
BEGIN
 SET QUOTED_IDENTIFIER ON;
 DELETE FROM ?;
    IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1
    DBCC CHECKIDENT(''?'', RESEED, 1);
END'
EXEC sp_MSForEachTable '
ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL
ALTER TABLE ? ENABLE TRIGGER ALL'
";
			GetDefaultConnection().Execute(sql, commandTimeout: 60);
		}

		public IEnumerable<TData> GetAll<TData>()
		{
			var tableName = DataTypeHelper.GetSqlTableName<TData>();
			return GetDefaultConnection().Query<TData>(String.Format("SELECT * FROM {0}", tableName));
		}

		public void SetAll<TData>(IEnumerable<TData> objects, TableNameResolver nameResolver = null)
		{
			setAll(objects, SqlBulkCopyOptions.KeepIdentity, nameResolver);
		}

		public void SetAll(IEnumerable<object> objects, TableNameResolver nameResolver = null)
		{
			var setAllMethod = GetType().GetMethod("setAll", BindingFlags.NonPublic | BindingFlags.Instance);
			var cast = typeof(Enumerable).GetMethod("Cast", BindingFlags.Public | BindingFlags.Static);
			foreach (var objectsGroupedByType in objects.GroupBy(o => o.GetType()))
			{
				var typedObjects = cast.MakeGenericMethod(objectsGroupedByType.Key).Invoke(null, new object[] { objectsGroupedByType });
				setAllMethod.MakeGenericMethod(objectsGroupedByType.Key).Invoke(
					this,
					new object[] {
						typedObjects,
						SqlBulkCopyOptions.KeepIdentity,
						nameResolver
					});
			}
		}

		public void SetAllIdentityOn<TData>(IEnumerable<TData> objects, TableNameResolver nameResolver = null)
		{
			setAll(objects, SqlBulkCopyOptions.Default, nameResolver);
		}

		private void setAll<TData>(IEnumerable<TData> objects, SqlBulkCopyOptions copyOptions, TableNameResolver nameResolver)
		{
			var connection = GetDefaultConnection();

			var bulkCopy = new SqlBulkCopy(connection, copyOptions, null);
			bulkCopy.SetupBulkCopyByDataType<TData>(nameResolver);

			bool keepClosed = connection.State == ConnectionState.Closed;

			try
			{
				if (connection.State == ConnectionState.Closed)
				{
					connection.Open();
				}
				using (var dataReader = new ObjectDataReader<TData>(objects))
				{
					bulkCopy.WriteToServer(dataReader);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("SetAll < " + typeof(TData) + " >", ex);
			}
			finally
			{
				if (keepClosed)
				{
					connection.Close();
				}
			}
		}

		public void ExecuteSp(string spName)
		{
			GetDefaultConnection().Execute(spName, commandType: CommandType.StoredProcedure);
		}

		public void ExecuteSp(string spName, DynamicParameters p, int? commandTimeout = null)
		{
			GetDefaultConnection().Execute(spName, p, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
		}

		public IEnumerable<T> ExecuteSp<T>(string spName, DynamicParameters p)
		{
			return GetDefaultConnection().Query<T>(spName, p, commandType: CommandType.StoredProcedure);
		}

		public IEnumerable<T> ExecuteMultiResultSp<T>(string spName, DynamicParameters p)
		{
			return ExecuteMultiResultSp<T, DynamicParameters>(spName, p);
		}

		public IEnumerable<T> ExecuteMultiResultSp<T, TP>(string spName, TP p)
		{
			var results = new List<T>();

			using (var multi = GetDefaultConnection()
									.QueryMultiple(spName,
												   p,
												   commandType: CommandType.StoredProcedure))
			{
				while (!multi.IsConsumed)
				{
					results.AddRange(multi.Read<T>());
				}
			}

			return results;
		}

		internal IEnumerable<T> ExecuteQuery<T>(string sqlQuery, DynamicParameters p)
		{
			return GetDefaultConnection().Query<T>(sqlQuery, p, commandType: CommandType.Text);
		}

		internal T ExecuteScalar<T>(string sqlQuery, DynamicParameters p)
		{
			return GetDefaultConnection().Query<T>(sqlQuery, p, commandType: CommandType.Text).First();
		}

		public void ExecuteNonQuery(string commandText)
		{
			GetDefaultConnection().Execute(commandText);
		}
	}
}
