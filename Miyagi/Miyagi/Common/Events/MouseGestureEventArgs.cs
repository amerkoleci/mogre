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
namespace Miyagi.Common.Events
{
    using System.Collections.Generic;

    using Miyagi.Common.Data;

    /// <summary>
    /// EventArgs for mouse gesture events.
    /// </summary>
    public class MouseGestureEventArgs : MiyagiEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MouseGestureEventArgs class.
        /// </summary>
        /// <param name="mouseGestures">The list of MouseGestures.</param>
        /// <param name="mouseGesturesLocations">The list of MouseGesture positions.</param>
        /// <param name="endLocation">The end position.</param>
        public MouseGestureEventArgs(IList<MouseGestures> mouseGestures, IList<Point> mouseGesturesLocations, Point endLocation)
        {
            this.Gestures = mouseGestures;
            this.Locations = mouseGesturesLocations;
            this.EndLocation = endLocation;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the end position.
        /// </summary>
        /// <value>A Location representing the end position.</value>
        public Point EndLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of recogized gestures.
        /// </summary>
        /// <value>A list of recogized gestures.</value>
        public IList<MouseGestures> Gestures
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of start positions for the Gestures list.
        /// </summary>
        /// <value>A list of start positions for the Gestures list.</value>
        public IList<Point> Locations
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties
    }
}