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
namespace Miyagi.Common.Data
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    using Miyagi.Internals;

    /// <summary>
    /// A structure describing the Size of an object as a pair of integers. 
    /// </summary>
    [TypeConverter(typeof(SizeConverter))]
    public struct Size : IEquatable<Size>
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty Size.
        /// </summary>
        public static readonly Size Empty = new Size(0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Size(int width, int height)
            : this()
        {
            this.Width = width;
            this.Height = height;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get
            {
                return this.Width == 0 && this.Height == 0;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="Miyagi.Common.Data.Size"/> to <see cref="Miyagi.Common.Data.SizeF"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SizeF(Size size)
        {
            return new SizeF(size.Width, size.Height);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Size left, Size right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Size operator +(Size left, Size right)
        {
            return new Size(left.Width + right.Width, left.Height + right.Height);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Size operator -(Size left, Size right)
        {
            return new Size(left.Width - right.Width, left.Height - right.Height);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Size operator -(Size left, Thickness right)
        {
            return new Size(left.Width - right.Horizontal, left.Height - right.Vertical);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Size operator -(Thickness left, Size right)
        {
            return new Size(right.Width - left.Horizontal, right.Height - left.Vertical);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Size left, Size right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Adds two sizes.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The result of the operator.</returns>
        public static Size Add(Size first, Size second)
        {
            return first + second;
        }

        /// <summary>
        /// Converts a string representation to a Size instance.
        /// </summary>
        /// <param name="s">A string representation of a Size instance.</param>
        /// <returns>The Size instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static Size Parse(string s)
        {
            s = s.Replace("Size(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2)
            {
                return new Size(
                    int.Parse(values[0], CultureInfo.InvariantCulture),
                    int.Parse(values[1], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="Size"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Size"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Size"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Size other)
        {
            return this.Width == other.Width && this.Height == other.Height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Size && this.Equals((Size)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Size({0},{1})", this.Width, this.Height);
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class SizeConverter : MiyagiTypeConverter<Size>
        {
            #region Methods

            #region Protected Methods

            protected override Size ConvertFromCore(string s)
            {
                return Size.Parse(s);
            }

            protected override string ConvertToCore(Size value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1}", value.Width, value.Height);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}