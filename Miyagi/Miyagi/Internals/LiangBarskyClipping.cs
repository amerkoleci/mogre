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
namespace Miyagi.Internals
{
    using Miyagi.Common.Data;

    internal sealed class LiangBarskyClipping
    {
        #region Fields

        private Point clipMax;
        private Point clipMin;
        private float max;
        private float min;

        #endregion Fields

        #region Constructors

        public LiangBarskyClipping(Rectangle rect)
        {
            this.clipMin = rect.Location;
            this.clipMax = new Point(rect.X + rect.Width, rect.Y + rect.Height);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        public Point ClipLineEndPoint(Point lineStart, Point lineEnd)
        {
            var p = new PointF(lineEnd.X - lineStart.X, lineEnd.Y - lineStart.Y);
            this.min = 0;
            this.max = 1;

            if (this.Clip(-p.X, lineStart.X - this.clipMin.X)
                && this.Clip(p.X, this.clipMax.X - lineStart.X)
                && this.Clip(-p.Y, lineStart.Y - this.clipMin.Y)
                && this.Clip(p.Y, this.clipMax.Y - lineStart.Y)
                && this.max < 1)
            {
                lineEnd = new Point(
                    (int)(lineStart.X + (this.max * p.X)),
                    (int)(lineStart.Y + (this.max * p.Y)));
            }

            return lineEnd;
        }

        #endregion Public Methods

        #region Private Methods

        private bool Clip(float proj, float dist)
        {
            if (proj == 0)
            {
                if (dist < 0)
                {
                    return false;
                }
            }
            else
            {
                float amount = dist / proj;
                if (proj < 0)
                {
                    if (amount > this.max)
                    {
                        return false;
                    }

                    if (amount > this.min)
                    {
                        this.min = amount;
                    }
                }
                else
                {
                    if (amount < this.min)
                    {
                        return false;
                    }

                    if (amount < this.max)
                    {
                        this.max = amount;
                    }
                }
            }

            return true;
        }

        #endregion Private Methods

        #endregion Methods
    }
}