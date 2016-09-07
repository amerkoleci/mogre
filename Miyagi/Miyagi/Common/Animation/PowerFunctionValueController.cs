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
namespace Miyagi.Common.Animation
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A ValueController with changes a value according to  a power function.
    /// </summary>
    /// <typeparam name = "T">The type of the value.</typeparam>
    public class PowerFunctionValueController<T> : ValueController<T>
        where T : struct
    {
        #region Fields

        private readonly T endValue;
        private readonly double min;
        private readonly double range;
        private readonly T startValue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PowerFunctionValueController class.
        /// </summary>
        /// <param name="startValue">The start value.</param>
        /// <param name="endValue">The end value.</param>
        /// <param name="exponent">The exponent of the power function.</param>
        /// <param name="duration">A TimeSpan representing the duration.</param>
        public PowerFunctionValueController(T startValue, T endValue, float exponent, TimeSpan duration)
            : base(duration)
        {
            this.startValue = startValue;
            this.endValue = endValue;
            this.Exponent = exponent;
            this.min = Convert.ToDouble(this.startValue, CultureInfo.InvariantCulture);
            this.range = Convert.ToDouble(this.endValue, CultureInfo.InvariantCulture) - this.min;
        }

        #endregion Constructors

        #region Properties

        #region Protected Properties

        /// <summary>
        /// Gets the exponent of the power function.
        /// </summary>
        protected float Exponent
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>A object representing the value.</returns>
        protected override object GetValue()
        {
            return this.Exponent < 0 && this.ElapsedPercentage == 0
                       ? this.min
                       : this.min + (this.range * Math.Pow(this.ElapsedPercentage, this.Exponent));
        }

        #endregion Protected Methods

        #endregion Methods
    }
}