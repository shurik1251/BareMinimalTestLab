using BusinessLogic.Services.Interfaces;
using System;
using System.Web.Mvc;

namespace TheApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPupilService _pupilService;

		public HomeController(IPupilService pupilService)
		{
			_pupilService = pupilService;
		}

		public ActionResult Index()
		{
			var theDate = new DateTime(2018, 2, 24);
			var pupils = _pupilService.GetPupilsBornThisDate(theDate);

			return View(pupils);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}