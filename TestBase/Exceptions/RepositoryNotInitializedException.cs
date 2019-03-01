using System;

namespace TheApp.Tests.Exceptions
{
	public class RepositoryNotInitializedException : Exception
	{
		public RepositoryNotInitializedException()
			: base("The repository is not initialized for the unit of work instance.")
		{
		}
	}
}
