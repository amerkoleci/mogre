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

    /// <summary>
    /// A ValueController describing a sine wave.
    /// </summary>
    /// <typeparam name = "T">The type of the value.</typeparam>
    public class SineWaveValueController<T> : WaveValueController<T>
        where T : struct
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SineWaveValueController class.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="frequency">The frequency of the wave.</param>
        /// <param name="phase">The phase of the wave.</param>
        /// <param name="duration">A TimeSpan representing the duration.</param>
        public SineWaveValueController(T minValue, T maxValue, float frequency, float phase, TimeSpan duration)
            : base(minValue, maxValue, frequency, phase, duration)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SineWaveValueController class.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="duration">A TimeSpan representing the duration.</param>
        public SineWaveValueController(T minValue, T maxValue, TimeSpan duration)
            : base(minValue, maxValue, 1, 0, duration)
        {
        }

        #endregion Constructors

        #region Methods

        #region Protected Methods

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>A object representing the value.</returns>
        protected override double GetValueCore()
        {
            return Math.Sin(2 * Math.PI * (this.Time + 0.75f)) / 2;
        }

        #endregion Protected Methods

        #endregion Methods
    }
}