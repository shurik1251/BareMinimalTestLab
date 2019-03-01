using Dapper;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;

namespace DataAccess
{
	public partial class TheAppModels
	{
		private TheAppModels(EntityConnection connection) : base(connection, false)
		{			
		}

		public static TheAppModels FactoryMethod(TheAppDbConnection connection)
		{
			var metadataWorkspace =
				new MetadataWorkspace(
					new string[] { "res://*/TheAppModels.csdl", "res://*/TheAppModels.ssdl", "res://*/TheAppModels.msl" },
					new Assembly[] { Assembly.GetExecutingAssembly() });

			var entityConnection = new EntityConnection(metadataWorkspace, connection.Connection);

			return new TheAppModels(entityConnection);
		}
	}

	[TableAttribute("Pupils")]
	public partial class Pupil
	{
	}
}
