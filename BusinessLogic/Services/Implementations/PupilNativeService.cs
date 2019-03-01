using BusinessLogic.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using BusinessLogic.Services.Interfaces;
using System.Data.SqlClient;

namespace BusinessLogic.Services.Implementations
{
	public class PupilNativeService : IPupilService
	{
		private readonly SqlConnection _theAppDbConnection;

		public PupilNativeService(TheAppDbConnection theAppDbConnection)
		{
			_theAppDbConnection = theAppDbConnection.Connection;
		}

		public List<PupilModel> GetPupilsBornThisDate(DateTime day)
		{
			var selectSql = "SELECT PupilId as Id, FirstName, LastName FROM Pupils where cast(BirthDate As Date) = @date";

			var result = _theAppDbConnection.Query<PupilModel>(selectSql, new { day.Date });

			return result.ToList();
		}
	}
}
