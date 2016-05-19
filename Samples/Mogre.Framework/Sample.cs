// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Mogre.RTShader;

namespace Mogre.Framework
{
    /// <summary>
    /// Base class for samples
    /// </summary>
    public abstract class Sample
    {
        protected readonly FileSystemLayer _fileSystemLayer;
        protected Root _root;
        protected RenderWindow _window;
        protected SceneManager _sceneManager;
        protected Camera _camera;

        public virtual string ThumbnailUrl
        {
            get { return string.Empty; }
        }

        protected Sample()
        {
            _fileSystemLayer = new FileSystemLayer();
        }

        public virtual bool Setup()
        {
            var pluginFileName = _fileSystemLayer.GetConfigFilePath("plugins.cfg");
            _root = new Root(pluginFileName,
                _fileSystemLayer.GetWritablePath("ogre.cfg"),
                _fileSystemLayer.GetWritablePath("Ogre.log"));

            SetupResources();
            if (Configure() == false)
            {
                return false;
            }

            // Now we have GPU stuff setup
            ResourceGroupManager.Singleton.AddBuiltinLocations();
            CreateSceneManager();
            CreateCamera();
            SetupCompositor();
            TextureManager.Singleton.DefaultNumMipmaps = 5;
            InitializeRTShaderSystem(_sceneManager);
            CreateResourceListener();
            LoadResources();
            CreateScene();
            //this.CreateFrameListener();
            //this.CreateInput();
            return true;
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
                var backgroundColor = Color4.Black;
                compositorManager.CreateBasicWorkspaceDef(workspaceName, backgroundColor);
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

            _root.StartRendering();
            DestroyScene();
            _root.Dispose();
            _root = null;
        }

        protected virtual bool Configure()
        {
            if (_root.ShowConfigDialog())
            {
                _window = _root.Initialise(true);
                return true;
            }

            return false;
        }

        protected abstract void CreateScene();

        protected virtual void DestroyScene()
        {
        }
    }
}
