using BusinessLogic.Services.Implementations;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using TestBase.DbTest;

namespace BusinessLogic.Tests.Services
{
	[TestClass]
	public class PupilServiceDbTest : DbUnitTestBase
	{
		private static TheAppDbConnection _theAppDbConnection =
			new TheAppDbConnection(ConfigurationManager.ConnectionStrings["TheAppConnectionString"].ConnectionString);

		private UnitOfWork _unitOfWork;
		private PupilORMService _pupilService;

		public PupilServiceDbTest() : base(_theAppDbConnection.Connection)
		{
			var theAppModels = TheAppModels.FactoryMethod(_theAppDbConnection);
			_unitOfWork = new UnitOfWork(theAppModels);

			_pupilService = new PupilORMService(_unitOfWork);
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
