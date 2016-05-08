using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public class SceneNode : Node
	{
		internal SceneNode(IntPtr handle) : base(handle)
		{

		}

		public SceneNode CreateChildSceneNode(SceneMemoryManagerTypes sceneType, Vector3 translate, Quaternion rotate)
		{
			return new SceneNode(SceneNode_createChildSceneNode(_handle, sceneType, ref translate, ref rotate));
		}

		public SceneNode CreateChildSceneNode(SceneMemoryManagerTypes sceneType, Vector3 translate)
		{
			return CreateChildSceneNode(sceneType, translate, Quaternion.Identity);
		}

		public SceneNode CreateChildSceneNode(SceneMemoryManagerTypes sceneType = SceneMemoryManagerTypes.Dynamic)
		{
			return CreateChildSceneNode(sceneType, Vector3.Zero, Quaternion.Identity);
		}

		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr SceneNode_createChildSceneNode(IntPtr handle, SceneMemoryManagerTypes sceneType, ref Vector3 translate, ref Quaternion rotate);

		#endregion
	}

	public enum SceneMemoryManagerTypes
	{
		Dynamic = 0,
		Static
	}
}
