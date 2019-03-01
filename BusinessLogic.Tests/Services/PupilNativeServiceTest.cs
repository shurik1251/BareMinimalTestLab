using BusinessLogic.Services.Implementations;
using BusinessLogic.Services.Interfaces;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using TestBase.DbTest;

namespace BusinessLogic.Tests
{
	[TestClass]
	public class PupilNativeServiceTest : DbUnitTestBase
	{
		private static TheAppDbConnection _theAppDbConnection =
			new TheAppDbConnection(ConfigurationManager.ConnectionStrings["TheAppConnectionString"].ConnectionString);

		private IPupilService _pupilService;

		public PupilNativeServiceTest() : base(_theAppDbConnection.Connection)
		{
			_pupilService = new PupilNativeService(_theAppDbConnection);
		}

		protected override void initializeRepository()
		{
			Db.SetAll(new object[]
			{
				new Pupil { PupilId = 1, FirstName = "FirstName", MiddleName = "MiddleName", LastName = "LastName", BirthDate = DateTime.Now }
			});
		}

		[TestMethod]
		public void TestMethod()
		{
			var actual = _pupilService.GetPupilsBornThisDate(DateTime.Today);
		}
	}
}
