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
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// One or two-way property binding helper.
    /// </summary>
    public sealed class Binding : IDisposable
    {
        #region Fields

        private readonly INotifyPropertyChanged first;
        private readonly Func<object> firstGetter;
        private readonly string firstProperty;
        private readonly Action<object> firstSetter;
        private readonly INotifyPropertyChanged second;
        private readonly Func<object> secondGetter;
        private readonly string secondProperty;
        private readonly Action<object> secondSetter;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Binding class (two-way binding).
        /// </summary>
        /// <param name="first">The first object.</param>
        /// <param name="firstProperty">The name of the property of the first object.</param>
        /// <param name="second">The second object.</param>
        /// <param name="secondProperty">The name of the property of the second object.</param>
        public Binding(INotifyPropertyChanged first, string firstProperty, INotifyPropertyChanged second, string secondProperty)
            : this(first, firstProperty, (object)second, secondProperty)
        {
            this.second = second;
            this.second.PropertyChanged += this.PropertyChanged;
            this.secondProperty = secondProperty;
        }

        /// <summary>
        /// Initializes a new instance of the Binding class (one-way binding).
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="sourceProperty">The name of the property of the source object.</param>
        /// <param name="target">The target object.</param>
        /// <param name="targetProperty">The name of the property of the target object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="target" /> is <c>null</c>.</exception>
        public Binding(INotifyPropertyChanged source, string sourceProperty, object target, string targetProperty)
            : this(source, sourceProperty)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            CreateDelegates(target, targetProperty, out this.secondGetter, out this.secondSetter);
        }

        /// <summary>
        /// Initializes a new instance of the Binding class (one-way binding).
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="sourceProperty">The name of the property of the source object.</param>
        /// <param name="customSetter">The custom setter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="customSetter"/> is <c>null</c>.</exception>
        public Binding(INotifyPropertyChanged source, string sourceProperty, Action<object> customSetter)
            : this(source, sourceProperty)
        {
            if (customSetter == null)
            {
                throw new ArgumentNullException("customSetter");
            }

            this.secondSetter = customSetter;
        }

        private Binding(INotifyPropertyChanged first, string firstProperty)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            CreateDelegates(first, firstProperty, out this.firstGetter, out this.firstSetter);
            this.first = first;
            this.first.PropertyChanged += this.PropertyChanged;
            this.firstProperty = firstProperty;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the binding is currently paused.
        /// </summary>
        public bool Paused
        {
            get;
            set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Disposes the binding.
        /// </summary>
        public void Dispose()
        {
            if (this.first != null)
            {
                this.first.PropertyChanged -= this.PropertyChanged;
            }

            if (this.second != null)
            {
                this.second.PropertyChanged -= this.PropertyChanged;
            }

            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Private Static Methods

        private static void CreateDelegates(object obj, string propertyName, out Func<object> getter, out Action<object> setter)
        {
            PropertyInfo prop = obj.GetType().GetProperty(propertyName);

            getter = () => prop.GetValue(obj, null);
            setter = val => prop.SetValue(obj, Convert.ChangeType(val, prop.PropertyType), null);
        }

        #endregion Private Static Methods

        #region Private Methods

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.Paused)
            {
                return;
            }

            if (sender == this.first
                && e.PropertyName == this.firstProperty)
            {
                this.secondSetter(this.firstGetter());
            }
            else if (this.second != null
                     && sender == this.second
                     && e.PropertyName == this.secondProperty)
            {
                this.firstSetter(this.secondGetter());
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}