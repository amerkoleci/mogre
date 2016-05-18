using Mogre.RTShader;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Mogre.SampleBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        SampleContext _sampleContext;

        FileSystemLayer _fileSystemLayer;
        Root _root;
        RenderWindow _window;
        SceneManager _sceneManager;
        private Camera _camera;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Setup OgreRoot first
            _fileSystemLayer = new FileSystemLayer();
            var pluginFileName = _fileSystemLayer.GetConfigFilePath("plugins.cfg");
            _root = new Root(pluginFileName,
                _fileSystemLayer.GetWritablePath("ogre.cfg"),
                _fileSystemLayer.GetWritablePath("Ogre.log"));

            SetupRenderSystem();

            _sampleContext = new SampleContext();

            MainWindow = new MainWindow();
            MainWindow.DataContext = _sampleContext;
            MainWindow.Loaded += MainWindow_Loaded;
            MainWindow.Show();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
#if TODO
            var window = (MainWindow)sender;
            var handle = window.panel.Handle;
            //var handle = new WindowInteropHelper(window).Handle;
            
            SetupRenderSystem();
            CreateRenderWindow(handle);
            InitResources();
            CreateSceneManager();
            InitializeResources();
            SetupCompositor();
            InitializeRTShaderSystem(_sceneManager);

            _sceneManager.SetSkyBox(true, "Examples/SpaceSkyBox", 5000);

            CompositionTarget.Rendering += OnCompositionTargetRendering;  
#endif // TODO
        }

        protected override void OnExit(ExitEventArgs e)
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;

            if (_camera != null)
            {
                //_camera.Dispose();
                _camera = null;
            }

            if (_sceneManager != null)
            {
                _sceneManager.Dispose();
                _sceneManager = null;
            }

            if (_window != null)
            {
                _window.Dispose();
                _window = null;
            }

            if (_root != null)
            {
                _root.Dispose();
                _root = null;
            }

            base.OnExit(e);
        }

        protected virtual void InitResources()
        {
            using (var configFile = new ConfigFile())
            {
                configFile.Load("resources.cfg");
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

            ResourceGroupManager.Singleton.AddBuiltinLocations();
        }

        private void SetupRenderSystem()
        {
            const string RenderSystemName = "Direct3D9 Rendering Subsystem";
            //const string RenderSystemName = "Direct3D11 Rendering Subsystem";
            //const string RenderSystemName = "OpenGL Rendering Subsystem";

            RenderSystem renderSystemByName = _root.GetRenderSystemByName(RenderSystemName);
            _root.RenderSystem = renderSystemByName;
            renderSystemByName.SetConfigOption("Full Screen", "No");
            renderSystemByName.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
        }

        protected virtual void CreateRenderWindow(IntPtr handle)
        {
            _root.Initialise(false);
            if (handle != IntPtr.Zero)
            {
                _window = _root.CreateRenderWindow("Test RenderWindow", handle, 800, 600);
                return;
            }

            _window = _root.CreateRenderWindow("Test RenderWindow", 800, 600);
        }

        private static void InitializeResources()
        {
            //TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
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
#if RTSHADER_SYSTEM
			mShaderGenerator->addSceneManager(_sceneManager);
#endif
            //if (mOverlaySystem)
            //	mSceneMgr->addRenderQueueListener(mOverlaySystem);

            // setup default viewport layout and camera
            _camera = _sceneManager.CreateCamera("MainCamera");
            _camera.AutoAspectRatio = true;
            _camera.NearClipDistance = 5.0f;
        }

        CompositorWorkspace SetupCompositor()
        {
            CompositorManager2 compositorManager = _root.CompositorManager2;
            const string workspaceName = "TestWorkspace";
            if (!compositorManager.HasWorkspaceDefinition(workspaceName))
            {
                var backgroundColor = Color4.Red;
                compositorManager.CreateBasicWorkspaceDef(workspaceName, backgroundColor);
            }

            return compositorManager.AddWorkspace(_sceneManager, _window, _camera, workspaceName);
        }

        bool InitializeRTShaderSystem(SceneManager sceneMgr)
        {
            if (ShaderGenerator.Initialize())
            {
                ShaderGenerator.Singleton.AddSceneManager(sceneMgr);
            }

            return true;
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            if (_root == null)
                return;

            _root.RenderOneFrame();
        }
    }
}