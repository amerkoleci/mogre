// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public static class OgreLibrary
	{
		public const string LibraryName = "MogreNative.dll";
	}

	internal class WrapperString : IDisposable
	{
		readonly IntPtr _handle;
		public readonly string StringValue;

		public WrapperString(IntPtr handle)
		{
			_handle = handle;
			StringValue = Marshal.PtrToStringAnsi(handle);
		}

		void IDisposable.Dispose()
		{
			Fbx_DeleteString(_handle);
		}

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void Fbx_DeleteString(IntPtr chrArray);
	}
}
