using System;
using System.Diagnostics;

namespace Mogre
{
	public abstract class OgreNativeObject : IDisposable
	{
		internal IntPtr _handle;

		public IntPtr NativeHandle { get { return _handle; } }

		/// <summary>
		/// True if underlying native object is deleted
		/// </summary>
		public bool IsDeleted { get; private set; }

		protected OgreNativeObject(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentException($"Attempted to instantiate a {GetType()} with a null handle");

			_handle = handle;
			Runtime.RegisterObject(this);
		}

		~OgreNativeObject()
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

			Runtime.UnregisterObject(_handle);
			_handle = IntPtr.Zero;
		}

		protected virtual void OnDeleted()
		{
		}

		/// <summary>
		/// Try to delete underlying native object if nobody uses it (Refs==0)
		/// </summary>
		void DeleteNativeObject()
		{
			if (!IsDeleted)
			{
				try
				{
					NativeDelete();
				}
				catch (Exception exc)
				{
					//should not happen, JIC.
					throw new InvalidOperationException("Underlying native object is already deleted", exc);
				}
			}
		}

		protected virtual void NativeDelete()
		{

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
