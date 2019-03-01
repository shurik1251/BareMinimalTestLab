using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestBase.Helpers
{
	public static class ReflectionHelper
	{
		public static List<FieldInfo> GetConstants(Type type)
		{
			FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
				 BindingFlags.Static | BindingFlags.FlattenHierarchy);

			return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
		}

		public static IEnumerable<PropertyInfo> GetPublicStaticProperties(Type type)
		{
			var props = type.GetProperties(BindingFlags.Public |
				BindingFlags.Static | BindingFlags.DeclaredOnly);

			return props.ToList();
		}

		public static Func<T, object> CreatePropertyAccessor<T>(PropertyInfo p)
		{
			// Define the parameter that will be passed - will be the current object
			var parameter = Expression.Parameter(typeof(T), "input");

			// Define an expression to get the value from the property
			var propertyAccess = Expression.MakeMemberAccess(parameter, p);

			// Make sure the result of the get method is cast as an object
			var castAsObject = Expression.TypeAs(propertyAccess, typeof(object));

			// Create a lambda expression for the property access and compile it
			var lamda = Expression.Lambda<Func<T, object>>(castAsObject, parameter);
			return lamda.Compile();
		}

		public static Func<object, object> CreatePropertyAccessor(PropertyInfo p)
		{
			// Define the parameter that will be passed - will be the current object
			var parameter = Expression.Parameter(typeof(object), "input");

			// Convert parameter to the current object type
			var typedParameter = Expression.Convert(parameter, p.DeclaringType);

			// Define an expression to get the value from the property
			var propertyAccess = Expression.MakeMemberAccess(typedParameter, p);

			// Make sure the result of the get method is cast as an object
			var castAsObject = Expression.TypeAs(propertyAccess, typeof(object));

			// Create a lambda expression for the property access and compile it
			var lamda = Expression.Lambda<Func<object, object>>(castAsObject, parameter);
			return lamda.Compile();
		}
	}
}
