using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public class ConfigFile : OgreNativeObject
	{
		public ConfigFile() : base(ConfigFile_new())
		{

		}

		protected override void NativeDelete()
		{
			ConfigFile_delete(_handle);
		}

		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr ConfigFile_new();

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void ConfigFile_delete(IntPtr handle);

		#endregion
	}
}
