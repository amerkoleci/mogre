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

    using Quaternion = global::Mogre.Quaternion;

    using Vector3 = global::Mogre.Vector3;

    internal class MogreSpriteRenderer3D : MogreSpriteRenderer2D, ISpriteRenderer3D
    {
        #region Fields

        private readonly RenderSystem rs = Root.Singleton.RenderSystem;

        private IRenderable3D _renderable;

        #endregion Fields

        #region Constructors

        internal MogreSpriteRenderer3D(MogreRenderManager owner)
            : base(owner)
        {
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        public void SetRenderable(IRenderable3D renderable)
        {
            this._renderable = renderable;
        }

        public override bool TransformCoordinate(ref int x, ref int y)
        {
            //var ori = this._renderable.Orientation;
            //var plane = new Plane(new Quaternion(ori.W, ori.X, ori.Y, ori.Z) * Vector3.UNIT_Z, 0);
            //plane.normal.Normalise();

            //float xx = x / (float)this.Viewport.Size.Width;
            //float yy = y / (float)this.Viewport.Size.Height;
            //var ray = this.MogreRenderManager.MogreViewport.Camera.GetCameraToViewportRay(xx, yy);
            //var off = this._renderable.Offset;
            //ray.Origin -= new Vector3(off.X, off.Y, off.Z);
            //var result = ray.Intersects(plane);

            //if (result.first)
            //{
            //    var v = new Quaternion(ori.W, ori.X, ori.Y, ori.Z).Inverse() * ray.GetPoint(result.second);
            //    x = (int)((v.x + 1) / 2 * this.Viewport.Size.Width);
            //    y = (int)((-v.y + 1) / 2 * this.Viewport.Size.Height);
            //    return true;
            //}

            return false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override MogreVertex CreateVertex(Vertex vertex)
        {
            MogreVertex v;

            var loc = Miyagi.Common.Data.Quaternion.Transform(vertex.Location, _renderable.Orientation) + _renderable.Offset;
            v.Location = new Vector3(loc.X, loc.Y, loc.Z);

            v.UV = new Vector2(vertex.UV.X, vertex.UV.Y);

            Colour c = vertex.Colour;
            uint val32;
            this.rs.ConvertColourValue(new ColourValue(c.Red / 255f, c.Green / 255f, c.Blue / 255f, c.Alpha / 255f), out val32);
            v.Colour = val32;

            return v;
        }

        protected override void PrepareMatrices()
        {
            this.rs._setWorldMatrix(Matrix4.IDENTITY);
        }

        protected override void RestoreMatrices()
        {
        }

        #endregion Protected Methods

        #endregion Methods
    }
}