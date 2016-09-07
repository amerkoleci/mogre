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
namespace Miyagi.Common.Rendering
{
    using System;

    using Miyagi.Common.Data;

    /// <summary>
    /// Interface for sprite renderer.
    /// </summary>
    public interface ISpriteRenderer : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the buffer is dirty.
        /// </summary>
        bool BufferDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sprite renderer should cache to texture.
        /// </summary>
        bool CacheToTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the hardware buffer capacity.
        /// </summary>
        int HardwareBufferCapacity
        {
            get;
        }

        /// <summary>
        /// Gets the render manager.
        /// </summary>
        RenderManager RenderManager
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sprite list needs to be sorted.
        /// </summary>
        bool SpriteOrderDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the viewport.
        /// </summary>
        IViewport Viewport
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the zorder.
        /// </summary>
        int ZOrder
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a Sprite to the Renderer.
        /// </summary>
        /// <param name="sprite">The sprite to add.</param>
        /// <returns><c>true</c> if the sprite has been added successfully; otherwise, <c>false</c>.</returns>
        bool AddSprite(Sprite sprite);

        /// <summary>
        /// Removes a Sprite from the Renderer.
        /// </summary>
        /// <param name="sprite">The sprite to remove.</param>
        /// <returns><c>true</c> if the sprite has been removed successfully; otherwise, <c>false</c>.</returns>
        bool RemoveSprite(Sprite sprite);

        /// <summary>
        /// Renders the sprites.
        /// </summary>
        void RenderSprites();

        /// <summary>
        /// Sort the sprites.
        /// </summary>
        void SortSprites();

        /// <summary>
        /// Transforms a screen coordinate to a viewport coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns><c>true</c> if the coordinate is inside the viewport; otherwise, <c>false</c>.</returns>
        bool TransformCoordinate(ref int x, ref int y);

        /// <summary>
        /// Transforms a screen coordinate to a viewport coordinate.
        /// </summary>
        /// <param name="p">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        Point TransformCoordinate(Point p);

        #endregion Methods
    }
}