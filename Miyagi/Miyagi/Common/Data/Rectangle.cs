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
    /// Represents the location and size of a rectangle as integer values.
    /// </summary>
    [TypeConverter(typeof(RectangleConverter))]
    public struct Rectangle : IEquatable<Rectangle>
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty Rectangle.
        /// </summary>
        public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        public Rectangle(Point location, Size size)
            : this(location.X, location.Y, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="size">The size.</param>
        public Rectangle(int x, int y, Size size)
            : this(x, y, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(Point location, int width, int height)
            : this(location.X, location.Y, width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(int x, int y, int width, int height)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the y-coordinate of the bottom edge.
        /// </summary>
        public int Bottom
        {
            get
            {
                return this.Y + this.Height;
            }
        }

        /// <summary>
        /// Gets the center.
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point(
                    this.Location.X + (this.Width / 2),
                    this.Location.Y + (this.Height / 2));
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the x-coordinate of the left edge.
        /// </summary>
        public int Left
        {
            get
            {
                return this.X;
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point(this.X, this.Y);
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the right edge.
        /// </summary>
        public int Right
        {
            get
            {
                return this.X + this.Width;
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(this.Width, this.Height);
            }
        }

        /// <summary>
        /// Gets the y-coordinate of the top edge.
        /// </summary>
        public int Top
        {
            get
            {
                return this.Y;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the X-coordinate.
        /// </summary>
        public int X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Y-coordinate.
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
        /// Performs an implicit conversion from <see cref="Miyagi.Common.Data.Rectangle"/> to <see cref="Miyagi.Common.Data.RectangleF"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RectangleF(Rectangle rectangle)
        {
            return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Creates a rectangle from edge locations.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <returns>The newly created Rectangle.</returns>
        public static Rectangle FromLTRB(int left, int top, int right, int bottom)
        {
            return new Rectangle(
                left,
                top,
                right - left,
                bottom - top);
        }

        /// <summary>
        /// Intersects two <see cref="Rectangle"/> instances.
        /// </summary>
        /// <param name="a">The first rectangle.</param>
        /// <param name="b">The second rectangle.</param>
        /// <returns>The intersected rectangle.</returns>
        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            int x = Math.Max(a.X, b.X);
            int num2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int y = Math.Max(a.Y, b.Y);
            int num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
            if ((num2 >= x) && (num4 >= y))
            {
                return new Rectangle(x, y, num2 - x, num4 - y);
            }

            return Empty;
        }

        /// <summary>
        /// Converts a string representation to a Rectangle instance.
        /// </summary>
        /// <param name="s">A string representation of a Rectangle instance.</param>
        /// <returns>The Rectangle instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static Rectangle Parse(string s)
        {
            s = s.Replace("Rectangle(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 4)
            {
                return new Rectangle(
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
        /// Determines whether this <see cref="Rectangle"/> instance contains the specified <see cref="Rectangle"/> instance.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to test.</param>
        /// <returns>
        /// <c>true</c> if this instance contains the specified <see cref="Rectangle"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Rectangle rect)
        {
            return this.X <= rect.X
                   && (rect.X + rect.Width) <= (this.X + this.Width)
                   && this.Y <= rect.Y
                   && (rect.Y + rect.Height) <= (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether this <see cref="Rectangle"/> instance contains the specified <see cref="Point"/> instance.
        /// </summary>
        /// <param name="p">The <see cref="Point"/> to test.</param>
        /// <returns>
        /// <c>true</c> if this instance contains the specified <see cref="Point"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Point p)
        {
            return this.Contains(p.X, p.Y);
        }

        /// <summary>
        /// Determines whether this <see cref="Rectangle"/> instance contains the specified coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate to test.</param>
        /// <param name="y">The y-coordinate to test..</param>
        /// <returns>
        /// <c>true</c> if this instance contains the specified coordinate; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(int x, int y)
        {
            return this.X <= x && x < (this.X + this.Width) && this.Y <= y && y < (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Rectangle"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Rectangle"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Rectangle"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Rectangle other)
        {
            return this.Size == other.Size && this.Location == other.Location;
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
            return obj is Rectangle && this.Equals((Rectangle)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Size.GetHashCode() ^ this.Location.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Rectangle({0},{1},{2},{3})", this.X, this.Y, this.Width, this.Height);
        }

        #endregion Public Methods

        #region Internal Methods

        internal RectangleF ToScreenCoordinates(Size screenResolution)
        {
            return ((RectangleF)this).ToScreenCoordinates(screenResolution);
        }

        #endregion Internal Methods

        #endregion Methods

        #region Nested Types

        private sealed class RectangleConverter : MiyagiTypeConverter<Rectangle>
        {
            #region Methods

            #region Protected Methods

            protected override Rectangle ConvertFromCore(string s)
            {
                return Rectangle.Parse(s);
            }

            protected override string ConvertToCore(Rectangle value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", value.X, value.Y, value.Width, value.Height);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}