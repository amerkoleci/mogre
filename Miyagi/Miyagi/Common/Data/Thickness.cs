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
    /// Describes the thickness of a rectangular object.
    /// </summary>
    [TypeConverter(typeof(ThicknessConverter))]
    public struct Thickness : IEquatable<Thickness>
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty Thickness.
        /// </summary>
        public static readonly Thickness Empty = new Thickness(0, 0, 0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Thickness struct.
        /// </summary>
        /// <param name="all">The thickness of all sides.</param>
        public Thickness(int all)
            : this()
        {
            this.Bottom = this.Left = this.Right = this.Top = all;
        }

        /// <summary>
        /// Initializes a new instance of the Thickness struct.
        /// </summary>
        /// <param name="left">The size of the left side.</param>
        /// <param name="top">The size of the top side.</param>
        /// <param name="right">The size of the right side.</param>
        /// <param name="bottom">The size of the bottom side.</param>
        public Thickness(int left, int top, int right, int bottom)
            : this()
        {
            this.Bottom = bottom;
            this.Left = left;
            this.Right = right;
            this.Top = top;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the width of the bottom side.
        /// </summary>
        /// <value>The width of the bottom side in pixels.</value>
        public int Bottom
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total width of the Thickness.
        /// </summary>
        /// <value>The width of the Thickness.</value>
        public int Horizontal
        {
            get
            {
                return this.Left + this.Right;
            }
        }

        /// <summary>
        /// Gets the width of the Left side.
        /// </summary>
        /// <value>The width of the Left side in pixels.</value>
        public int Left
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the width of the right side.
        /// </summary>
        /// <value>The width of the right side in pixels.</value>
        public int Right
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the width of the top side.
        /// </summary>
        /// <value>The width of the top side in pixels.</value>
        public int Top
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total height of the Thickness.
        /// </summary>
        /// <value>The height of the Thickness.</value>
        public int Vertical
        {
            get
            {
                return this.Top + this.Bottom;
            }
        }

        #endregion Public Properties

        #region Private Properties

        private bool All
        {
            get
            {
                return this.Bottom == this.Left && this.Bottom == this.Right && this.Bottom == this.Top;
            }
        }

        #endregion Private Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Returns a value indicating whether two Thickness instances are not equal.
        /// </summary>
        /// <param name="left">A Thickness instance.</param>
        /// <param name="right">Another Thickness instance.</param>
        /// <returns><c>true</c> if both are unequal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Thickness left, Thickness right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Thickness operator +(Thickness left, Thickness right)
        {
            return new Thickness(left.Left + right.Left, left.Top + right.Top, left.Right + right.Right, left.Bottom + right.Bottom);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Thickness operator -(Thickness left, Thickness right)
        {
            return new Thickness(left.Left - right.Left, left.Top - right.Top, left.Right - right.Right, left.Bottom - right.Bottom);
        }

        /// <summary>
        /// Returns a value indicating whether two Thickness instances are equal.
        /// </summary>
        /// <param name="left">A Thickness instance.</param>
        /// <param name="right">Another Thickness instance.</param>
        /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Thickness left, Thickness right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Converts a string representation to a Thickness instance.
        /// </summary>
        /// <param name="s">A string representation of a Thickness instance.</param>
        /// <returns>The Thickness instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static Thickness Parse(string s)
        {
            s = s.Replace("Thickness(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 1)
            {
                return new Thickness(int.Parse(values[0], CultureInfo.InvariantCulture));
            }

            if (values.Length == 4)
            {
                return new Thickness(
                    int.Parse(values[0], CultureInfo.InvariantCulture),
                    int.Parse(values[1], CultureInfo.InvariantCulture),
                    int.Parse(values[2], CultureInfo.InvariantCulture),
                    int.Parse(values[3], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified Thickness.
        /// </summary>
        /// <param name="other">The other Thickness.</param>
        /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
        public bool Equals(Thickness other)
        {
            // add comparisions for all members here
            return this.Top == other.Top && this.Bottom == other.Bottom && this.Left == other.Left && this.Right == other.Right;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified Object.
        /// </summary>
        /// <param name="obj">The other Object.</param>
        /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Thickness && this.Equals((Thickness)obj);
        }

        /// <summary>
        /// Returns the HashCode of the current instance.
        /// </summary>
        /// <returns>The HashCode of the Thickness.</returns>
        public override int GetHashCode()
        {
            // combine the hash codes of all members here (e.g. with XOR operator ^)
            return this.Top.GetHashCode() ^ this.Bottom.GetHashCode() ^ this.Left.GetHashCode() ^ this.Right.GetHashCode();
        }

        /// <summary>
        /// Converts a Thickness instance to a string representation.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString()
        {
            return this.All
                       ? string.Format(CultureInfo.InvariantCulture, "Thickness({0})", this.Bottom)
                       : string.Format(CultureInfo.InvariantCulture, "Thickness({0},{1},{2},{3})", this.Left, this.Top, this.Right, this.Bottom);
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class ThicknessConverter : MiyagiTypeConverter<Thickness>
        {
            #region Methods

            #region Protected Methods

            protected override Thickness ConvertFromCore(string s)
            {
                return Thickness.Parse(s);
            }

            protected override string ConvertToCore(Thickness value)
            {
                return value.All
                           ? string.Format(CultureInfo.InvariantCulture, "{0}", value.Bottom)
                           : string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", value.Left, value.Top, value.Right, value.Bottom);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}