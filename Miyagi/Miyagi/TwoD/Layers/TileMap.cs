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
namespace Miyagi.TwoD.Layers
{
    using System;
    using System.Collections.Generic;

    using Miyagi.Common.Data;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;

    /// <summary>
    /// A 2d overlay composed of tiles.
    /// </summary>
    public class TileMap : LayerElement
    {
        #region Fields

        private readonly List<TwoDSprite> sprites;

        private Point location;
        private string[][] map;
        private bool mapNeedsUpdate;
        private TimeSpan textureAnimationTime;
        private TextureCollection textures;
        private Size tileSize;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/> class.
        /// </summary>
        public TileMap()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        public TileMap(string name)
            : base(name)
        {
            this.sprites = new List<TwoDSprite>();
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public Point Location
        {
            get
            {
                return this.location;
            }

            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    this.mapNeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <value>The map.</value>
        public string[][] Map
        {
            get
            {
                return this.map;
            }

            set
            {
                if (this.map != value)
                {
                    this.map = value;
                    this.mapNeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the textures.
        /// </summary>
        /// <value>The textures.</value>
        public TextureCollection Textures
        {
            get
            {
                return this.textures;
            }

            set
            {
                if (this.textures != value)
                {
                    this.textures = value;
                    this.mapNeedsUpdate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of a tile.
        /// </summary>
        public Size TileSize
        {
            get
            {
                return this.tileSize;
            }

            set
            {
                if (this.tileSize != value)
                {
                    this.tileSize = value;
                    this.mapNeedsUpdate = true;
                }
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Updates the Element.
        /// </summary>
        public override void Update()
        {
            if (this.mapNeedsUpdate)
            {
                this.DestroyElement();
                this.mapNeedsUpdate = false;

                var point = this.location;
                var viewportSize = this.ViewportSize;
                foreach (var row in this.Map)
                {
                    for (int index = 0; index < row.Length; index++)
                    {
                        var quad = new Quad(new Rectangle(point, this.tileSize).ToScreenCoordinates(viewportSize));
                        var sprite = new TwoDSprite(this.Layer.SpriteRenderer, quad)
                                     {
                                         Opacity = 1f,
                                         Visible = true
                                     };

                        this.sprites.Add(sprite);

                        point += new Point(this.tileSize.Width, 0);
                    }

                    point = new Point(this.location.X, point.Y + this.tileSize.Height);
                }
            }

            int i = 0;
            for (int rowIndex = 0; rowIndex < this.Map.Length; rowIndex++)
            {
                var row = this.Map[rowIndex];
                for (int cellIndex = 0; cellIndex < row.Length; cellIndex++)
                {
                    var cell = row[cellIndex];
                    var frame = this.textures[cell].GetFrameFromTime(this.textureAnimationTime);
                    this.sprites[i].SetTexture(frame.FileName);
                    this.sprites[i].SetUV(frame.UV.GetPoints());
                    i++;
                }
            }

            this.textureAnimationTime += this.Layer.TwoDManager.MiyagiSystem.TimeSinceLastUpdate;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Destroys the Element.
        /// </summary>
        protected override void DestroyElement()
        {
            foreach (var sprite in this.sprites)
            {
                sprite.RemoveFromRenderer();
            }

            this.sprites.Clear();
        }

        /// <summary>
        /// Disposes the Element.
        /// </summary>
        /// <param name="disposing">Whether Dispose has been called.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.DestroyElement();
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}