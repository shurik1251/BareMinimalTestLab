﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using TestBase.Helpers;

namespace TestBase.BulkCopy
{
	internal sealed class ObjectDataReader<TData> : IDataReader
	{
		/// <summary>
		/// The enumerator for the IEnumerable{TData} passed to the constructor for 
		/// this instance.
		/// </summary>
		private IEnumerator<TData> _dataEnumerator;

		/// <summary>
		/// The lookup of accessor functions for the properties on the TData type.
		/// </summary>
		private readonly Func<TData, object>[] _accessors;

		/// <summary>
		/// The lookup of property names against their ordinal positions.
		/// </summary>
		private readonly Dictionary<string, int> _ordinalLookup;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectDataReader&lt;TData&gt;"/> class.
		/// </summary>
		/// <param name="data">The data this instance should enumerate through.</param>
		public ObjectDataReader(IEnumerable<TData> data)
		{
			_dataEnumerator = data.GetEnumerator();

			// Get all the readable properties for the class and
			// compile an expression capable of reading it
			var propertyAccessors = typeof(TData)
				.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(p => p.CanRead)
				.Select((p, i) => new
				{
					Index = i,
					Property = p,
					Accessor = ReflectionHelper.CreatePropertyAccessor<TData>(p)
				})
				.ToArray();

			_accessors = propertyAccessors.Select(p => p.Accessor).ToArray();
			_ordinalLookup = propertyAccessors.ToDictionary(
				p => p.Property.Name,
				p => p.Index,
				StringComparer.OrdinalIgnoreCase);
		}

		#region IDataReader Members

		public void Close()
		{
			Dispose();
		}

		public int Depth
		{
			get { return 1; }
		}

		public DataTable GetSchemaTable()
		{
			return null;
		}

		public bool IsClosed
		{
			get { return _dataEnumerator == null; }
		}

		public bool NextResult()
		{
			return false;
		}

		public bool Read()
		{
			if (_dataEnumerator == null)
			{
				throw new ObjectDisposedException("ObjectDataReader");
			}

			return _dataEnumerator.MoveNext();
		}

		public int RecordsAffected
		{
			get { return -1; }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_dataEnumerator != null)
				{
					_dataEnumerator.Dispose();
					_dataEnumerator = null;
				}
			}
		}

		#endregion

		#region IDataRecord Members

		public int FieldCount
		{
			get { return _accessors.Length; }
		}

		public bool GetBoolean(int i)
		{
			throw new NotImplementedException();
		}

		public byte GetByte(int i)
		{
			throw new NotImplementedException();
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public char GetChar(int i)
		{
			throw new NotImplementedException();
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public IDataReader GetData(int i)
		{
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i)
		{
			throw new NotImplementedException();
		}

		public DateTime GetDateTime(int i)
		{
			throw new NotImplementedException();
		}

		public decimal GetDecimal(int i)
		{
			throw new NotImplementedException();
		}

		public double GetDouble(int i)
		{
			throw new NotImplementedException();
		}

		public Type GetFieldType(int i)
		{
			throw new NotImplementedException();
		}

		public float GetFloat(int i)
		{
			throw new NotImplementedException();
		}

		public Guid GetGuid(int i)
		{
			throw new NotImplementedException();
		}

		public short GetInt16(int i)
		{
			throw new NotImplementedException();
		}

		public int GetInt32(int i)
		{
			throw new NotImplementedException();
		}

		public long GetInt64(int i)
		{
			throw new NotImplementedException();
		}

		public string GetName(int i)
		{
			throw new NotImplementedException();
		}

		public int GetOrdinal(string name)
		{
			int ordinal;
			if (!_ordinalLookup.TryGetValue(name, out ordinal))
			{
				throw new InvalidOperationException("Unknown parameter name " + name);
			}

			return ordinal;
		}

		public string GetString(int i)
		{
			throw new NotImplementedException();
		}

		public object GetValue(int i)
		{
			if (_dataEnumerator == null)
			{
				throw new ObjectDisposedException("ObjectDataReader");
			}

			return _accessors[i](_dataEnumerator.Current);
		}

		public int GetValues(object[] values)
		{
			throw new NotImplementedException();
		}

		public bool IsDBNull(int i)
		{
			throw new NotImplementedException();
		}

		public object this[string name]
		{
			get { throw new NotImplementedException(); }
		}

		public object this[int i]
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}
