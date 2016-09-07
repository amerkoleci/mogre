/*
 Miyagi.MogreBackend v1.0
 Copyright (c) 2010 Tobias Bohnen

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
namespace Miyagi.Backend.Mogre
{
    using global::Mogre;

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;

    public sealed class MogreViewport : IViewport
    {
        #region Fields

        private readonly Camera camera;

        private Miyagi.Common.Data.Rectangle bounds;

        #endregion Fields

        #region Constructors

        public MogreViewport(Camera camera, int width, int height)
        {
            this.camera = camera;
            this.bounds = new Miyagi.Common.Data.Rectangle(0, 0, width, height);
            this.UpdateBounds();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        public Miyagi.Common.Data.Rectangle Bounds
        {
            get
            {
                return this.bounds;
            }
        }

        public object Native
        {
            get
            {
                return this.camera.LastViewport;
            }
        }

        public Point Offset
        {
            get
            {
                return this.bounds.Location;
            }
        }

        public Size Size
        {
            get
            {
                return this.bounds.Size;
            }
        }

        public Camera Camera
        {
            get
            {
                return this.camera;
            }
        }


        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        public void UpdateBounds()
        {
            if (this.camera.LastViewport == null)
                return;

            int left, top, width, height;
            this.camera.LastViewport.GetActualDimensions(out left, out top, out width, out height);
            this.bounds = new Miyagi.Common.Data.Rectangle(left, top, width, height);
        }

        #endregion Public Methods

        #endregion Methods
    }
}