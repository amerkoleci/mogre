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
    /// The base class for ValueController which describe wave.
    /// </summary>
    /// <typeparam name = "T">The type of the value.</typeparam>
    public abstract class WaveValueController<T> : ValueController<T>
        where T : struct
    {
        #region Fields

        private readonly float frequency;
        private readonly float phase;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WaveValueController class.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="frequency">The frequency of the wave.</param>
        /// <param name="phase">The phase of the wave.</param>
        /// <param name="duration">A TimeSpan representing the duration.</param>
        protected WaveValueController(T minValue, T maxValue, float frequency, float phase, TimeSpan duration)
            : base(duration)
        {
            this.frequency = frequency;
            this.phase = phase;
            this.Min = Convert.ToDouble(minValue, CultureInfo.InvariantCulture);
            this.Range = Convert.ToDouble(maxValue, CultureInfo.InvariantCulture) - this.Min;
        }

        #endregion Constructors

        #region Properties

        #region Protected Properties

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        protected double Min
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the range of the wave.
        /// </summary>
        protected double Range
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the time.
        /// </summary>
        protected double Time
        {
            get
            {
                return (this.frequency * this.ElapsedPercentage) + this.phase;
            }
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>A object representing the value.</returns>
        protected override sealed object GetValue()
        {
            double val = this.GetValueCore();
            return this.Min + (this.Range * val);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>A object representing the value.</returns>
        protected abstract double GetValueCore();

        #endregion Protected Methods

        #endregion Methods
    }
}