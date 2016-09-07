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
    /// Represents an ordered pair of floating point x- and y-coordinates that defines a point in a two-dimensional plane.
    /// </summary>
    [TypeConverter(typeof(PointFConverter))]
    public struct PointF : IEquatable<PointF>
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty PointF.
        /// </summary>
        public static readonly PointF Empty = new PointF(0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointF"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public PointF(float x, float y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public double Length
        {
            get
            {
                return Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
            }
        }

        /// <summary>
        /// Gets the x-coordinate of this <see cref="PointF"/>.
        /// </summary>
        public float X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref="PointF"/>.
        /// </summary>
        public float Y
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
        public static bool operator !=(PointF left, PointF right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF operator *(PointF left, float right)
        {
            return new PointF(left.X * right, left.Y * right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF operator *(PointF left, PointF right)
        {
            return new PointF(left.X * right.X, left.Y * right.Y);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF operator *(float left, PointF right)
        {
            return right * left;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF operator +(PointF left, PointF right)
        {
            return new PointF(left.X + right.X, left.Y + right.Y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF operator -(PointF p)
        {
            return new PointF(-p.X, -p.Y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF operator -(PointF left, PointF right)
        {
            return new PointF(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF operator /(PointF left, float right)
        {
            return new PointF(left.X / right, left.Y / right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF operator /(PointF left, PointF right)
        {
            return new PointF(left.X / right.X, left.Y / right.Y);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PointF left, PointF right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF Add(PointF first, PointF second)
        {
            return first + second;
        }

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="x">The x-coordnate of the second point.</param>
        /// <param name="y">The y-coordnate of the second point.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF Add(PointF first, float x, float y)
        {
            return Add(first, new PointF(x, y));
        }

        /// <summary>
        /// Divides two points.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF Divide(PointF first, PointF second)
        {
            return first / second;
        }

        /// <summary>
        /// Converts an XElement representation to a <see cref="PointF"/> instance.
        /// </summary>
        /// <param name="xelement">The xelement.</param>
        /// <returns>The resulting <see cref="PointF"/>.</returns>
        public static PointF FromXElement(XElement xelement)
        {
            return new PointF(
                float.Parse(xelement.Element("X").Value, CultureInfo.InvariantCulture),
                float.Parse(xelement.Element("Y").Value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Multiplies two points.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF Multiply(PointF first, PointF second)
        {
            return first * second;
        }

        /// <summary>
        /// Converts a string representation to a <see cref="PointF"/> instance.
        /// </summary>
        /// <param name="s">A string representation of a <see cref="PointF"/> instance.</param>
        /// <returns>The <see cref="PointF"/> instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static PointF Parse(string s)
        {
            s = s.Replace("PointF(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2)
            {
                return new PointF(
                    float.Parse(values[0], CultureInfo.InvariantCulture),
                    float.Parse(values[1], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        /// <summary>
        /// Subtracts two points.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <returns>The result of the operator.</returns>
        public static PointF Subtract(PointF first, PointF second)
        {
            return first - second;
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Gets the angle between two points.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>A <see cref="System.Double"/> representing the angle in degrees.</returns>
        public double AngleBetween(PointF p)
        {
            float deltaX = p.X - this.X;
            float deltaY = -(p.Y - this.Y);

            double length = Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));

            double sin = deltaX / length;
            double cos = deltaY / length;

            double angle = Math.Round(Math.Asin(Math.Abs(sin)) * 180 / Math.PI, MidpointRounding.AwayFromZero);

            if (sin >= 0 && cos < 0)
            {
                angle = 180 - angle;
            }
            else if (sin < 0 && cos < 0)
            {
                angle = angle + 180;
            }
            else if (sin < 0 && cos >= 0)
            {
                angle = 360 - angle;
            }

            return angle;
        }

        /// <summary>
        /// Gets the distance between two points.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>A <see cref="System.Double"/> representing the distance.</returns>
        public double Distance(PointF p)
        {
            return p.Length - this.Length;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="other"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="other">Another object to compare to.</param>
        public bool Equals(PointF other)
        {
            return this.X == other.X && this.Y == other.Y;
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
            return obj is PointF && this.Equals((PointF)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        /// <summary>
        /// Determines whether this inside is inside the specified polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>
        /// <c>true</c> if this instance is in the specified polygon; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInsidePolygon(params PointF[] polygon)
        {
            // from http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
            bool retValue = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                PointF verti = polygon[i];
                PointF vertj = polygon[j];
                if (((verti.Y > this.Y) != (vertj.Y > this.Y))
                    && (this.X < (((vertj.X - verti.X) * (this.Y - verti.Y)) / (vertj.Y - verti.Y)) + verti.X))
                {
                    retValue = !retValue;
                }
            }

            return retValue;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "PointF({0},{1})", this.X, this.Y);
        }

        /// <summary>
        /// Converts a <see cref="PointF"/> to an XElement representation.
        /// </summary>
        /// <param name="name">The name of the XElement.</param>
        /// <returns>The XElement representation of this instance.</returns>
        public XElement ToXElement(string name)
        {
            return new XElement(
                name,
                new XElement("X", this.X.ToString(CultureInfo.InvariantCulture)),
                new XElement("Y", this.Y.ToString(CultureInfo.InvariantCulture)));
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class PointFConverter : MiyagiTypeConverter<PointF>
        {
            #region Methods

            #region Protected Methods

            protected override PointF ConvertFromCore(string s)
            {
                return PointF.Parse(s);
            }

            protected override string ConvertToCore(PointF value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1}", value.X, value.Y);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}