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
namespace Miyagi.UI.Controls.Elements
{
    using System;
    using System.Collections.Generic;

    using Miyagi.Common.Data;
    using Miyagi.Common.Events;
    using Miyagi.Common.Rendering;
    using Miyagi.Common.Resources;

    /// <summary>
    /// Interface for Elements.
    /// </summary>
    public interface IElement : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether sprites of the Element should not be cropped.
        /// </summary>
        bool CroppingDisabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the owner of the element.
        /// </summary>
        /// <value>A IElementOwner representing the owner.</value>
        IElementOwner Owner
        {
            get;
        }

        /// <summary>
        /// Gets the owning control.
        /// </summary>
        /// <value>A Control representing the owner of the element.</value>
        Control OwningControl
        {
            get;
        }

        /// <summary>
        /// Gets the sprite of the element.
        /// </summary>
        /// <value>The Sprite of the element.</value>
        UISprite Sprite
        {
            get;
        }

        /// <summary>
        /// Gets the sprite renderer.
        /// </summary>
        ISpriteRenderer SpriteRenderer
        {
            get;
        }

        /// <summary>
        /// Gets a collection of subelement.
        /// </summary>
        IEnumerable<IElement> SubElements
        {
            get;
        }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        Texture Texture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the UpdateType.
        /// </summary>
        UpdateTypes UpdateType
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Applies the opacity.
        /// </summary>
        void ApplyOpacity();

        /// <summary>
        /// Applies the TextureFiltering.
        /// </summary>
        void ApplyTextureFiltering();

        /// <summary>
        /// Applies the visibility.
        /// </summary>
        void ApplyVisibility();

        /// <summary>
        /// Applies the ZOrder.
        /// </summary>
        void ApplyZOrder();

        /// <summary>
        /// Returns whether the Sprites property is null.
        /// </summary>
        /// <returns><c>true</c> if the Sprites property is null; otherwise, <c>false</c>.</returns>
        bool AreAllSpritesNull();

        /// <summary>
        /// Gets the ZOrder.
        /// </summary>
        /// <returns>An <see cref="Int32"/> representing the zorder.</returns>
        int GetZOrder();

        /// <summary>
        /// Performs a hit test.
        /// </summary>
        /// <param name="p">The coordinate.</param>
        /// <returns><c>true</c> if the Sprite is hit; otherwise, <c>false</c>.</returns>
        bool HitTest(Point p);

        /// <summary>
        /// Injects a pressed key.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> to inject.</param>
        void InjectKeyDown(KeyboardEventArgs e);

        /// <summary>
        /// Inject a released key.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> to inject.</param>
        void InjectKeyUp(KeyboardEventArgs e);

        /// <summary>
        /// Inject a pressed mouse button.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> to inject.</param>
        void InjectMouseDown(MouseButtonEventArgs e);

        /// <summary>
        /// Inject a dragged mouse.
        /// </summary>
        /// <param name="e">The <see cref="ChangedValueEventArgs{T}"/> to inject.</param>
        void InjectMouseDrag(ChangedValueEventArgs<Point> e);

        /// <summary>
        /// Inject an entering mouse.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> to inject.</param>
        void InjectMouseEnter(MouseEventArgs e);

        /// <summary>
        /// Inject a hovering mouse.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> to inject.</param>
        void InjectMouseHover(MouseEventArgs e);

        /// <summary>
        /// Inject a leaving mouse.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> to inject.</param>
        void InjectMouseLeave(MouseEventArgs e);

        /// <summary>
        /// Inject a released mouse button.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> to inject.</param>
        void InjectMouseUp(MouseButtonEventArgs e);

        /// <summary>
        /// Inject a moved mouse wheel.
        /// </summary>
        /// <param name="e">The <see cref="ValueEventArgs{T}"/> to inject.</param>
        void InjectMouseWheelMoved(ValueEventArgs<int> e);

        /// <summary>
        /// Moves the sprites of the element.
        /// </summary>
        /// <param name="offset">The distance to move.</param>
        void Move(Point offset);

        /// <summary>
        /// Removes the sprites.
        /// </summary>
        void RemoveSprite();

        /// <summary>
        /// Resizes the sprites of the element.
        /// </summary>
        /// <param name="diff">The distance to resize.</param>
        void Resize(Point diff);

        /// <summary>
        /// Restarts the texture animation.
        /// </summary>
        void RestartTextureAnimation();

        /// <summary>
        /// Stops the texture animation.
        /// </summary>
        void StopTextureAnimation();

        /// <summary>
        /// Updates the element.
        /// </summary>
        void Update();

        void Update(Point deltaLocation, Point deltaSize);

        /// <summary>
        /// Updates the crop of the sprites.
        /// </summary>
        void UpdateSpriteCrop();

        #endregion Methods
    }
}