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
    using System;

    using Miyagi.Common.Data;

    internal class ResizeHelper
    {
        #region Fields

        private const double Threshold = 0.000001;

        private readonly Action<double, double> resizeMethod;

        private double accHeightFactor = 1;
        private double accWidthFactor = 1;
        private double curHeightFactor = 1;
        private double curWidthFactor = 1;

        #endregion Fields

        #region Constructors

        public ResizeHelper(Action<double, double> resizeMethod)
        {
            this.resizeMethod = resizeMethod;
        }

        #endregion Constructors

        #region Methods

        #region Public Static Methods

        public static void Scale(ref Range self, double scale)
        {
            self = new Range(ScaleFunc(self.First, scale)(), ScaleFunc(self.Last, scale)());
        }

        public static void Scale(ref int self, double scale)
        {
            self = ScaleFunc(self, scale)();
        }

        #endregion Public Static Methods

        #region Public Methods

        public void Resize(double widthFactor, double heightFactor)
        {
            this.curWidthFactor = 1 / this.accWidthFactor;
            this.curHeightFactor = 1 / this.accHeightFactor;
            if (Math.Abs(this.curWidthFactor - 1) > Threshold || Math.Abs(this.curHeightFactor - 1) > Threshold)
            {
                this.resizeMethod(this.curWidthFactor, this.curHeightFactor);
            }

            this.accWidthFactor *= widthFactor;
            this.accHeightFactor *= heightFactor;

            if (Math.Abs(this.accWidthFactor - 1) > Threshold || Math.Abs(this.accHeightFactor - 1) > Threshold)
            {
                this.curWidthFactor = this.accWidthFactor;
                this.curHeightFactor = this.accHeightFactor;
                this.resizeMethod(this.accWidthFactor, this.accHeightFactor);
            }
        }

        public void Scale(ref Thickness self)
        {
            self = new Thickness(
                ScaleFunc(self.Left, this.curWidthFactor)(),
                ScaleFunc(self.Top, this.curHeightFactor)(),
                ScaleFunc(self.Right, this.curWidthFactor)(),
                ScaleFunc(self.Bottom, this.curHeightFactor)());
        }

        public void Scale(ref Point self)
        {
            self = new Point(
                ScaleFunc(self.X, this.curWidthFactor)(),
                ScaleFunc(self.Y, this.curHeightFactor)());
        }

        public void Scale(ref Size self)
        {
            self = new Size(
                ScaleFunc(self.Width, this.curWidthFactor)(),
                ScaleFunc(self.Height, this.curHeightFactor)());
        }

        public void Scale(ref Rectangle self)
        {
            self = new Rectangle(
                ScaleFunc(self.X, this.curWidthFactor)(),
                ScaleFunc(self.Y, this.curHeightFactor)(),
                ScaleFunc(self.Width, this.curWidthFactor)(),
                ScaleFunc(self.Height, this.curHeightFactor)());
        }

        #endregion Public Methods

        #region Private Static Methods

        private static Func<int> ScaleFunc(int self, double val)
        {
            return val >= 1
                       ? (Func<int>)(() => (int)Math.Ceiling(self * val))
                       : (() => (int)Math.Floor(self * val));
        }

        #endregion Private Static Methods

        #endregion Methods
    }
}