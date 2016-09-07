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

    using Miyagi.Common.Rendering;
    using Miyagi.Internals;

    /// <summary>
    /// Represents an ordered pair of integer x- and y-coordinates that defines a point in a two-dimensional plane.
    /// </summary>
    [TypeConverter(typeof(PointConverter))]
    public struct Point : IEquatable<Point>
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty Point.
        /// </summary>
        public static readonly Point Empty = new Point(0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Point(int x, int y)
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
        /// Gets the x-coordinate of this <see cref="Point"/>.
        /// </summary>
        public int X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref="Point"/>.
        /// </summary>
        public int Y
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="Miyagi.Common.Data.Point"/> to <see cref="Miyagi.Common.Data.PointF"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PointF(Point point)
        {
            return new PointF(point.X, point.Y);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator *(Point left, int right)
        {
            return new Point(left.X * right, left.Y * right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator *(Point left, Point right)
        {
            return new Point(left.X * right.X, left.Y * right.Y);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator *(int left, Point right)
        {
            return right * left;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator +(Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator -(Point p)
        {
            return new Point(-p.X, -p.Y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator -(Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator /(Point left, int right)
        {
            return new Point(left.X / right, left.Y / right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Point operator /(Point left, Point right)
        {
            return new Point(left.X / right.X, left.Y / right.Y);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The <see cref="Point"/> that is the result of the addition operation.</returns>
        public static Point Add(Point first, Point second)
        {
            return first + second;
        }

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The <see cref="Point"/> that is the result of the addition operation.</returns>
        public static Point Add(Point first, int x, int y)
        {
            return Add(first, new Point(x, y));
        }

        /// <summary>
        /// Divides two points.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The <see cref="Point"/> that is the result of the division operation.</returns>
        public static Point Divide(Point first, Point second)
        {
            return first / second;
        }

        /// <summary>
        /// Multiplies two points.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The <see cref="Point"/> that is the result of the multiplication operation.</returns>
        public static Point Multiply(Point first, Point second)
        {
            return first * second;
        }

        /// <summary>
        /// Converts a string representation to a Point instance.
        /// </summary>
        /// <param name="s">A string representation of a Point instance.</param>
        /// <returns>The Point instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static Point Parse(string s)
        {
            s = s.Replace("Point(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2)
            {
                return new Point(
                    int.Parse(values[0], CultureInfo.InvariantCulture),
                    int.Parse(values[1], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        /// <summary>
        /// Subtracts two points.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The <see cref="Point"/> that is the result of the subtraction operation.</returns>
        public static Point Subtract(Point first, Point second)
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
        public double AngleBetween(Point p)
        {
            int deltaX = p.X - this.X;
            int deltaY = -(p.Y - this.Y);

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
        public double Distance(Point p)
        {
            return (this - p).Length;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Point"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Point"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Point"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Point other)
        {
            return this.X == other.X && this.Y == other.Y;
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
            return obj is Point && this.Equals((Point)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Point({0},{1})", this.X, this.Y);
        }

        #endregion Public Methods

        #region Internal Methods

        internal Point MoveAlongAngle(double angle, double distance)
        {
            angle -= 90;
            angle = angle * Math.PI / 180f;
            return new Point((int)Math.Ceiling(this.X + (distance * Math.Cos(angle))), (int)Math.Ceiling(this.Y + (distance * Math.Sin(angle))));
        }

        internal PointF ToScreenCoordinates(Size screenResolution)
        {
            float x = this.X + RenderManager.HorizontalTexelOffset;
            float y = this.Y + RenderManager.VerticalTexelOffset;

            return new PointF(
                ((x / screenResolution.Width) * 2) - 1,
                -(((y / screenResolution.Height) * 2) - 1));
        }

        #endregion Internal Methods

        #endregion Methods

        #region Nested Types

        private sealed class PointConverter : MiyagiTypeConverter<Point>
        {
            #region Methods

            #region Protected Methods

            protected override Point ConvertFromCore(string s)
            {
                return Point.Parse(s);
            }

            protected override string ConvertToCore(Point value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1}", value.X, value.Y);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}