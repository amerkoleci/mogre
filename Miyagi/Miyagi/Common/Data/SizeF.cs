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
    /// A structure describing the Size of an object as a pair of floats. 
    /// </summary>
    [TypeConverter(typeof(SizeFConverter))]
    public struct SizeF : IEquatable<SizeF>
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty SizeF.
        /// </summary>
        public static readonly SizeF Empty = new SizeF(0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeF"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SizeF(float width, float height)
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
        public float Height
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
        public float Width
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SizeF left, SizeF right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static SizeF operator +(SizeF left, SizeF right)
        {
            return new SizeF(left.Width + right.Width, left.Height + right.Height);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static SizeF operator -(SizeF left, SizeF right)
        {
            return new SizeF(left.Width - right.Width, left.Height - right.Height);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SizeF left, SizeF right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Adds two sizes.
        /// </summary>
        /// <param name="first">The first size.</param>
        /// <param name="second">The second size.</param>
        /// <returns>The result of the operator.</returns>
        public static SizeF Add(SizeF first, SizeF second)
        {
            return first + second;
        }

        /// <summary>
        /// Converts a string representation to a SizeF instance.
        /// </summary>
        /// <param name="s">A string representation of a SizeF instance.</param>
        /// <returns>The SizeF instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static SizeF Parse(string s)
        {
            s = s.Replace("SizeF(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2)
            {
                return new SizeF(
                    float.Parse(values[0], CultureInfo.InvariantCulture),
                    float.Parse(values[1], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="SizeF"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SizeF"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SizeF"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(SizeF other)
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
            return obj is SizeF && this.Equals((SizeF)obj);
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
            return string.Format(CultureInfo.InvariantCulture, "SizeF({0},{1})", this.Width, this.Height);
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class SizeFConverter : MiyagiTypeConverter<SizeF>
        {
            #region Methods

            #region Protected Methods

            protected override SizeF ConvertFromCore(string s)
            {
                return SizeF.Parse(s);
            }

            protected override string ConvertToCore(SizeF value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1}", value.Width, value.Height);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}