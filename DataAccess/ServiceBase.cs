using DataAccess.Interfaces;

namespace DataAccess
{
	public abstract partial class ServiceBase
	{
		protected IUnitOfWork UnitOfWork { get; private set; }

		protected ServiceBase(IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}

		protected IPupilRepository PupilRepository
		{
			get { return UnitOfWork.PupilRepository; }
		}
	}
}
