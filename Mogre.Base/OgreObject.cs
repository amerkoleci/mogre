// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Mogre
{
	public enum OgreObjectFlags
	{
		Empty
	}

	public abstract class OgreObject : IDisposable
	{
		protected internal IntPtr _handle;

		public IntPtr Handle
		{
			get { return _handle; }
		}

		/// <summary>
		/// True if underlying native object is deleted
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool IsDeleted { get; private set; }

		protected OgreObject(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentException(string.Format("Attempted to instantiate a {0} with a null handle", GetType()));

			_handle = handle;
			//Runtime.RegisterObject(this);
		}

		internal OgreObject(OgreObjectFlags emptyFlag)
		{
		}

		~OgreObject()
		{
			DeleteNativeObject();
			Dispose(false);
		}

		public void Dispose()
		{
			DeleteNativeObject();
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (IsDeleted)
				return;

			if (disposing)
			{
				IsDeleted = true;
				OnDeleted();
			}

			//Runtime.UnregisterObject(_handle);
		}

		/// <summary>
		/// Try to delete underlying native object if nobody uses it (Refs==0)
		/// </summary>
		void DeleteNativeObject()
		{
			if (!IsDeleted)
			{
			}
		}

		protected virtual void OnDeleted()
		{
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj.GetType() != GetType())
				return false;

			var refObj = obj as OgreObject;
			if (refObj != null && refObj._handle == _handle)
				return true;

			return false;
		}

		public static bool operator ==(OgreObject left, OgreObject right)
		{
			object a = left;
			object b = right;
			if (a == null)
			{
				if (b == null)
					return true;

				return false;
			}

			if (b == null)
				return false;

			return left._handle == right._handle;
		}

		public static bool operator !=(OgreObject left, OgreObject right)
		{
			object a = left;
			object b = right;
			if (a == null)
			{
				return b != null;
			}

			if (b == null)
				return true;

			return left._handle != right._handle;
		}

		public override int GetHashCode()
		{
			if (IntPtr.Size == 8) //means 64bit
				return unchecked((int)(long)_handle);

			return (int)_handle;
		}

		public static explicit operator IntPtr(OgreObject nativeObject)
		{
			if (nativeObject != null)
				return nativeObject.Handle;

			return IntPtr.Zero;
		}

		/// <summary>
		/// Helper that throws an exception if the instance is disposed.
		/// </summary>
		[Conditional("DEBUG")]
		protected void ThrowIfDisposed()
		{
			if (IsDeleted)
				throw new InvalidOperationException("The underlying native object was deleted");

			if (_handle == IntPtr.Zero)
				throw new InvalidOperationException("Object has zero handle");
		}
	}
}
