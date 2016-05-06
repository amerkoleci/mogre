using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public class FileSystemLayer : OgreNativeObject
	{
		public FileSystemLayer() : base(FileSystemLayer_new())
		{

		}

		protected override void NativeDelete()
		{
			FileSystemLayer_delete(_handle);
		}

		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr FileSystemLayer_new();

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void FileSystemLayer_delete(IntPtr handle);

		#endregion
	}
}
