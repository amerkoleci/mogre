using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public sealed class RenderSystem : OgreNativeObject
	{
		internal RenderSystem(IntPtr handle) : base(handle)
		{

		}

		public void SetConfigOption(string name, string value)
		{
			RenderSystem_setConfigOption(_handle, name, value ?? string.Empty);
		}

		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void RenderSystem_setConfigOption(IntPtr handle, string name, string value);

		#endregion PInvoke
	}
}
