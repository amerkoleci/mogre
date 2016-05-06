using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public class Camera : Frustum
	{
		internal Camera(IntPtr handle) : base(handle)
		{

		}

		public float NearClipDistance
		{
			get { return Camera_getNearClipDistance(_handle); }
			set { Camera_setNearClipDistance(_handle, value); }
		}

		public float FarClipDistance
		{
			get { return Camera_getFarClipDistance(_handle); }
			set { Camera_setFarClipDistance(_handle, value); }
		}

		#region PInvoke
		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern float Camera_getNearClipDistance(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void Camera_setNearClipDistance(IntPtr handle, float value);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern float Camera_getFarClipDistance(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void Camera_setFarClipDistance(IntPtr handle, float value);

		#endregion
	}
}
