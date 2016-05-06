using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public class SceneManager : OgreNativeObject
	{
		internal SceneManager(IntPtr handle) : base(handle)
		{

		}

		public Camera CreateCamera(string name, bool notShadowCaster = true, bool forCubemapping = false)
		{
			return Runtime.LookupObject(
				SceneManager_createCamera(_handle, name ?? string.Empty, notShadowCaster, forCubemapping),
				(ptr) => new Camera(ptr));
		}

		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr SceneManager_createCamera(IntPtr handle, string name, bool notShadowCaster, bool forCubemapping);

		#endregion PInvoke
	}

	/// <summary>
	/// Classification of a scene to allow a decision of what type of
	/// SceneManager to provide back to the application.
	/// </summary>
	[Flags]
	public enum SceneType
	{
		Generic = 1,
		ExteriorClose = 2,
		ExteriorFar = 4,
		ExteriorRealFar = 8,
		Interior = 16
	}
}
