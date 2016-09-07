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

    using Miyagi.Common.Rendering;
    using Miyagi.Internals;

    /// <summary>
    /// Represents the location and size of a rectangle as floating point values.
    /// </summary>
    [TypeConverter(typeof(RectangleFConverter))]
    public struct RectangleF : IEquatable<RectangleF>
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty RectangleF.
        /// </summary>
        public static readonly RectangleF Empty = new RectangleF(0, 0, 0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        public RectangleF(PointF location, SizeF size)
            : this(location.X, location.Y, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectangleF(PointF location, float width, float height)
            : this(location.X, location.Y, width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectangleF(float x, float y, float width, float height)
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
        public float Bottom
        {
            get
            {
                return this.Y + this.Height;
            }
        }

        /// <summary>
        /// Gets the center.
        /// </summary>
        public PointF Center
        {
            get
            {
                return new PointF(
                    this.Location.X + (this.Width / 2),
                    this.Location.Y + (this.Height / 2));
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public float Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the x-coordinate of the left edge.
        /// </summary>
        public float Left
        {
            get
            {
                return this.X;
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        public PointF Location
        {
            get
            {
                return new PointF(this.X, this.Y);
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the right edge.
        /// </summary>
        public float Right
        {
            get
            {
                return this.X + this.Width;
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public SizeF Size
        {
            get
            {
                return new SizeF(this.Width, this.Height);
            }
        }

        /// <summary>
        /// Gets the y-coordinate of the top edge.
        /// </summary>
        public float Top
        {
            get
            {
                return this.Y;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public float Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the X-coordinate.
        /// </summary>
        public float X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Y-coordinate.
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
        public static bool operator !=(RectangleF left, RectangleF right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RectangleF left, RectangleF right)
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
        public static RectangleF FromLTRB(float left, float top, float right, float bottom)
        {
            return new RectangleF(
                left,
                top,
                right - left,
                bottom - top);
        }

        /// <summary>
        /// Converts an XElement representation to a <see cref="RectangleF"/> instance.
        /// </summary>
        /// <param name="xelement">The xelement.</param>
        /// <returns>The resulting <see cref="RectangleF"/>.</returns>
        public static RectangleF FromXElement(XElement xelement)
        {
            var ci = CultureInfo.InvariantCulture;
            return xelement.Element("Left") != null
                       ? RectangleF.FromLTRB(
                           float.Parse(xelement.Element("Left").Value, ci),
                           float.Parse(xelement.Element("Top").Value, ci),
                           float.Parse(xelement.Element("Right").Value, ci),
                           float.Parse(xelement.Element("Bottom").Value, ci))
                       : new RectangleF(
                             float.Parse(xelement.Element("X").Value, ci),
                             float.Parse(xelement.Element("Y").Value, ci),
                             float.Parse(xelement.Element("Width").Value, ci),
                             float.Parse(xelement.Element("Height").Value, ci));
        }

        /// <summary>
        /// Intersects two <see cref="RectangleF"/> instances.
        /// </summary>
        /// <param name="a">The first rectangle.</param>
        /// <param name="b">The second rectangle.</param>
        /// <returns>The intersected rectangle.</returns>
        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            float x = Math.Max(a.X, b.X);
            float num2 = Math.Min(a.X + a.Width, b.X + b.Width);
            float y = Math.Max(a.Y, b.Y);
            float num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
            if ((num2 >= x) && (num4 >= y))
            {
                return new RectangleF(x, y, num2 - x, num4 - y);
            }

            return Empty;
        }

        /// <summary>
        /// Converts a string representation to a <see cref="RectangleF"/> instance.
        /// </summary>
        /// <param name="s">A string representation of a <see cref="RectangleF"/> instance.</param>
        /// <returns>The <see cref="RectangleF"/> instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static RectangleF Parse(string s)
        {
            s = s.Replace("RectangleF(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 4)
            {
                return new RectangleF(
                    float.Parse(values[0], CultureInfo.InvariantCulture),
                    float.Parse(values[1], CultureInfo.InvariantCulture),
                    float.Parse(values[2], CultureInfo.InvariantCulture),
                    float.Parse(values[3], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        /// <summary>
        /// Converts a string representation to a <see cref="RectangleF"/> instance.
        /// </summary>
        /// <param name="s">A string representation of a <see cref="RectangleF"/> instance.</param>
        /// <param name="rectangleF">A <see cref="RectangleF"/> instance representing the conversion result.</param>
        /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string s, out RectangleF rectangleF)
        {
            rectangleF = new RectangleF();
            s = s.Replace("RectangleF(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 4)
            {
                return false;
            }

            float x, y, width, height;
            if (!float.TryParse(values[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x))
            {
                return false;
            }

            if (!float.TryParse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y))
            {
                return false;
            }

            if (!float.TryParse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture, out width))
            {
                return false;
            }

            if (!float.TryParse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture, out height))
            {
                return false;
            }

            rectangleF = new RectangleF(x, y, width, height);
            return true;
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Determines whether this <see cref="RectangleF"/> instance contains the specified <see cref="RectangleF"/> instance.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to test.</param>
        /// <returns>
        /// <c>true</c> if this instance contains the specified <see cref="RectangleF"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(RectangleF rect)
        {
            return this.X <= rect.X
                   && (rect.X + rect.Width) <= (this.X + this.Width)
                   && this.Y <= rect.Y
                   && (rect.Y + rect.Height) <= (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether this <see cref="RectangleF"/> instance contains the specified <see cref="PointF"/> instance.
        /// </summary>
        /// <param name="p">The <see cref="PointF"/> to test.</param>
        /// <returns>
        /// <c>true</c> if this instance contains the specified <see cref="PointF"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(PointF p)
        {
            return this.Contains(p.X, p.Y);
        }

        /// <summary>
        /// Determines whether this <see cref="RectangleF"/> instance contains the specified coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate to test.</param>
        /// <param name="y">The y-coordinate to test..</param>
        /// <returns>
        /// <c>true</c> if this instance contains the specified coordinate; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(float x, float y)
        {
            return this.X <= x && x < (this.X + this.Width) && this.Y <= y && y < (this.Y + this.Height);
        }

        /// <summary>
        /// Determines whether the specified <see cref="RectangleF"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="RectangleF"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="RectangleF"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(RectangleF other)
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
            return obj is RectangleF && this.Equals((RectangleF)obj);
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
            return string.Format(CultureInfo.InvariantCulture, "RectangleF({0},{1},{2},{3})", this.X, this.Y, this.Width, this.Height);
        }

        /// <summary>
        /// Converts a <see cref="RectangleF"/> to an XElement representation.
        /// </summary>
        /// <param name="name">The name of the XElement.</param>
        /// <returns>The XElement representation of this instance.</returns>
        public XElement ToXElement(string name)
        {
            return new XElement(
                name,
                new XElement("X", this.X.ToString(CultureInfo.InvariantCulture)),
                new XElement("Y", this.Y.ToString(CultureInfo.InvariantCulture)),
                new XElement("Width", this.Width.ToString(CultureInfo.InvariantCulture)),
                new XElement("Height", this.Height.ToString(CultureInfo.InvariantCulture)));
        }

        #endregion Public Methods

        #region Internal Methods

        internal PointF[] GetPoints()
        {
            return new[]
                   {
                       new PointF(this.Left, this.Top),
                       new PointF(this.Left, this.Bottom),
                       new PointF(this.Right, this.Bottom),
                       new PointF(this.Right, this.Top),
                   };
        }

        internal RectangleF GetUVOffset(RectangleF modUV)
        {
            float left = this.Left + (this.Width * modUV.Left);
            float right = this.Left + (this.Width * modUV.Right);
            float top = this.Top + (this.Height * modUV.Top);
            float bottom = this.Top + (this.Height * modUV.Bottom);
            return RectangleF.FromLTRB(left, top, right, bottom);
        }

        internal RectangleF ToScreenCoordinates(Size screenResolution)
        {
            float x1 = this.Left;
            float x2 = this.Right;
            float y1 = this.Top;
            float y2 = this.Bottom;

            x1 += RenderManager.HorizontalTexelOffset;
            x2 += RenderManager.HorizontalTexelOffset;
            y1 += RenderManager.VerticalTexelOffset;
            y2 += RenderManager.VerticalTexelOffset;

            x1 = ((x1 / screenResolution.Width) * 2) - 1;
            x2 = ((x2 / screenResolution.Width) * 2) - 1;
            y1 = -(((y1 / screenResolution.Height) * 2) - 1);
            y2 = -(((y2 / screenResolution.Height) * 2) - 1);

            return RectangleF.FromLTRB(x1, y1, x2, y2);
        }

        #endregion Internal Methods

        #endregion Methods

        #region Nested Types

        private sealed class RectangleFConverter : MiyagiTypeConverter<RectangleF>
        {
            #region Methods

            #region Protected Methods

            protected override RectangleF ConvertFromCore(string s)
            {
                return RectangleF.Parse(s);
            }

            protected override string ConvertToCore(RectangleF value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", value.X, value.Y, value.Width, value.Height);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}