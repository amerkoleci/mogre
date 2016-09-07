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
namespace Miyagi.Common.Scripting
{
    using System;

    using Miyagi.Common.Serialization;

    /// <summary>
    /// The ScriptingManager.
    /// </summary>
    public class ScriptingManager : IManager
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScriptingManager class.
        /// </summary>
        /// <param name="miyagiSystem">The MiyagiSystem.</param>
        protected internal ScriptingManager(MiyagiSystem miyagiSystem)
        {
            this.MiyagiSystem = miyagiSystem;
            this.ScriptingSchemeList = new MiyagiCollection<ScriptingScheme>();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the binding of events is requested.
        /// </summary>
        public event EventHandler<BindEventsEventArgs> BindEventsRequested;

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
        /// Gets the collection of ScriptingScheme.
        /// </summary>
        /// <value>The collection of ScriptingScheme.</value>
        public MiyagiCollection<ScriptingScheme> ScriptingSchemes
        {
            get
            {
                return this.ScriptingSchemeList;
            }
        }

        /// <summary>
        /// Gets the type of the manager.
        /// </summary>
        public string Type
        {
            get
            {
                return "Scripting";
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal MiyagiCollection<ScriptingScheme> ScriptingSchemeList
        {
            get;
            private set;
        }

        #endregion Internal Properties

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
        /// Binds all events according to the ScriptingSchemes.
        /// </summary>
        public virtual void BindAllUnboundSchemes()
        {
            foreach (ScriptingScheme scheme in this.ScriptingSchemeList)
            {
                if (!scheme.IsBound)
                {
                    if (this.BindEventsRequested != null)
                    {
                        this.BindEventsRequested(this, new BindEventsEventArgs(scheme));
                    }

                    scheme.IsBound = true;
                }
            }
        }

        /// <summary>
        /// Disposes the scripting manager.
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
        /// Initializes the manager.
        /// </summary>
        public virtual void Initialize()
        {
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
        /// Disposes the ScriptingManager.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.BindEventsRequested = null;
            this.Disposing = null;
        }

        #endregion Protected Methods

        #endregion Methods
    }
}