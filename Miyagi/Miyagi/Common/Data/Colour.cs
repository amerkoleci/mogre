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
    /// Represents an ARGB colour.
    /// </summary>
    [TypeConverter(typeof(ColourConverter))]
    public struct Colour : IEquatable<Colour>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> struct.
        /// </summary>
        /// <param name="alpha">The alpha component.</param>
        /// <param name="red">The red component.</param>
        /// <param name="green">The green component.</param>
        /// <param name="blue">The blue component.</param>
        public Colour(byte alpha, byte red, byte green, byte blue)
            : this()
        {
            this.Alpha = alpha;
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the alpha component value of this <see cref="Colour"/> structure.
        /// </summary>
        public byte Alpha
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the blue component value of this <see cref="Colour"/> structure.
        /// </summary>
        public byte Blue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the green component value of this <see cref="Colour"/> structure.
        /// </summary>
        public byte Green
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the red component value of this <see cref="Colour"/> structure.
        /// </summary>
        public byte Red
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
        public static bool operator !=(Colour left, Colour right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Colour left, Colour right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Creates a <see cref="Colour"/> structure from an ARGB value.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <param name="rgb">The RGB.</param>
        /// <returns>The <see cref="Colour"/> structure representing the ARGB value.</returns>
        public static Colour FromArgb(byte alpha, int rgb)
        {
            return Colour.FromArgb(rgb + (alpha << 24));
        }

        /// <summary>
        /// Creates a <see cref="Colour"/> structure from an ARGB value.
        /// </summary>
        /// <param name="argb">The ARGB.</param>
        /// <returns>The <see cref="Colour"/> structure representing the ARGB value.</returns>
        public static Colour FromArgb(long argb)
        {
            return new Colour(
                (byte)((argb >> 24) & 0xffL),
                (byte)((argb >> 16) & 0xffL),
                (byte)((argb >> 8) & 0xffL),
                (byte)(argb & 0xffL));
        }

        /// <summary>
        /// Creates a <see cref="Colour"/> structure from a RGB value.
        /// </summary>
        /// <param name="rgb">The RGB.</param>
        /// <returns>The <see cref="Colour"/> structure representing the RGB value.</returns>
        public static Colour FromRgb(int rgb)
        {
            return Colour.FromArgb(255, rgb);
        }

        /// <summary>
        /// Converts a string representation to a Colour instance.
        /// </summary>
        /// <param name="s">A string representation of a Colour instance.</param>
        /// <returns>The Colour instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static Colour Parse(string s)
        {
            s = s.Replace("Colour(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 4)
            {
                return new Colour(
                    byte.Parse(values[0], CultureInfo.InvariantCulture),
                    byte.Parse(values[1], CultureInfo.InvariantCulture),
                    byte.Parse(values[2], CultureInfo.InvariantCulture),
                    byte.Parse(values[3], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="Colour"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Colour"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Colour"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Colour other)
        {
            return this.Alpha == other.Alpha && this.Blue == other.Blue && this.Green == other.Green && this.Red == other.Red;
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
            return obj is Colour && this.Equals((Colour)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Alpha.GetHashCode() ^ this.Blue.GetHashCode() ^ this.Green.GetHashCode() ^ this.Red.GetHashCode();
        }

        /// <summary>
        /// Gets the 64-bit ARGB value of this <see cref="Colour"/> structure.
        /// </summary>
        /// <returns>A <see cref="System.Int64"/> representing the ARGB value of this <see cref="Colour"/> structure.</returns>
        public long ToArgb()
        {
            return (this.Alpha << 24) + (this.Red << 16) + (this.Green << 8) + this.Blue;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Colour({0},{1},{2},{3})", this.Alpha, this.Red, this.Green, this.Blue);
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class ColourConverter : MiyagiTypeConverter<Colour>
        {
            #region Methods

            #region Protected Methods

            protected override Colour ConvertFromCore(string s)
            {
                return Colour.Parse(s);
            }

            protected override string ConvertToCore(Colour value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", value.Alpha, value.Red, value.Green, value.Blue);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}