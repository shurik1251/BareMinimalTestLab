using BusinessLogic.Models;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Services.Interfaces
{
	public interface IPupilService
	{
		List<PupilModel> GetPupilsBornThisDate(DateTime today);
	}
}
