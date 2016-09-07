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
namespace Miyagi.Common.Plugins
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Miyagi.Common.Serialization;

    /// <summary>
    /// A PluginManager.
    /// </summary>
    public class PluginManager : IManager
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PluginManager class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        protected internal PluginManager(MiyagiSystem system)
        {
            this.MiyagiSystem = system;
            this.Plugins = new Collection<Plugin>();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the manager is disposing.
        /// </summary>
        public event EventHandler Disposing;

        #endregion Events

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the manager has been disposed.
        /// </summary>
        /// <value></value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the collection of loaded plugins.
        /// </summary>
        /// <value>A collection of loaded plugins.</value>
        public Collection<Plugin> Plugins
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the manager.
        /// </summary>
        public string Type
        {
            get
            {
                return "Plugin";
            }
        }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the MiyagiSystem.
        /// </summary>
        protected MiyagiSystem MiyagiSystem
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        void IManager.LoadSerializationData(SerializationData data)
        {
        }

        void IManager.NotifyManagerRegistered(IManager manager)
        {
        }

        void IManager.SaveSerializationData(SerializationData data)
        {
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Disposes the PluginManager.
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.Disposing != null)
            {
                this.Disposing(this, EventArgs.Empty);
            }

            this.Dispose(true);
            GC.SuppressFinalize(this);
            this.IsDisposed = true;
            this.MiyagiSystem.UnregisterManager(this);
            this.MiyagiSystem = null;
        }

        /// <summary>
        /// Gets a plugin by name.
        /// </summary>
        /// <param name="name">The name of the plugin.</param>
        /// <returns>The plugin if it exists, otherwise null.</returns>
        public Plugin GetPlugin(string name)
        {
            return this.Plugins.FirstOrDefault(plugin => plugin.Name == name);
        }

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Loads all plugins in the specified folder.
        /// </summary>
        /// <param name="folder">The folder that contains the plugins.</param>
        /// <param name="includeSubfolders">Indicates whether subfolders should also be searched.</param>
        public void LoadAllPlugins(string folder, bool includeSubfolders)
        {
            SearchOption so = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            string[] files = Directory.GetFiles(folder, "Miyagi.Common.Plugin.*.dll", so);

            foreach (string file in files)
            {
                this.LoadPlugin(file, string.Empty);
            }
        }

        /// <summary>
        /// Loads a plugin.
        /// </summary>
        /// <param name="assemblyRef">Unique identity of the plugin.</param>
        /// <param name="args">Specifies parameters which are passed to the plugin's <see cref="Plugin.NotifyLoaded"/> method.</param>
        /// <returns>The loaded plugin.</returns>
        public Plugin LoadPlugin(AssemblyName assemblyRef, params object[] args)
        {
            return this.LoadPluginCore(Assembly.Load(assemblyRef), args);
        }

        /// <summary>
        /// Loads a plugin.
        /// </summary>
        /// <param name="fileName">The filename of the plugin.</param>
        /// <param name="args">Specifies parameters which are passed to the plugin's <see cref="Plugin.NotifyLoaded"/> method.</param>
        /// <returns>The loaded plugin.</returns>
        public Plugin LoadPlugin(string fileName, params object[] args)
        {
            return this.LoadPluginCore(Assembly.LoadFrom(fileName), args);
        }

        /// <summary>
        /// Unloads all plugins.
        /// </summary>
        public void UnloadAllPlugins()
        {
            while (this.Plugins.Count > 0)
            {
                this.UnloadPluginCore(this.Plugins[0]);
            }
        }

        /// <summary>
        /// Unloads a plugin.
        /// </summary>
        /// <param name="name">The name of the plugin.</param>
        public void UnloadPlugin(string name)
        {
            this.UnloadPluginCore(this.GetPlugin(name));
        }

        /// <summary>
        /// Updates the manager.
        /// </summary>
        public virtual void Update()
        {
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the PluginManager.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.UnloadAllPlugins();
            this.Disposing = null;
        }

        /// <summary>
        /// Loads a plugin.
        /// </summary>
        /// <param name="assembly">The plugin <see cref="Assembly"/> to load.</param>
        /// <param name="args">Specifies parameters which are passed to the plugin's <see cref="Plugin.NotifyLoaded"/> method.</param>
        /// <returns>The loaded plugin.</returns>
        /// <exception cref="FileLoadException">Error while loading plugin.</exception>
        protected virtual Plugin LoadPluginCore(Assembly assembly, object[] args)
        {
            // check version
            object[] verAttrib = assembly.GetCustomAttributes(typeof(RequiredVersionAttribute), false);
            if (verAttrib.Length > 0)
            {
                var mva = (RequiredVersionAttribute)verAttrib[0];
                Version miyagiVersion = Assembly.GetExecutingAssembly().GetName().Version;

                int minV = mva.MinVersion.CompareTo(miyagiVersion);
                int maxV = mva.MaxVersion.CompareTo(miyagiVersion);

                if (minV < 0 && maxV > 0)
                {
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        // check if there's a type that derives from Plugin
                        if (type.IsSubclassOf(typeof(Plugin)))
                        {
                            // invoke .ctor(GUIManager)
                            var ci = type.GetConstructor(new[] { typeof(MiyagiSystem) });
                            var plugin = (Plugin)ci.Invoke(new object[] { this.MiyagiSystem });
                            plugin.NotifyLoaded(args);

                            this.Plugins.Add(plugin);
                            return plugin;
                        }
                    }
                }
                else
                {
                    if (minV >= 0)
                    {
                        throw new ArgumentException("Plugin requires a later version.", "assembly");
                    }

                    if (maxV <= 0)
                    {
                        throw new ArgumentException("Pluginrequires an earlier version.", "assembly");
                    }
                }
            }

            throw new ArgumentException("Invalid plugin assembly.", "assembly");
        }

        /// <summary>
        /// Unloads a plugin.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        protected virtual void UnloadPluginCore(Plugin plugin)
        {
            if (this.Plugins.Contains(plugin))
            {
                this.Plugins.Remove(plugin);

                plugin.NotifyUnloaded();
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}