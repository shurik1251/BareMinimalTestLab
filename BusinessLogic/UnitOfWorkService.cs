using DataAccess;

namespace BusinessLogic
{
	public class UnitOfWorkService : ServiceBase
	{
		public UnitOfWorkService(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public void SubmitChanges()
		{
			UnitOfWork.SubmitChanges();
		}
	}
}
