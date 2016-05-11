using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public class SceneManager : OgreNativeObject
	{
		internal SceneManager(IntPtr handle) : base(handle)
		{

		}

		public SceneNode RootSceneNode
		{
			get
			{
				return Runtime.LookupObject(SceneManager_getRootSceneNode(_handle), (ptr) => new SceneNode(ptr));
			}
		}

		public Camera CreateCamera(string name, bool notShadowCaster = true, bool forCubemapping = false)
		{
			return Runtime.LookupObject(
				SceneManager_createCamera(_handle, name ?? string.Empty, notShadowCaster, forCubemapping),
				(ptr) => new Camera(ptr));
		}

		public Entity CreateEntity(string meshName, string groupName, SceneMemoryManagerTypes sceneType = SceneMemoryManagerTypes.Dynamic)
		{
			Contract.ArgumentNotNull(meshName, nameof(meshName));

			return Runtime.LookupObject(
				SceneManager_createEntity(_handle, meshName, groupName ?? "AutoDetect", sceneType),
				(ptr) => new Entity(ptr));
		}

		public Entity CreateEntity(string meshName)
		{
			Contract.ArgumentNotNull(meshName, nameof(meshName));

			return Runtime.LookupObject(
				SceneManager_createEntity(_handle, meshName, "AutoDetect", SceneMemoryManagerTypes.Dynamic),
				(ptr) => new Entity(ptr));
		}

		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr SceneManager_getRootSceneNode(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr SceneManager_createEntity(IntPtr handle, string meshName, string groupName, SceneMemoryManagerTypes sceneType);

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
