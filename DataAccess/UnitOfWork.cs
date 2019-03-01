using System;
using DataAccess.Implementations;
using DataAccess.Interfaces;

namespace DataAccess
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly TheAppModels _dataContext;
		private bool _disposed;

		public UnitOfWork(TheAppModels dataContext)
		{
			_dataContext = dataContext;
			_disposed = false;
		}

		public void SubmitChanges()
		{
			_dataContext.SaveChanges();
		}

		#region Repositories

		private IPupilRepository _pupilRepository;

		public IPupilRepository PupilRepository
		{
			get { return _pupilRepository ?? (_pupilRepository = new PupilRepository(_dataContext)); }
		}

		#endregion Repositories

		#region IDispose

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool dispose)
		{
			if (!_disposed)
			{
				if (dispose)
				{
					_dataContext.Dispose();
				}
			}
			_disposed = true;
		}

		#endregion IDispose
	}
}
