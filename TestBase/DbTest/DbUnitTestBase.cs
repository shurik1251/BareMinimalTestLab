using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FluentAssertions;
using System.Transactions;
using System.Data.SqlClient;

namespace TestBase.DbTest
{
	[TestClass]
	public abstract class DbUnitTestBase
	{
		private TransactionScope _ambientTransaction;
		private readonly IsolationLevel _isolationLevel;

		[TestInitialize]
		public void ScopeInitialize()
		{
			Db.ResetAllTablesData();
			Db.ResetAllTablesIdentity();
			_ambientTransaction = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = new TimeSpan(0, 0, 60),
					IsolationLevel = _isolationLevel
				});

			initializeRepository();
		}

		[TestCleanup]
		public void ScopeCleanup()
		{
			Db.DisposeDefaultConnection();
			_ambientTransaction.Dispose();
		}

		protected virtual void initializeRepository()
		{
		}

		public SqlDbHelper Db { get; private set; }

		protected DbUnitTestBase(SqlConnection connection)
			: this(connection, IsolationLevel.ReadCommitted)
		{
		}

		protected DbUnitTestBase(SqlConnection connection, IsolationLevel isolationLevel)
		{
			_isolationLevel = isolationLevel;
			Db = new SqlDbHelper(connection);
		}

		#region Utils
		protected void DbCheckActualAndExpected<T>(IEnumerable<T> expected)
		{
			var actual = Db.GetAll<T>();
			actual.Should().BeEquivalentTo(expected);
		}
		#endregion
	}
}
