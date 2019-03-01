using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BusinessLogic.Services.Implementations
{
	public class PupilORMService : ServiceBase, IPupilService
	{
		public PupilORMService(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public List<PupilModel> GetPupilsBornThisDate(DateTime day)
		{
			var result =
				from pupil in PupilRepository.GetAll()
				where DbFunctions.TruncateTime(pupil.BirthDate) == day.Date
				select new PupilModel
				{
					Id = pupil.PupilId,
					FirstName = pupil.FirstName,
					LastName = pupil.LastName
				};

			return result.ToList();
		}
	}
}
