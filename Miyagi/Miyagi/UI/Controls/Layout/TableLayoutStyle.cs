/*
// Copyright (c) 2009 Realmforge Studios GmbH.
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Author: Mario Fernandez
// Created: 4/29/2009 5:22:39 PM
 */
namespace Miyagi.UI.Controls.Layout
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    using Miyagi.Common;
    using Miyagi.Internals;

    /// <summary>
    /// Represents the look and feel of cells in a TableLayout.
    /// </summary>
    [TypeConverter(typeof(TableLayoutStyleConverter))]
    public struct TableLayoutStyle : INamable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TableLayoutStyle struct.
        /// </summary>
        /// <param name="sizeType">The SizeType.</param>
        /// <param name="magnitude">The magnitude.</param>
        public TableLayoutStyle(SizeType sizeType, float magnitude)
            : this()
        {
            this.SizeType = sizeType;
            this.Magnitude = magnitude;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the size representing either the width or the height of a cell, in the units specified by the <see cref="SizeType"/> property.
        /// </summary>
        /// <value>The size representing either the width or the height of a cell.</value>
        public float Magnitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a flag indicating how a row or column should be sized relative to its containing table.
        /// </summary>
        /// <value>A flag indicating how a row or column should be sized relative to its containing table.</value>
        public SizeType SizeType
        {
            get;
            set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Nested Types

        private sealed class TableLayoutStyleConverter : TypeConverter
        {
            #region Methods

            #region Public Methods

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(TableLayoutStyle);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value.GetType() != typeof(string))
                {
                    throw new InvalidOperationException();
                }

                var values = ((string)value).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                return new TableLayoutStyle(values[0].ParseEnum<SizeType>(), float.Parse(values[1], CultureInfo.InvariantCulture));
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType != typeof(string))
                {
                    throw new InvalidOperationException();
                }

                TableLayoutStyle t = (TableLayoutStyle)value;
                return t.SizeType + "," + t.Magnitude.ToString(CultureInfo.InvariantCulture);
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(value);
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            #endregion Public Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}