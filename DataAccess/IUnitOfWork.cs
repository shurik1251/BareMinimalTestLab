using DataAccess.Interfaces;
using System;

namespace DataAccess
{
	public interface IUnitOfWork : IDisposable
	{
		void SubmitChanges();

		IPupilRepository PupilRepository { get; }
	}
}
