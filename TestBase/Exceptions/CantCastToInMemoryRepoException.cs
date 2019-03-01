using System;

namespace TheApp.Tests.Exceptions
{
	public class CantCastToInMemoryRepoException<T> : Exception
	{
		public CantCastToInMemoryRepoException()
			: base($"Can't cast to in {typeof(T).Name}.")
		{

		}
	}
}
