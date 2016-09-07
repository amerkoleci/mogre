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
namespace Miyagi.TwoD
{
    using System;
    using System.Linq;

    using Miyagi.Common;
    using Miyagi.Common.Events;
    using Miyagi.Common.Serialization;
    using Miyagi.TwoD.Layers;
    using Miyagi.TwoD.Painting;

    /// <summary>
    /// The TwoDManager.
    /// </summary>
    public class TwoDManager : IManager
    {
        #region Fields

        private Painter painter;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TwoDManager class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        public TwoDManager(MiyagiSystem system)
        {
            this.Layers = new MiyagiCollection<Layer>();
            this.Layers.ItemAdded += this.LayerAdded;
            this.Layers.ItemInserted += this.LayerAdded;
            this.Layers.ItemRemoved += LayerRemoved;
            this.MiyagiSystem = system;
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
        /// Gets the list of layers.
        /// </summary>
        public MiyagiCollection<Layer> Layers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the MiyagiSystem.
        /// </summary>
        public MiyagiSystem MiyagiSystem
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the painter.
        /// </summary>
        /// <value>The painter.</value>
        public Painter Painter
        {
            get
            {
                return this.painter ?? (this.painter = new Painter(this));
            }
        }

        /// <summary>
        /// Gets the type of the manager.
        /// </summary>
        public string Type
        {
            get
            {
                return "TwoD";
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        void IManager.LoadSerializationData(SerializationData data)
        {
            this.Layers = (MiyagiCollection<Layer>)data["Layers"];
            foreach (var layer in this.Layers)
            {
                this.AddLayer(layer);
            }
        }

        void IManager.NotifyManagerRegistered(IManager manager)
        {
        }

        void IManager.SaveSerializationData(SerializationData data)
        {
            data.Add("Layers", this.Layers);
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Disposes the TwoDManager.
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
        /// Gets an element of the specified type by name.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <returns>The first element of that name and type if it exists; otherwise, null.</returns>
        /// <typeparam name="T">The type of the element.</typeparam>
        public virtual T GetElement<T>(string name)
            where T : LayerElement
        {
            return this.Layers.Select(layer => layer.GetElement<T>(name)).Where(element => element != null).FirstOrDefault();
        }

        /// <summary>
        /// Gets an element by name.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <returns>The first element of that name if it exists; otherwise, null.</returns>
        public virtual LayerElement GetElement(string name)
        {
            return this.Layers.Select(layer => layer.GetElement(name)).FirstOrDefault(element => element != null);
        }

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Updates the TwoDManager.
        /// </summary>
        public virtual void Update()
        {
            foreach (Layer l in this.Layers)
            {
                l.Update();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the TwoDManager.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            foreach (var l in this.Layers)
            {
                l.Dispose();
            }

            this.Layers.Clear();

            if (this.painter != null)
            {
                this.painter.Dispose();
            }

            this.Disposing = null;
        }

        #endregion Protected Methods

        #region Private Static Methods

        private static void LayerRemoved(object sender, CollectionEventArgs<Layer> e)
        {
            e.Item.DestroyRenderer();
            e.Item.TwoDManager = null;
        }

        #endregion Private Static Methods

        #region Private Methods

        private void AddLayer(Layer layer)
        {
            layer.TwoDManager = this;
        }

        private void LayerAdded(object sender, CollectionEventArgs<Layer> e)
        {
            this.AddLayer(e.Item);
        }

        #endregion Private Methods

        #endregion Methods
    }
}