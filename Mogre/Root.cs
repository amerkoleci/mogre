using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public sealed class Root : OgreNativeObject
	{
		public Root(string pluginFileName = "plugins.cfg", string configFileName = "ogre.cfg", string logFileName = "Ogre.log")
			: base(Root_New(pluginFileName, configFileName, logFileName))
		{

		}

		protected override void NativeDelete()
		{
			Root_Delete(_handle);
		}

		#region PInvoke

		// FbxManager
		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_New(string pluginFileName, string configFileName, string logFileName);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void Root_Delete(IntPtr handle);

		#endregion
	}
}
