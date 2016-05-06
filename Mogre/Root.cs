using System;
using System.Runtime.InteropServices;

namespace Mogre
{
	public sealed class Root : OgreNativeObject
	{
		public Root()
			: this("plugins.cfg", "ogre.cfg", "Ogre.log")
		{

		}

		public Root(string pluginFileName)
			: this(pluginFileName, "ogre.cfg", "Ogre.log")
		{

		}

		public Root(string pluginFileName, string configFileName)
			: this(pluginFileName, configFileName, "Ogre.log")
		{

		}

		public Root(string pluginFileName, string configFileName, string logFileName)
			: base(Root_new(pluginFileName, configFileName, logFileName))
		{

		}

		public RenderSystem RenderSystem
		{
			get
			{
				return Runtime.LookupObject(Root_getRenderSystem(_handle), (ptr) => new RenderSystem(ptr));
			}
			set
			{
				Root_setRenderSystem(_handle, value != null ? value.NativeHandle : IntPtr.Zero);
			}
		}

		public CompositorManager2 CompositorManager2
		{
			get
			{
				return Runtime.LookupObject(Root_getCompositorManager2(_handle), (ptr) => new CompositorManager2(ptr));
			}
		}

		protected override void NativeDelete()
		{
			Root_delete(_handle);
		}

		public void Shutdown()
		{
			Root_shutdown(_handle);
		}

		public void StartRendering()
		{
			Root_startRendering(_handle);
		}

		public bool RenderOneFrame()
		{
			return Root_renderOneFrame(_handle);
		}

		public bool RenderOneFrame(float timeSinceLastFrame)
		{
			return Root_renderOneFrame2(_handle, timeSinceLastFrame);
		}

		public RenderSystem GetRenderSystemByName(string name)
		{
			return Runtime.LookupObject(Root_getRenderSystemByName(_handle, name), (ptr) => new RenderSystem(ptr));
		}

		public RenderWindow Initialise(bool autoCreateWindow, string windowTitle = "")
		{
			return Runtime.LookupObject(Root_initialise(_handle, autoCreateWindow, windowTitle), (ptr) => new RenderWindow(ptr));
		}

		public RenderWindow Initialise(bool autoCreateWindow, string windowTitle, string customCapabilitiesConfig)
		{
			return Runtime.LookupObject(Root_initialise2(_handle, autoCreateWindow, windowTitle, customCapabilitiesConfig), (ptr) => new RenderWindow(ptr));
		}

		public RenderWindow CreateRenderWindow(string name, int width = 640, int height = 480, bool fullscreen = false)
		{
			return Runtime.LookupObject(
				Root_createRenderWindow(_handle, name, width, height, fullscreen),
				(ptr) => new RenderWindow(ptr));
		}

		public RenderWindow CreateRenderWindow(string name, IntPtr windowHandle, int width = 640, int height = 480, bool fullscreen = false)
		{
			return Runtime.LookupObject(
				Root_createRenderWindow2(_handle, name, width, height, fullscreen, windowHandle),
				(ptr) => new RenderWindow(ptr));
		}

		public SceneManager CreateSceneManager(SceneType type, int numWorkerThreads = -1, InstancingThreadedCullingMethod threadedCullingMethod = InstancingThreadedCullingMethod.SingleThread, string instanceName = "")
		{
			if (numWorkerThreads < 0)
			{
				numWorkerThreads = PlatformInformation.NumLogicalCores;
				if (numWorkerThreads > 1)
				{
					threadedCullingMethod = InstancingThreadedCullingMethod.Threaded;
				}
			}

			return Runtime.LookupObject(
				Root_createSceneManager2(_handle, (ushort)type, (uint)numWorkerThreads, threadedCullingMethod, instanceName),
				(ptr) => new SceneManager(ptr));
		}

		public SceneManager CreateSceneManager(string typeName, int numWorkerThreads = -1, InstancingThreadedCullingMethod threadedCullingMethod = InstancingThreadedCullingMethod.SingleThread, string instanceName = "")
		{
			if (numWorkerThreads < 0)
			{
				numWorkerThreads = PlatformInformation.NumLogicalCores;
				if (numWorkerThreads > 1)
				{
					threadedCullingMethod = InstancingThreadedCullingMethod.Threaded;
				}
			}

			return Runtime.LookupObject(
				Root_createSceneManager(_handle, typeName, (uint)numWorkerThreads, threadedCullingMethod, instanceName),
				(ptr) => new SceneManager(ptr));
		}


		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_new(string pluginFileName, string configFileName, string logFileName);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void Root_delete(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void Root_shutdown(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void Root_startRendering(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_getRenderSystemByName(IntPtr handle, string name);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_getRenderSystem(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void Root_setRenderSystem(IntPtr handle, IntPtr value);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_initialise(IntPtr handle, [MarshalAs(UnmanagedType.U1)] bool autoCreateWindow, string windowTitle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_initialise2(IntPtr handle, [MarshalAs(UnmanagedType.U1)] bool autoCreateWindow, string windowTitle, string caps);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_createRenderWindow(IntPtr handle, string name, int width, int height, [MarshalAs(UnmanagedType.U1)] bool fullscreen);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_createRenderWindow2(IntPtr handle, string name, int width, int height, [MarshalAs(UnmanagedType.U1)] bool fullscreen, IntPtr windowHandle);

		[return: MarshalAs(UnmanagedType.U1)]
		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern bool Root_renderOneFrame(IntPtr handle);

		[return: MarshalAs(UnmanagedType.U1)]
		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern bool Root_renderOneFrame2(IntPtr handle, float timeSinceLastFrame);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_createSceneManager(IntPtr _this, string typeName, uint numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod, string instanceName);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_createSceneManager2(IntPtr _this, ushort typeMask, uint numWorkerThreads, InstancingThreadedCullingMethod threadedCullingMethod, string instanceName);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr Root_getCompositorManager2(IntPtr handle);

		#endregion PInvoke
	}

	public enum InstancingThreadedCullingMethod
	{
		SingleThread,
		Threaded,
	}
}
