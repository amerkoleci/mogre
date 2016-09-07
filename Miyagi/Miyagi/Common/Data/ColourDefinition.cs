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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Miyagi.Common.Rendering;
    using Miyagi.Internals;

    /// <summary>
    /// Maps <see cref="Colour"/> structs to verticies.
    /// </summary>
    [TypeConverter(typeof(ColourDefinitionConverter))]
    public struct ColourDefinition
    {
        #region Fields

        private readonly Colour[] colours;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourDefinition"/> struct.
        /// </summary>
        /// <param name="colours">The colours.</param>
        public ColourDefinition(params Colour[] colours)
            : this()
        {
            this.colours = colours;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourDefinition"/> struct for <see cref="Quad"/> primitives.
        /// </summary>
        /// <param name="topColour">The top colour.</param>
        /// <param name="bottomColour">The bottom colour.</param>
        public ColourDefinition(Colour topColour, Colour bottomColour)
            : this()
        {
            this.colours = new Colour[4];
            this.colours[1] = bottomColour;
            this.colours[2] = bottomColour;
            this.colours[0] = topColour;
            this.colours[3] = topColour;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the bottom colour.
        /// </summary>
        /// <value>The colour of the bottom of the text.</value>
        public ReadOnlyCollection<Colour> Colours
        {
            get
            {
                if (this.colours != null)
                {
                    return new ReadOnlyCollection<Colour>(this.colours);
                }

                return null;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="Miyagi.Common.Data.Colour"/> to <see cref="Miyagi.Common.Data.ColourDefinition"/>.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ColourDefinition(Colour colour)
        {
            return new ColourDefinition(colour);
        }

        /// <summary>
        /// Returns a value indicating whether two ColourDefinition instances are not equal.
        /// </summary>
        /// <param name="left">A ColourDefinition instance.</param>
        /// <param name="right">Another ColourDefinition instance.</param>
        /// <returns><c>true</c> if both are unequal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ColourDefinition left, ColourDefinition right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a value indicating whether two ColourDefinition instances are equal.
        /// </summary>
        /// <param name="left">A ColourDefinition instance.</param>
        /// <param name="right">Another ColourDefinition instance.</param>
        /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ColourDefinition left, ColourDefinition right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Converts a string representation to a ColourDefinition instance.
        /// </summary>
        /// <param name="s">A string representation of a ColourDefinition instance.</param>
        /// <returns>The ColourDefinition instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static ColourDefinition Parse(string s)
        {
            s = s.Replace("ColourDefinition(", string.Empty);
            s = s.Remove(s.Length - 1);

            var values = s.Split(new[] { ")," }, StringSplitOptions.RemoveEmptyEntries);

            var colours = new Colour[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                colours[i] = Colour.Parse(values[i]);
            }

            return new ColourDefinition(colours);
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified Object.
        /// </summary>
        /// <param name="obj">The other Object.</param>
        /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ColourDefinition && this.Equals((ColourDefinition)obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified ColourDefinition.
        /// </summary>
        /// <param name="other">The other ColourDefinition.</param>
        /// <returns><c>true</c> if both are equal; otherwise, <c>false</c>.</returns>
        public bool Equals(ColourDefinition other)
        {
            if (this.colours == null && other.colours != null)
            {
                return false;
            }

            if (other.colours == null && this.colours != null)
            {
                return false;
            }

            if (other.colours == null && this.colours == null)
            {
                return true;
            }

            if (this.colours.Length != other.colours.Length)
            {
                return false;
            }

            // add comparisions for all members here
            return !this.colours.Where((t, i) => t != other.colours[i]).Any();
        }

        /// <summary>
        /// Returns the HashCode of the current instance.
        /// </summary>
        /// <returns>The HashCode of the Thickness.</returns>
        public override int GetHashCode()
        {
            // combine the hash codes of all members here (e.g. with XOR operator ^)
            return this.colours.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < this.colours.Length; i++)
            {
                sb.Append(this.colours[i].ToString());
                if (i != this.colours.Length - 1)
                {
                    sb.Append(",");
                }
            }

            return string.Format(CultureInfo.InvariantCulture, "ColourDefinition({0})", sb);
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class ColourDefinitionConverter : MiyagiTypeConverter<ColourDefinition>
        {
            #region Methods

            #region Protected Methods

            protected override ColourDefinition ConvertFromCore(string s)
            {
                var split = s.Split(';');
                var colours = new Colour[split.Length];
                for (int i = 0; i < split.Length; i++)
                {
                    colours[i] = Colour.FromArgb(long.Parse(split[i]));
                }

                return new ColourDefinition(colours);
            }

            protected override string ConvertToCore(ColourDefinition value)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < value.colours.Length; i++)
                {
                    sb.Append(value.colours[i].ToArgb());
                    if (i != value.colours.Length - 1)
                    {
                        sb.Append(";");
                    }
                }

                return sb.ToString();
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}