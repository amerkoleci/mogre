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
    /// Represents a range of integer values.
    /// </summary>
    [TypeConverter(typeof(RangeConverter))]
    public struct Range
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> struct.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        public Range(int first, int last)
            : this()
        {
            this.First = first;
            this.Last = last;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the first value.
        /// </summary>
        public int First
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last value.
        /// </summary>
        public int Last
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
        public static bool operator !=(Range left, Range right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Range left, Range right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Converts a string representation to a Range instance.
        /// </summary>
        /// <param name="s">A string representation of a Range instance.</param>
        /// <returns>The Range instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static Range Parse(string s)
        {
            s = s.Replace("Range(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2)
            {
                return new Range(
                    int.Parse(values[0], CultureInfo.InvariantCulture),
                    int.Parse(values[1], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="Range"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Range"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Range"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Range other)
        {
            return this.First == other.First && this.Last == other.Last;
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
            return obj is Range && this.Equals((Range)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.First.GetHashCode() ^ this.Last.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Range({0},{1})", this.First, this.Last);
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class RangeConverter : MiyagiTypeConverter<Range>
        {
            #region Methods

            #region Protected Methods

            protected override Range ConvertFromCore(string s)
            {
                return Range.Parse(s);
            }

            protected override string ConvertToCore(Range value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1}", value.First, value.Last);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}