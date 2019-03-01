using Autofac;
using Autofac.Integration.Mvc;
using BusinessLogic;
using BusinessLogic.Services.Implementations;
using BusinessLogic.Services.Interfaces;
using DataAccess;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace TheApp
{
	public sealed class InjectionConfig
	{
		public static void RegisterTypes()
		{
			IContainer container = GetAutofacContainer();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}

		private static IContainer GetAutofacContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

			builder.Register(
				c => {
					var connectionString = ConfigurationManager.ConnectionStrings["TheAppConnectionString"].ConnectionString;
					return new TheAppDbConnection(connectionString);
				})
				.As<TheAppDbConnection>()
				.InstancePerLifetimeScope()
				.ExternallyOwned();

			builder.Register(
				c =>
				{
					var theAppDbConnection = c.Resolve<TheAppDbConnection>();
					return TheAppModels.FactoryMethod(theAppDbConnection);
				})
				.InstancePerDependency();

			builder.Register<HttpSessionStateBase>(c => new HttpSessionStateWrapper(HttpContext.Current.Session))
				.InstancePerDependency();

			builder.Register<HttpContextBase>(c => new HttpContextWrapper(HttpContext.Current))
				.InstancePerLifetimeScope();

			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
			builder.RegisterType<UnitOfWorkService>().InstancePerLifetimeScope();
			builder.RegisterType<PupilNativeService>().As<IPupilService>().InstancePerLifetimeScope();

			return builder.Build();
		}
	}
}