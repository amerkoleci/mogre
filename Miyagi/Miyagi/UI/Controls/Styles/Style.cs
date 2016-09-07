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
namespace Miyagi.UI.Controls.Styles
{
    using System.ComponentModel;

    using Miyagi.Common.Serialization;
    using Miyagi.Internals;

    /// <summary>
    /// The base class for styles.
    /// </summary>
    [SerializableType]
    public class Style : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Events

        /// <summary>
        /// Occurs after a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs before a property changes.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion Events

        #region Properties

        #region Internal Properties

        internal ResizeHelper ResizeHelper
        {
            get;
            private set;
        }

        #endregion Internal Properties

        #endregion Properties

        #region Methods

        #region Internal Methods

        internal void Resize(double widthFactor, double heightFactor)
        {
            if (this.ResizeHelper == null)
            {
                this.ResizeHelper = new ResizeHelper(this.DoResize);
            }

            this.ResizeHelper.Resize(widthFactor, heightFactor);
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <summary>
        /// Resizes the style.
        /// </summary>
        /// <param name="widthFactor">The relative growth of the width.</param>
        /// <param name="heightFactor">The relative growth of the height.</param>
        protected virtual void DoResize(double widthFactor, double heightFactor)
        {
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="propertyName">The name of the changing property.</param>
        protected void OnPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}