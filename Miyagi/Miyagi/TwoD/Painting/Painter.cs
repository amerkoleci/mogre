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
namespace Miyagi.TwoD.Painting
{
    using System;
    using System.Collections.Generic;

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;

    /// <summary>
    /// Provides basic methods for drawing abstract objects.
    /// </summary>
    public sealed class Painter : IDisposable
    {
        #region Fields

        private readonly TwoDManager manager;
        private readonly List<Sprite> sprites;

        #endregion Fields

        #region Constructors

        internal Painter(TwoDManager manager)
        {
            this.manager = manager;
            this.SpriteRenderer = manager.MiyagiSystem.RenderManager.Create2DRenderer();
            this.SpriteRenderer.ZOrder = int.MaxValue;
            this.sprites = new List<Sprite>();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the SpriteRenderer.
        /// </summary>
        public ISpriteRenderer SpriteRenderer
        {
            get;
            private set;
        }

        #endregion Public Properties

        #region Private Properties

        private Size ViewportSize
        {
            get
            {
                return this.SpriteRenderer.Viewport.Size;
            }
        }

        #endregion Private Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Removes all objects.
        /// </summary>
        public void Clear()
        {
            foreach (var sprite in this.sprites)
            {
                sprite.RemoveFromRenderer();
            }

            this.sprites.Clear();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.manager.MiyagiSystem.RenderManager.DestroyRenderer(this.SpriteRenderer);
            this.SpriteRenderer = null;
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="pen">The pen.</param>
        /// <param name="rectangle">The rectangle.</param>
        public void DrawFilledRectangle(Pen pen, Rectangle rectangle)
        {
            var quad = new Quad();
            quad.SetBounds(rectangle.ToScreenCoordinates(this.ViewportSize));
            quad.SetVertexUVs(new RectangleF(0, 0, 1, 1));

            this.CreateSprite(pen, quad);
        }

        /// <summary>
        /// Draws a filled triangle.
        /// </summary>
        /// <param name="pen">The pen.</param>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        public void DrawFilledTriangle(Pen pen, Point a, Point b, Point c)
        {
            var tri = new Triangle();
            tri.SetVertexLocations(
                a.ToScreenCoordinates(this.ViewportSize),
                b.ToScreenCoordinates(this.ViewportSize),
                c.ToScreenCoordinates(this.ViewportSize));
            this.CreateSprite(pen, tri);
        }

        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="pen">The pen.</param>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        public void DrawLine(Pen pen, Point p1, Point p2)
        {
            double angle = p1.AngleBetween(p2);
            float halfWidth = Math.Max(1, pen.Width / 2.0f);

            var size = this.ViewportSize;
            var quad = new Quad();
            quad.SetVertexLocations(
                p1.MoveAlongAngle(angle + 90, halfWidth).ToScreenCoordinates(size),
                p1.MoveAlongAngle(angle - 90, halfWidth).ToScreenCoordinates(size),
                p2.MoveAlongAngle(angle - 90, halfWidth).ToScreenCoordinates(size),
                p2.MoveAlongAngle(angle + 90, halfWidth).ToScreenCoordinates(size));

            quad.SetVertexUVs(new RectangleF(0, 0, 1, 1));

            this.CreateSprite(pen, quad);
        }

        /// <summary>
        /// Draws a point.
        /// </summary>
        /// <param name="pen">The pen.</param>
        /// <param name="point">The point.</param>
        public void DrawPoint(Pen pen, Point point)
        {
            this.DrawPoints(pen, point);
        }

        /// <summary>
        /// Draws points.
        /// </summary>
        /// <param name="pen">The pen.</param>
        /// <param name="points">The points.</param>
        public void DrawPoints(Pen pen, params Point[] points)
        {
            var quads = new Quad[points.Length];

            for (int x = 0; x < points.Length; x++)
            {
                var quad = new Quad();
                quad.SetBounds(new Rectangle(points[x], new Size(pen.Width, pen.Width)).ToScreenCoordinates(this.ViewportSize));
                quad.SetVertexUVs(new RectangleF(0, 0, 1, 1));

                quads[x] = quad;
            }

            this.CreateSprite(pen, quads);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="pen">The pen.</param>
        /// <param name="rectangle">The rectangle.</param>
        public void DrawRectangle(Pen pen, Rectangle rectangle)
        {
            var topQuad = new Quad();
            var topRect = new Rectangle(rectangle.Location, rectangle.Width, pen.Width);
            topQuad.SetBounds(topRect.ToScreenCoordinates(this.ViewportSize));
            topQuad.SetVertexUVs(new RectangleF(0, 0, 1, 1));

            var bottomQuad = new Quad();
            var bottomRect = new Rectangle(rectangle.Left, rectangle.Bottom - pen.Width, rectangle.Width, pen.Width);
            bottomQuad.SetBounds(bottomRect.ToScreenCoordinates(this.ViewportSize));
            bottomQuad.SetVertexUVs(new RectangleF(0, 0, 1, 1));

            var leftQuad = new Quad();
            var leftRect = new Rectangle(rectangle.Left, rectangle.Top + pen.Width, pen.Width, rectangle.Height - (pen.Width * 2));
            leftQuad.SetBounds(leftRect.ToScreenCoordinates(this.ViewportSize));
            leftQuad.SetVertexUVs(new RectangleF(0, 0, 1, 1));

            var rightQuad = new Quad();
            var rightRect = new Rectangle(rectangle.Right - pen.Width, rectangle.Top + pen.Width, pen.Width, rectangle.Height - (pen.Width * 2));
            rightQuad.SetBounds(rightRect.ToScreenCoordinates(this.ViewportSize));
            rightQuad.SetVertexUVs(new RectangleF(0, 0, 1, 1));

            this.CreateSprite(pen, topQuad, bottomQuad, leftQuad, rightQuad);
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateSprite(Pen pen, params Primitive[] primitives)
        {
            var sprite = new TwoDSprite(this.SpriteRenderer, primitives)
                         {
                             ZOrder = this.sprites.Count
                         };
            pen.Apply(sprite);

            this.sprites.Add(sprite);
        }

        #endregion Private Methods

        #endregion Methods
    }
}