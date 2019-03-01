using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestBase
{
	internal class ObjectStateManager
	{
		public object Object { get; }
		private readonly Dictionary<string, object> _ObjectSnapshot;

		public ObjectStateManager(object obj)
		{
			Object = obj ?? throw new ArgumentNullException("obj");
			_ObjectSnapshot = _MakeSnapshot(obj);
		}

		public bool IsModified()
		{
			var original = _ObjectSnapshot;
			var current = _MakeSnapshot(Object);

			object currentValue;
			object originalValue;
			foreach (var o in original)
			{
				originalValue = o.Value;
				currentValue = current[o.Key];

				if (!Equals(currentValue, originalValue))
				{
					return true;
				}
			}

			return false;
		}

		public static Dictionary<string, object> _MakeSnapshot(Object obj)
		{
			Type type = obj.GetType();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var objectSnapshot = new Dictionary<string, object>(properties.Length);

			foreach (PropertyInfo property in properties)
			{
				objectSnapshot[property.Name] = property.GetValue(obj, null);
			}

			return objectSnapshot;
		}
	}
}
