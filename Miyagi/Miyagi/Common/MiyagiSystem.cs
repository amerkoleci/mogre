/*
 Miyagi v1.2.1
 Copyright (c) 2008 - 2012 Tobias Bohnen

 Permission is hereby granted, free of charge, to any person obtaining a copy of this
 software and associated documentation files (the "Software"), to deal in the Software
 without restriction, including without limitation the rights to use, copy, modify, merge,
 publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 to whom the Software is furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all copies or
 substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 DEALINGS IN THE SOFTWARE.
 */
namespace Miyagi.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    using Miyagi.Common.Plugins;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Scripting;
    using Miyagi.Common.Serialization;
    using Miyagi.Internals;
    using Miyagi.TwoD;
    using Miyagi.UI;

    /// <summary>
    /// The root class of Miyagi.
    /// </summary>
    public sealed class MiyagiSystem : IDisposable, ISynchronizeInvoke
    {
        #region Fields

        private static readonly object PadLock = new object();

        private bool firstUpdate = true;
        private Thread instanceThread;
        private InvokeHelper invokeHelper;
        private ManagerCollection managers;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MiyagiSystem class.
        /// </summary>
        public MiyagiSystem(int width, int height)
            : this("*", width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiyagiSystem"/> class.
        /// </summary>
        /// <param name="backendName">Name of the backend.</param>
        public MiyagiSystem(string backendName, int width, int height)
        {
            string path = Environment.CurrentDirectory;
            if (path == null)
            {
                throw new Exception("invalid assembly location");
            }

            var files = Directory.GetFiles(path, "Miyagi.Backend." + backendName + ".dll");
            if (files.Length < 1)
            {
                throw new ArgumentException("Backend not found.", "backendName");
            }

            this.Backend = this.LoadBackend(
                Assembly.LoadFrom(Directory.GetFiles(path, "Miyagi.Backend." + backendName + ".dll")[0]),
                null,
                width,
                height);
            this.Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiyagiSystem"/> class.
        /// </summary>
        /// <param name="backend">The backend.</param>
        public MiyagiSystem(Backend backend)
        {
            this.Backend = backend;
            this.Init();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs after the root has been updated.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Occues before the root has been updated.
        /// </summary>
        public event EventHandler Updating;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the rendering back-end.
        /// </summary>
        public Backend Backend
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the GUIManager.
        /// </summary>
        public GUIManager GUIManager
        {
            get
            {
                this.ThrowIfDisposed();

                if (!this.managers.Contains("GUI") && !this.IsDisposed)
                {
                    this.RegisterManager(new GUIManager(this));
                }

                return (GUIManager)this.managers["GUI"];
            }
        }

        /// <summary>
        /// Gets the InputManager.
        /// </summary>
        public InputManager InputManager
        {
            get
            {
                this.ThrowIfDisposed();

                if (!this.managers.Contains("Input") && !this.IsDisposed)
                {
                    this.RegisterManager(new InputManager(this));
                }

                return (InputManager)this.managers["Input"];
            }
        }

        /// <summary>
        /// Gets a value indicating whether a invoke method must be called.
        /// </summary>
        public bool InvokeRequired
        {
            get
            {
                return this.InvokeHelper.InvokeRequired;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the LocaleManager.
        /// </summary>
        public LocaleManager LocaleManager
        {
            get
            {
                this.ThrowIfDisposed();

                if (!this.managers.Contains("Locale") && !this.IsDisposed)
                {
                    this.RegisterManager(new LocaleManager(this));
                }

                return (LocaleManager)this.managers["Locale"];
            }
        }

        /// <summary>
        /// Gets the collection of managers.
        /// </summary>
        public ReadOnlyCollection<IManager> Managers
        {
            get
            {
                this.ThrowIfDisposed();
                return new ReadOnlyCollection<IManager>(this.managers);
            }
        }

        /// <summary>
        /// Gets the PluginManager.
        /// </summary>
        public PluginManager PluginManager
        {
            get
            {
                this.ThrowIfDisposed();

                if (!this.managers.Contains("Plugin") && !this.IsDisposed)
                {
                    this.RegisterManager(new PluginManager(this));
                }

                return (PluginManager)this.managers["Plugin"];
            }
        }

        /// <summary>
        /// Gets the RenderManager.
        /// </summary>
        public RenderManager RenderManager
        {
            get
            {
                this.ThrowIfDisposed();

                if (!this.managers.Contains("Render") && !this.IsDisposed)
                {
                    this.RegisterManager(this.Backend.RenderManager);
                }

                return (RenderManager)this.managers["Render"];
            }
        }

        /// <summary>
        /// Gets the ScriptingManager.
        /// </summary>
        public ScriptingManager ScriptingManager
        {
            get
            {
                this.ThrowIfDisposed();

                if (!this.managers.Contains("Scripting") && !this.IsDisposed)
                {
                    this.RegisterManager(new ScriptingManager(this));
                }

                return (ScriptingManager)this.managers["Scripting"];
            }
        }

        /// <summary>
        /// Gets the SerializationManager.
        /// </summary>
        public SerializationManager SerializationManager
        {
            get
            {
                this.ThrowIfDisposed();

                if (!this.managers.Contains("Serialization") && !this.IsDisposed)
                {
                    this.RegisterManager(new SerializationManager(this));
                }

                return (SerializationManager)this.managers["Serialization"];
            }
        }

        /// <summary>
        /// Gets the TwoDManager.
        /// </summary>
        public TwoDManager TwoDManager
        {
            get
            {
                this.ThrowIfDisposed();

                if (!this.managers.Contains("TwoD") && !this.IsDisposed)
                {
                    this.RegisterManager(new TwoDManager(this));
                }

                return (TwoDManager)this.managers["TwoD"];
            }
        }

        #endregion Public Properties

        #region Internal Static Properties

        internal static MiyagiSystem Latest
        {
            get;
            private set;
        }

        #endregion Internal Static Properties

        #region Internal Properties

        internal DateTime LastUpdate
        {
            get;
            private set;
        }

        internal TimeSpan TimeSinceLastUpdate
        {
            get;
            private set;
        }

        #endregion Internal Properties

        #region Private Properties

        private InvokeHelper InvokeHelper
        {
            get
            {
                return this.invokeHelper ?? (this.invokeHelper = new InvokeHelper(this.instanceThread));
            }
        }

        #endregion Private Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Executes the specified delegate asynchronously.
        /// </summary>
        /// <param name="method">The delegate to execute.</param>
        /// <param name="args">An array of objects to pass as arguments to the given method.</param>
        /// <returns>An IAsyncResult representing the result of the BeginInvoke operation.</returns>
        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            lock (PadLock)
            {
                this.ThrowIfDisposed();
                return this.InvokeHelper.BeginInvoke(method, args);
            }
        }

        /// <summary>
        /// Executes the specified action asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>An IAsyncResult representing the result of the BeginInvoke operation.</returns>
        public IAsyncResult BeginInvoke(Action action)
        {
            return this.BeginInvoke(action, null);
        }

        /// <summary>
        /// Executes the specified delegate asynchronously.
        /// </summary>
        /// <param name="method">The delegate to execute.</param>
        /// <returns>An IAsyncResult representing the result of the BeginInvoke operation.</returns>
        public IAsyncResult BeginInvoke(Delegate method)
        {
            return this.BeginInvoke(method, null);
        }

        /// <summary>
        /// Destroys the MiyagiSystem and disposes all components.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            // dispose managers
            while (this.managers.Count > 0)
            {
                IManager manager = this.managers[0];
                this.UnregisterManager(manager);
            }

            this.Backend = null;

            // get rid of events
            this.Updating = null;
            this.Updated = null;

            this.instanceThread = null;
            this.invokeHelper = null;

            this.IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Retrieves the return value of the asynchronous operation.
        /// </summary>
        /// <param name="result">An IAsyncResult representing the result of a previous BeginInvoke operation.</param>
        /// <returns>The Object  generated by the asynchronous operation.</returns>
        public object EndInvoke(IAsyncResult result)
        {
            lock (PadLock)
            {
                this.ThrowIfDisposed();
                return this.InvokeHelper.EndInvoke(result);
            }
        }

        /// <summary>
        /// Gets a manager by type.
        /// </summary>
        /// <param name="type">The type of the manager.</param>
        /// <returns>The manager of the specified type if it exists; otherwise, null.</returns>
        public IManager GetManager(string type)
        {
            this.ThrowIfDisposed();

            switch (type)
            {
                case "GUI":
                    return this.GUIManager;
                case "Input":
                    return this.InputManager;
                case "Locale":
                    return this.LocaleManager;
                case "Plugin":
                    return this.PluginManager;
                case "Render":
                    return this.RenderManager;
                case "Scripting":
                    return this.ScriptingManager;
                case "Serialization":
                    return this.SerializationManager;
                case "TwoD":
                    return this.TwoDManager;
            }

            return this.HasManager(type) ? this.managers[type] : null;
        }

        /// <summary>
        /// Gets the SerializationData.
        /// </summary>
        /// <returns>The SerializationData.</returns>
        public IDictionary<IManager, SerializationData> GetSerializationData()
        {
            this.ThrowIfDisposed();
            var retValue = new Dictionary<IManager, SerializationData>();

            foreach (IManager manager in this.Managers)
            {
                var data = new SerializationData();
                manager.SaveSerializationData(data);
                retValue[manager] = data;
            }

            return retValue;
        }

        /// <summary>
        /// Returns whether a manager of the specified type has been registered.
        /// </summary>
        /// <param name="type">The type of the manager.</param>
        /// <returns><c>true</c> if there is a manager of the specified type; otherwise, <c>false</c>.</returns>
        public bool HasManager(string type)
        {
            this.ThrowIfDisposed();
            return this.managers.Contains(type);
        }

        /// <summary>
        /// Executes the specified delegate synchronously.
        /// </summary>
        /// <param name="method">The delegate to execute.</param>
        /// <param name="args">An array of objects to pass as arguments to the given method.</param>
        /// <returns>An object representing the result of the invocation.</returns>
        public object Invoke(Delegate method, object[] args)
        {
            lock (PadLock)
            {
                this.ThrowIfDisposed();
                return this.InvokeHelper.Invoke(method, args);
            }
        }

        /// <summary>
        /// Executes the specified action synchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void Invoke(Action action)
        {
            this.Invoke(action, null);
        }

        /// <summary>
        /// Executes the specified delegate synchronously.
        /// </summary>
        /// <param name="method">The delegate to execute.</param>
        /// <returns>An object representing the result of the invocation.</returns>
        public object Invoke(Delegate method)
        {
            return this.Invoke(method, null);
        }

        /// <summary>
        /// Registers a manager.
        /// </summary>
        /// <param name="manager">The IManager to register.</param>
        public void RegisterManager(IManager manager)
        {
            this.ThrowIfDisposed();

            if (this.managers.Contains(manager.Type))
            {
                this.UnregisterManager(manager);
            }

            foreach (var mgr in this.managers)
            {
                mgr.NotifyManagerRegistered(manager);
            }

            this.managers.Add(manager);
            manager.Initialize();
        }

        /// <summary>
        /// Unregisters a manager.
        /// </summary>
        /// <param name="manager">The IManager to unregister.</param>
        public void UnregisterManager(IManager manager)
        {
            if (this.managers.Contains(manager.Type))
            {
                this.managers.Remove(manager.Type);
                manager.Dispose();
            }
        }

        /// <summary>
        /// Updates the MiyagiSystem.
        /// </summary>
        public void Update()
        {
            this.ThrowIfDisposed();

            if (this.Updating != null)
            {
                this.Updating(this, EventArgs.Empty);
            }

            // time related
            DateTime now = DateTime.Now;
            if (!this.firstUpdate)
            {
                this.TimeSinceLastUpdate = now - this.LastUpdate;
            }
            else
            {
                this.firstUpdate = false;
                this.TimeSinceLastUpdate = TimeSpan.Zero;
            }

            this.LastUpdate = now;

            // update invokes
            if (this.invokeHelper != null)
            {
                this.InvokeHelper.Update();
            }

            // update managers
            int count = this.managers.Count;
            for (int i = 0; i < count; i++)
            {
                this.managers[i].Update();

                if (this.IsDisposed)
                {
                    return;
                }
            }

            if (this.Updated != null)
            {
                this.Updated(this, EventArgs.Empty);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Init()
        {
            this.managers = new ManagerCollection();

            this.LastUpdate = DateTime.Now;
            this.instanceThread = Thread.CurrentThread;

            Latest = this;
        }

        private Backend LoadBackend(Assembly asm, object backendContext, int width, int height)
        {
            foreach (Type type in asm.GetExportedTypes())
            {
                if (type.IsSubclassOf(typeof(Backend)))
                {
                    var ci = type.GetConstructor(new[] { typeof(MiyagiSystem), typeof(int), typeof(int) });
                    var retValue = (Backend)ci.Invoke(new object[] { this, width, height });
                    retValue.SetContext(backendContext);

                    return retValue;
                }
            }

            throw new FileLoadException("Backend has no type which inherits from Miyagi.Common.Rendering.Backend.");
        }

        private void ThrowIfDisposed()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException("MiyagiSystem");
            }
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        internal sealed class ManagerCollection : KeyedCollection<string, IManager>
        {
            #region Methods

            #region Protected Methods

            protected override string GetKeyForItem(IManager item)
            {
                return item.Type;
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}