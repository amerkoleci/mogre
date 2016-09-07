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
namespace Miyagi.Common.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A SerializationManager.
    /// </summary>
    public class SerializationManager : IManager
    {
        #region Fields

        private readonly List<Serializer> serializers;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SerializationManager class.
        /// </summary>
        /// <param name="system">The MiyagiSystem.</param>
        public SerializationManager(MiyagiSystem system)
        {
            this.MiyagiSystem = system;
            this.serializers = new List<Serializer>();
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
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        public Encoding Encoding
        {
            get;
            set;
        }

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
        /// Gets the MiyagiSystem.
        /// </summary>
        public MiyagiSystem MiyagiSystem
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the collection of serializers.
        /// </summary>
        public ReadOnlyCollection<Serializer> Serializers
        {
            get
            {
                return new ReadOnlyCollection<Serializer>(this.serializers);
            }
        }

        /// <summary>
        /// Gets the type of the manager.
        /// </summary>
        public string Type
        {
            get
            {
                return "Serialization";
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Explicit Interface Methods

        void IManager.LoadSerializationData(SerializationData data)
        {
        }

        void IManager.NotifyManagerRegistered(IManager manager)
        {
        }

        void IManager.SaveSerializationData(SerializationData info)
        {
        }

        #endregion Explicit Interface Methods

        #region Public Methods

        /// <summary>
        /// Disposed the SerializionManager.
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
        /// Exports the MiyagiSystem to file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        public void ExportToFile(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                this.ExportToStream(fileStream, Path.GetExtension(path));
            }
        }

        /// <summary>
        /// Exports the MiyagiSystem to a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="extension">The extension.</param>
        public virtual void ExportToStream(Stream stream, string extension)
        {
            Serializer serializer = this.GetSerializer(extension);
            if (serializer != null)
            {
                serializer.ExportToStream(stream, this.MiyagiSystem.GetSerializationData());
            }
            else
            {
                throw new ArgumentException("No serializer found for file extension", extension);
            }
        }

        /// <summary>
        /// Gets a serializer.
        /// </summary>
        /// <param name="fileExtension">The file extension the serializer has to support.</param>
        /// <returns>A Serializer representing the serializer.</returns>
        public virtual Serializer GetSerializer(string fileExtension)
        {
            fileExtension = fileExtension.ToLowerInvariant();
            return this.serializers.FirstOrDefault(serializer => serializer.FileExtensions.Contains(fileExtension));
        }

        /// <summary>
        /// Imports a MiyagiSystem from file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        public void ImportFromFile(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                this.ImportFromStream(fileStream, Path.GetExtension(path));
            }
        }

        /// <summary>
        /// Imports a MiyagiSystem from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="extension">The extension.</param>
        public virtual void ImportFromStream(Stream stream, string extension)
        {
            Serializer serializer = this.GetSerializer(extension);
            if (serializer != null)
            {
                serializer.ImportFromStream(stream);
            }
            else
            {
                throw new ArgumentException("No serializer found for file extension", extension);
            }
        }

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        public virtual void Initialize()
        {
            this.RegisterSerializer(new XmlSerializer(this));
        }

        /// <summary>
        /// Registers a serializer.
        /// </summary>
        /// <param name="serializer">The new serializer.</param>
        public virtual void RegisterSerializer(Serializer serializer)
        {
            if (!this.serializers.Contains(serializer))
            {
                this.serializers.Add(serializer);
            }
        }

        /// <summary>
        /// Unregisters a serializer.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        public virtual void UnregisterSerializer(Serializer serializer)
        {
            this.serializers.Remove(serializer);
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
        /// Disposes the SerializationManager.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.serializers.Clear();
            this.Disposing = null;
        }

        #endregion Protected Methods

        #endregion Methods
    }
}