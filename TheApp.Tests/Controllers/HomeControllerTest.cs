using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TheApp.Controllers;

namespace TheApp.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		private MockRepository _mockRepository;
		private Mock<IPupilService> _pupilServiceMock;

		public HomeControllerTest()
		{
			_mockRepository = new MockRepository(MockBehavior.Loose);
			_pupilServiceMock = _mockRepository.Create<IPupilService>();
		}

		[TestMethod]
		public void Index()
		{
			var expected = new List<PupilModel>
			{
				new PupilModel { Id = 1, FirstName = "FirstName", LastName = "LastName" }
			};

			_pupilServiceMock
				.Setup(mq => mq.GetPupilsBornThisDate(It.IsAny<DateTime>()))
				.Returns(expected);
			
			var homeController = new HomeController(_pupilServiceMock.Object);

			var viewResult = homeController.Index() as ViewResult;

			viewResult.Should().NotBeNull();
		}
	}
}
