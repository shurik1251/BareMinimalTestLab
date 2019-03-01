using System.Data.SqlClient;

namespace DataAccess
{
	public class TheAppDbConnection
	{
		public SqlConnection Connection { get; }

		public TheAppDbConnection(string connectionString)
		{
			var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString) { ConnectTimeout = 5 };

			Connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
		}
	}
}
