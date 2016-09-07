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
    using System.Xml.Linq;

    using Miyagi.Internals;

    /// <summary>
    /// Represents a point in  three-dimensional space.
    /// </summary>
    [TypeConverter(typeof(Vector3Converter))]
    public struct Vector3 : IEquatable<Vector3>
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty Vector3.
        /// </summary>
        public static readonly Vector3 Empty = new Vector3(0, 0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Vector3(float x, float y, float z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the x-component of the vector.
        /// </summary>
        public float X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the y-component of the vector.
        /// </summary>
        public float Y
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the z-component of the vector.
        /// </summary>
        public float Z
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
        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator *(Vector3 left, float right)
        {
            return new Vector3(left.X * right, left.Y * right, left.Z * right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator *(float left, Vector3 right)
        {
            return right * left;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator -(Vector3 p)
        {
            return new Vector3(-p.X, -p.Y, -p.Z);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator /(Vector3 left, float right)
        {
            return new Vector3(left.X / right, left.Y / right, left.Z / right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 Add(Vector3 first, Vector3 second)
        {
            return first + second;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 Divide(Vector3 first, Vector3 second)
        {
            return first / second;
        }

        /// <summary>
        /// Converts an XElement representation to a <see cref="Vector3"/> instance.
        /// </summary>
        /// <param name="xelement">The xelement.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 FromXElement(XElement xelement)
        {
            return new Vector3(
                float.Parse(xelement.Element("X").Value, CultureInfo.InvariantCulture),
                float.Parse(xelement.Element("Y").Value, CultureInfo.InvariantCulture),
                float.Parse(xelement.Element("Z").Value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 Multiply(Vector3 first, Vector3 second)
        {
            return first * second;
        }

        /// <summary>
        /// Converts a string representation to a PointF instance.
        /// </summary>
        /// <param name="s">A string representation of a PointF instance.</param>
        /// <returns>The PointF instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static Vector3 Parse(string s)
        {
            s = s.Replace("Vector3(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 3)
            {
                return new Vector3(
                    float.Parse(values[0], CultureInfo.InvariantCulture),
                    float.Parse(values[1], CultureInfo.InvariantCulture),
                    float.Parse(values[2], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 Subtract(Vector3 first, Vector3 second)
        {
            return first - second;
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="Vector3"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Vector3"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Vector3"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector3 other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to.</param>
        public override bool Equals(object obj)
        {
            return obj is Vector3 && this.Equals((Vector3)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Vector3({0},{1},{2})", this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Converts a <see cref="Vector3"/> to an XElement representation.
        /// </summary>
        /// <param name="name">The name of the XElement.</param>
        /// <returns>The XElement representation of this instance.</returns>
        public XElement ToXElement(string name)
        {
            return new XElement(
                name,
                new XElement("X", this.X.ToString(CultureInfo.InvariantCulture)),
                new XElement("Y", this.Y.ToString(CultureInfo.InvariantCulture)),
                new XElement("Z", this.Z.ToString(CultureInfo.InvariantCulture)));
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class Vector3Converter : MiyagiTypeConverter<Vector3>
        {
            #region Methods

            #region Protected Methods

            protected override Vector3 ConvertFromCore(string s)
            {
                return Vector3.Parse(s);
            }

            protected override string ConvertToCore(Vector3 value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", value.X, value.Y, value.Z);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}