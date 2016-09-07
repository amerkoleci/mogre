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
    using System;

    using global::Mogre;

    using Miyagi.UI.Controls;

    public sealed class MovableObjectAttacher : IDisposable
    {
        #region Fields

        private readonly Vector3 offset;

        private Control control;
        private MovableObject movableObject;
        private MovableObjectListener movableObjectListener;

        #endregion Fields

        #region Constructors

        public MovableObjectAttacher(Control control, MovableObject obj, Vector3 offset)
        {
            this.offset = offset;
            this.control = control;
            this.movableObject = obj;
            this.movableObjectListener = new MovableObjectListener(this);
            obj.SetListener(this.movableObjectListener);
        }

        ~MovableObjectAttacher()
        {
            this.Dispose();
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        public void Dispose()
        {
            this.movableObjectListener.Dispose();
            this.movableObjectListener = null;
            this.movableObject = null;
            this.control = null;
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Private Methods

        private bool GetScreenspaceCoords(MovableObject mo, Camera camera, out Vector2 result)
        {
            result = Vector2.ZERO;

            //if (!mo.IsInScene)
            //{
            //    return false;
            //}

            var optimizedAabb = mo.WorldAabbUpdated;
            var aabb = new AxisAlignedBox(optimizedAabb.Minimum, optimizedAabb.Maximum);

            var point = this.offset
                        + (aabb.GetCorner(AxisAlignedBox.CornerEnum.FAR_LEFT_BOTTOM)
                           + aabb.GetCorner(AxisAlignedBox.CornerEnum.FAR_RIGHT_BOTTOM)
                           + aabb.GetCorner(AxisAlignedBox.CornerEnum.NEAR_LEFT_BOTTOM)
                           + aabb.GetCorner(AxisAlignedBox.CornerEnum.NEAR_RIGHT_BOTTOM)) / 4;

            // Is the camera facing that point? If not, return false
            var cameraPlane = new Plane(camera.DerivedOrientation.ZAxis, camera.DerivedPosition);
            if (cameraPlane.GetSide(point) != Plane.Side.NEGATIVE_SIDE)
            {
                return false;
            }

            // Transform the 3D point into screen space
            point = camera.ProjectionMatrix * (camera.ViewMatrix * point);

            // Transform from coordinate space [-1, 1] to [0, 1] and update in-value
            result.x = (point.x / 2) + 0.5f;
            result.y = 1 - ((point.y / 2) + 0.5f);

            return true;
        }

        private void NotifyMoved()
        {
            ///TODO pkubat ensure this is ok
            //var size = this.control.GUI.SpriteRenderer.Viewport.Size;
            //var cam = ((Viewport)this.control.GUI.SpriteRenderer.Viewport.Native).Camera;
            //Vector2 v2;
            //if (this.GetScreenspaceCoords(this.movableObject, cam, out v2))
            //{
            //    var x = (int)(v2.x * size.Width);
            //    x -= control.Width / 2;
            //    var y = (int)(v2.y * size.Height);
            //    control.Location = new Common.Data.Point(x, y);
            //}
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        private class MovableObjectListener : MovableObject.Listener
        {
            #region Fields

            private MovableObjectAttacher parent;

            #endregion Fields

            #region Constructors

            public MovableObjectListener(MovableObjectAttacher parent)
            {
                this.parent = parent;
            }

            #endregion Constructors

            #region Methods

            #region Public Methods

            ///TODO pkubat ensure this is ok
            //public override void ObjectMoved(MovableObject param1)
            //{
            //    this.parent.NotifyMoved();
            //}

            #endregion Public Methods

            #region Protected Methods

            protected override void Dispose(bool value)
            {
                this.parent = null;
                base.Dispose(value);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}