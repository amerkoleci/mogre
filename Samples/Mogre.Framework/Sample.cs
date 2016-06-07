// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Mogre.RTShader;
using System;
using System.Windows.Forms;

namespace Mogre.Framework
{
	/// <summary>
	/// Base class for samples
	/// </summary>
	public abstract class Sample : Form
	{
		protected readonly FileSystemLayer _fileSystemLayer;
		protected Root _root;
		protected OverlaySystem _overlaySystem;
		protected RenderWindow _window;
		protected SceneManager _sceneManager;
		protected Camera _camera;
		protected CompositorWorkspace _workspace;
		protected ColourValue _backgroundColor = ColourValue.Black;
		

		public Camera Camera
		{
			get { return _camera; }
		}

		protected Sample()
		{
			_fileSystemLayer = new FileSystemLayer();
			base.SuspendLayout();
			ClientSize = new System.Drawing.Size(800, 600);
			FormBorderStyle = FormBorderStyle.Fixed3D;
			MaximizeBox = false;
			Name = "OgreWindow";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Mogre Render Window";
			ResumeLayout(false);
		}

		public virtual bool Setup()
		{
			// Create root
			var pluginFileName = _fileSystemLayer.GetConfigFilePath("plugins.cfg");
			_root = new Root(pluginFileName,
				_fileSystemLayer.GetWritablePath("ogre.cfg"),
				_fileSystemLayer.GetWritablePath("Ogre.log"));

			// Create OverlaySystem
			_overlaySystem = new OverlaySystem();

			// 
			SetupResources();
			//if (Configure() == false)
			//{
			//	return false;
			//}
			SetupDirectX();

			// Create RenderWindow
			_root.Initialise(false);
			var nameValuePairList = new NameValuePairList();
			nameValuePairList["externalWindowHandle"] = Handle.ToString();
			_window = _root.CreateRenderWindow("Mogre RenderWindow", 800, 600, false, nameValuePairList);

			TestCapabilities(_root.RenderSystem.Capabilities);

			// Now we have GPU stuff setup
			ResourceGroupManager.Singleton.AddBuiltinLocations();

			CreateSceneManager();
			//InitializeRTShaderSystem(_sceneManager);
			_overlaySystem.SceneManager = _sceneManager;
			CreateCamera();
			_workspace = SetupCompositor();
			TextureManager.Singleton.DefaultNumMipmaps = 5;
			CreateResourceListener();
			LoadResources();
			CreateInputHandler();
			Disposed += OgreWindow_Disposed;
			CreateScene();

			// Create Frame Listeners
			_root.FrameStarted += OnFrameStarted;
			_root.FrameRenderingQueued += OnFrameRenderingQueued;
			_root.FrameEnded += OnFrameEnded;

			//this.CreateInput();
			return true;
		}


		protected virtual bool OnFrameStarted(FrameEvent evt)
		{
			return true;
		}

		protected virtual bool OnFrameRenderingQueued(FrameEvent evt)
		{
			return true;
		}

		protected virtual bool OnFrameEnded(FrameEvent evt)
		{
			// quit if window was closed
			if (_window.IsDisposed || _window.IsClosed) return false;

			return true;
		}

		protected virtual void TestCapabilities(RenderSystemCapabilities caps)
		{

		}

		protected virtual void SetupResources()
		{
			using (var configFile = new ConfigFile())
			{
				configFile.Load(_fileSystemLayer.GetConfigFilePath("resources.cfg"));
				ConfigFile.SectionIterator sectionIterator = configFile.GetSectionIterator();
				while (sectionIterator.MoveNext())
				{
					string currentKey = sectionIterator.CurrentKey;
					foreach (var pair in sectionIterator.Current)
					{
						ResourceGroupManager.Singleton.AddResourceLocation(pair.Value, pair.Key, currentKey);
					}
				}
			}
		}

		protected virtual void CreateInputHandler()
		{
			new DefaultInputHandler(this);
		}

		protected virtual void CreateSceneManager()
		{
#if DEBUG
			//Debugging multithreaded code is a PITA, disable it.
			const int numThreads = 1;
			InstancingThreadedCullingMethod threadedCullingMethod = InstancingThreadedCullingMethod.SingleThread;
#else
            // GetNumLogicalCores() may return 0 if couldn't detect
            var numThreads = Math.Max( 1, PlatformInformation.NumLogicalCores );

			InstancingThreadedCullingMethod threadedCullingMethod = InstancingThreadedCullingMethod.SingleThread;

            //See doxygen documentation regarding culling methods.
            //In some cases you may still want to use single thread.
            if( numThreads > 1 )
                threadedCullingMethod = InstancingThreadedCullingMethod.Threaded;
#endif

			_sceneManager = _root.CreateSceneManager(SceneType.Generic, numThreads, threadedCullingMethod);
		}

		protected virtual void CreateCamera()
		{
			_camera = _sceneManager.CreateCamera("PlayerCam");
			_camera.Position = new Vector3(0f, 0f, 500f);
			_camera.LookAt(new Vector3(0f, 0f, -300f));
			_camera.AutoAspectRatio = true;
			_camera.NearClipDistance = 5.0f;
		}

		protected virtual CompositorWorkspace SetupCompositor()
		{
			CompositorManager2 compositorManager = _root.CompositorManager2;
			string workspaceName = GetType().Name + "Workspace";
			if (!compositorManager.HasWorkspaceDefinition(workspaceName))
			{
				compositorManager.CreateBasicWorkspaceDef(workspaceName, _backgroundColor);
			}

			return compositorManager.AddWorkspace(_sceneManager, _window, _camera, workspaceName);
		}

		protected virtual bool InitializeRTShaderSystem(SceneManager sceneMgr)
		{
			if (ShaderGenerator.Initialize())
			{
				ShaderGenerator.Singleton.AddSceneManager(sceneMgr);
			}

			return true;
		}

		protected virtual void CreateResourceListener()
		{
		}

		protected virtual void LoadResources()
		{
			ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
		}

		public virtual void Run()
		{
			if (!Setup())
			{
				return;
			}
			Show();

			while (_root != null && _root.RenderOneFrame())
			{
				Application.DoEvents();
			}
		}

		void Shutdown()
		{
			DestroyScene();

			if (_overlaySystem != null)
			{
				_overlaySystem.Dispose();
				_overlaySystem = null;
			}

			_root.FrameStarted -= OnFrameStarted;
			_root.FrameRenderingQueued -= OnFrameRenderingQueued;
			_root.FrameEnded -= OnFrameEnded;
			_root.Dispose();
			_root = null;

			_window = null;
			_camera = null;
			_workspace = null;
			_sceneManager = null;
		}

		private void OgreWindow_Disposed(object sender, EventArgs e)
		{
			Shutdown();
		}

		private void SetupDirectX()
		{
			RenderSystem renderSystemByName = _root.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
			_root.RenderSystem = renderSystemByName;
			renderSystemByName.SetConfigOption("Full Screen", "No");
			renderSystemByName.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
		}

		protected virtual bool Configure()
		{
			if (_root.RestoreConfig() || _root.ShowConfigDialog())
			{
				return true;
			}

			return false;
		}

		protected abstract void CreateScene();

		protected virtual void DestroyScene()
		{
		}

		protected void TakeScreenshot()
		{
			string[] temp = System.IO.Directory.GetFiles(Environment.CurrentDirectory, "screenshot*.jpg");
			string fileName = string.Format("screenshot{0}.jpg", temp.Length + 1);

			TakeScreenshot(fileName);
		}

		protected void TakeScreenshot(string fileName)
		{
			_window.WriteContentsToFile(fileName);
		}
	}
}
