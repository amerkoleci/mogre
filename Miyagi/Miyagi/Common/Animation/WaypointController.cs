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
    using System.Collections.Generic;

    using Miyagi.Common.Data;

    /// <summary>
    /// A ValueController for point values describing describing a path along multiple waypoints.
    /// </summary>
    public class WaypointController : ValueController<Point>
    {
        #region Fields

        private readonly Point startPoint;
        private readonly List<Waypoint> waypoints;

        private Point currentPoint;
        private double distance;
        private Point nextPoint;
        private int nextPointIndex;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WaypointController class.
        /// </summary>
        /// <param name="startPoint">The starting point.</param>
        public WaypointController(Point startPoint)
            : base(TimeSpan.Zero)
        {
            this.startPoint = startPoint;
            this.waypoints = new List<Waypoint>();
            this.Progression = Progression.Constant;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the progression of the animation speed.
        /// </summary>
        public Progression Progression
        {
            get;
            set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds a waypoint.
        /// </summary>
        /// <param name="point">The waypoint position.</param>
        public void AddWaypoint(Point point)
        {
            this.AddWaypoint(point, null);
        }

        /// <summary>
        /// Adds a waypoint.
        /// </summary>
        /// <param name="point">The waypoint position.</param>
        /// <param name="duration">A TimeSpan describing the time it should take to reach the waypoint.</param>
        public void AddWaypoint(Point point, TimeSpan? duration)
        {
            var wp = new Waypoint
                     {
                         Point = point,
                         Duration = duration
                     };

            if ((this.waypoints.Count == 0 && point != this.startPoint)
                || (this.waypoints.Count > 0 && this.waypoints[this.waypoints.Count - 1].Point != point))
            {
                this.waypoints.Add(wp);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>A object representing the value.</returns>
        protected override object GetValue()
        {
            float exp = 1;
            switch (this.Progression)
            {
                case Progression.Decreasing:
                    exp = 0.5f;
                    break;
                case Progression.Increasing:
                    exp = 2;
                    break;
            }

            double t = this.distance * Math.Pow(this.ElapsedPercentage, exp);

            double x = this.currentPoint.X + (t * ((this.nextPoint.X - this.currentPoint.X) / this.distance));
            double y = this.currentPoint.Y + (t * ((this.nextPoint.Y - this.currentPoint.Y) / this.distance));

            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// Raises the Finished event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnFinished(EventArgs e)
        {
            if (++this.nextPointIndex >= this.waypoints.Count)
            {
                base.OnFinished(e);
            }
            else
            {
                this.SetWaypoint(this.nextPointIndex);
            }
        }

        /// <summary>
        /// Is called on startup.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnStart(EventArgs e)
        {
            base.OnStart(e);

            if (this.waypoints.Count == 0)
            {
                this.Stop();
                return;
            }

            this.nextPointIndex = 0;
            this.nextPoint = this.startPoint;
            this.SetWaypoint(0);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetWaypoint(int index)
        {
            this.currentPoint = this.nextPoint;

            Waypoint wp = this.waypoints[index];
            this.nextPoint = wp.Point;
            this.StartTime = this.MiyagiSystem.LastUpdate;

            this.distance = this.nextPoint.Distance(this.currentPoint);
            this.Duration = wp.Duration == null ? TimeSpan.FromMilliseconds(this.distance) : wp.Duration.Value;
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        private sealed class Waypoint
        {
            #region Properties

            #region Public Properties

            public TimeSpan? Duration
            {
                get;
                set;
            }

            public Point Point
            {
                get;
                set;
            }

            #endregion Public Properties

            #endregion Properties
        }

        #endregion Nested Types
    }
}