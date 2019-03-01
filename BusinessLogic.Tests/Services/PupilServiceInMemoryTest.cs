using BusinessLogic.Services.Implementations;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestBase;

namespace BusinessLogic.Tests
{
	[TestClass]
	public class PupilServiceInMemoryTest
	{
		private UnitOfWorkMock _unitOfWorkMock;
		private PupilORMService _pupilService;

		[TestInitialize]
		public void TestInitialize()
		{
			_unitOfWorkMock = new UnitOfWorkMock();

			_unitOfWorkMock.InitializeEmptyRepository<IPupilRepository, Pupil>();

			_pupilService = new PupilORMService(_unitOfWorkMock.UnitOfWork);
		}

		[TestMethod]
		public void TestMethod()
		{
			_unitOfWorkMock.InitializeRepository<IPupilRepository, Pupil>(new List<Pupil>
			{
				new Pupil { PupilId = 1, FirstName = "FirstName", MiddleName = "MiddleName", LastName = "LastName", BirthDate = DateTime.Now }
			});

			var actual = _pupilService.GetPupilsBornThisDate(DateTime.Today);
		}
	}
}
