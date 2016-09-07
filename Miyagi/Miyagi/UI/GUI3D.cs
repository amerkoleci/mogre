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
namespace Miyagi.UI
{
    using Miyagi.Common;
    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.UI.Controls;

    /// <summary>
    /// A GUI represents a composite group of controls.
    /// </summary>
    public class GUI3D : GUI, IRenderable3D
    {
        #region Fields

        private Vector3 offset;
        private Quaternion orientation;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GUI3D"/> class.
        /// </summary>
        public GUI3D()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GUI3D"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public GUI3D(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public Vector3 Offset
        {
            get
            {
                return this.offset;
            }

            set
            {
                if (this.offset != value)
                {
                    if (this.SpriteRenderer != null)
                    {
                        this.SpriteRenderer.BufferDirty = true;
                    }

                    this.offset = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public Quaternion Orientation
        {
            get
            {
                return this.orientation;
            }

            set
            {
                if (this.orientation != value)
                {
                    if (this.SpriteRenderer != null)
                    {
                        this.SpriteRenderer.BufferDirty = true;
                    }

                    this.orientation = value;
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns the topmost control at the specified position.
        /// </summary>
        /// <param name="p">The coordinate of the position where you want to look for a control.</param>
        /// <returns>
        /// If there is a control at the position the topmost, otherwise null.
        /// </returns>
        public override Control GetTopControlAt(Point p)
        {
            int x = p.X;
            int y = p.Y;
            return this.SpriteRenderer.TransformCoordinate(ref x, ref y) ? GetTopControlAt(this.Controls, new Point(x, y)) : null;
        }

        #endregion Public Methods

        #region Protected Internal Methods

        /// <summary>
        /// Creates the sprite renderer.
        /// </summary>
        /// <param name="system">The system.</param>
        protected internal override void CreateSpriteRenderer(MiyagiSystem system)
        {
            if (this.SpriteRenderer != null)
            {
                this.DestroySpriteRenderer();
            }

            this.SpriteRenderer = system.RenderManager.Create3DRenderer();
            ((ISpriteRenderer3D)this.SpriteRenderer).SetRenderable(this);
            this.SpriteRenderer.ZOrder = this.ZOrder;
        }

        #endregion Protected Internal Methods

        #endregion Methods
    }
}