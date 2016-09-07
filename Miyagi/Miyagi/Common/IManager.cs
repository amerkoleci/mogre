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

    using Miyagi.Common.Serialization;

    /// <summary>
    /// The interface for managers.
    /// </summary>
    public interface IManager : IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when the manager is disposing.
        /// </summary>
        event EventHandler Disposing;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the manager has been disposed.
        /// </summary>
        bool IsDisposed
        {
            get;
        }

        /// <summary>
        /// Gets the type of the manager.
        /// </summary>
        string Type
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Loads the serialization data.
        /// </summary>
        /// <param name="data">The data.</param>
        void LoadSerializationData(SerializationData data);

        /// <summary>
        /// Notifies a manager on registration of another manager.
        /// </summary>
        /// <param name="manager">The newly registered manager.</param>
        void NotifyManagerRegistered(IManager manager);

        /// <summary>
        /// Saves the serialization data.
        /// </summary>
        /// <param name="data">The data.</param>
        void SaveSerializationData(SerializationData data);

        /// <summary>
        /// Updates the manager.
        /// </summary>
        void Update();

        #endregion Methods
    }
}