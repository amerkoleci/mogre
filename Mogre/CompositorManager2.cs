using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public class CompositorManager2 : OgreNativeObject
	{
		internal CompositorManager2(IntPtr handle) : base(handle)
		{

		}

		public bool HasWorkspaceDefinition(string name)
		{
			return CompositorManager2_hasWorkspaceDefinition(_handle, name);
		}

		public void CreateBasicWorkspaceDef(string name, Color4 backgroundColor)
		{
			CompositorManager2_createBasicWorkspaceDef(_handle, name, ref backgroundColor);
		}

		/*public CompositorWorkspace AddWorkspace(SceneManager scene, RenderTarget finalRenderTarget, Camera defaultCamera, string name, bool enabled)
		{
			return Runtime.LookupObject(
				CompositorManager2_addWorkspace(_handle, 
				scene != null ? scene.NativeHandle : IntPtr.Zero,
				finalRenderTarget != null ? finalRenderTarget.NativeHandle : IntPtr.Zero,
				defaultCamera != null ? defaultCamera.NativeHandle : IntPtr.Zero,
				name,
				enabled
				),
				(ptr) => new CompositorWorkspace(ptr));
		}*/

		#region PInvoke
		[return: MarshalAs(UnmanagedType.U1)]
		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern bool CompositorManager2_hasWorkspaceDefinition(IntPtr handle, string name);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void CompositorManager2_createBasicWorkspaceDef(IntPtr handle, string name, ref Color4 backgroundColor);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr CompositorManager2_addWorkspace(IntPtr handle, IntPtr scene, IntPtr finalRenderTarget, IntPtr defaultCamera, string name, bool enabled);
		#endregion
	}
}
