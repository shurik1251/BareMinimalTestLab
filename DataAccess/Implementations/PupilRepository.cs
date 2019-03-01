using DataAccess.Interfaces;

namespace DataAccess.Implementations
{
	public class PupilRepository : TableRepository<Pupil>, IPupilRepository
	{
		public PupilRepository(TheAppModels context) : base(context)
		{
		}
	}
}
