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
	using System.Collections.Generic;
	using System.Diagnostics;

	using Miyagi.Common.Data;

	/// <summary>
	/// A sprite renderer.
	/// </summary>
	public abstract class SpriteRenderer : ISpriteRenderer
	{
		#region Fields

		private readonly List<Sprite> spriteList;

		private bool cacheToTexture;
		private IViewport viewport;
		private int zorder;
		private bool bufferDirty;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the SpriteRenderer class.
		/// </summary>
		/// <param name="owner">The owning RenderManager.</param>
		protected SpriteRenderer(RenderManager owner)
		{
			this.RenderManager = owner;
			this.spriteList = new List<Sprite>();
			this.Viewport = owner.MainViewport;
		}

		/// <summary>
		/// Finalizes an instance of the SpriteRenderer class.
		/// </summary>
		~SpriteRenderer()
		{
			this.Dispose(false);
		}

		#endregion Constructors

		#region Properties

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether the buffer is dirty.
		/// </summary>
		/// <value><c>true</c> if the buffer is dirty; otherwise, <c>false</c>.</value>
		public bool BufferDirty
		{
			get
			{
				return bufferDirty;
			}
			set
			{
				if (bufferDirty != value)
				{
					bufferDirty = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the sprite renderer should cache to texture.
		/// </summary>
		/// <value></value>
		public bool CacheToTexture
		{
			get
			{
				return this.cacheToTexture;
			}

			set
			{
				if (this.cacheToTexture != value)
				{
					this.cacheToTexture = value;
					this.OnCacheToTextureChanged();
				}
			}
		}

		/// <summary>
		/// Gets the hardware buffer capacity.
		/// </summary>
		/// <value>An <see cref="int"/> representing the hardware buffer capacity.</value>
		public abstract int HardwareBufferCapacity
		{
			get;
		}

		/// <summary>
		/// Gets the owning RenderManager.
		/// </summary>
		public RenderManager RenderManager
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the sprite list needs to be sorted.
		/// </summary>
		public bool SpriteOrderDirty
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the viewport.
		/// </summary>
		public IViewport Viewport
		{
			get
			{
				return this.viewport;
			}

			set
			{
				if (this.viewport != value)
				{
					this.viewport = value;
					this.OnViewportChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the zorder.
		/// </summary>
		public int ZOrder
		{
			get
			{
				return this.zorder;
			}

			set
			{
				if (this.zorder != value)
				{
					this.zorder = value;
					this.RenderManager.SortRenderers();
				}
			}
		}

		#endregion Public Properties

		#region Protected Properties

		/// <summary>
		/// Gets the list of sprites.
		/// </summary>
		protected IList<Sprite> SpriteList
		{
			get
			{
				return this.spriteList;
			}
		}

		/// <summary>
		/// Gets or sets the number of triangles.
		/// </summary>
		protected int TriangleCount
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the vertex count.
		/// </summary>
		/// <value>The vertex count.</value>
		protected int VertexCount
		{
			get;
			set;
		}

		#endregion Protected Properties

		#endregion Properties

		#region Methods

		#region Public Methods

		/// <summary>
		/// Adds a Sprite to the Renderer.
		/// </summary>
		/// <param name="sprite">The sprite to add.</param>
		/// <returns><c>true</c> if the sprite has been added successfully; otherwise, <c>false</c>.</returns>
		public virtual bool AddSprite(Sprite sprite)
		{
			if (sprite == null)
			{
				return false;
			}

			this.SpriteList.Add(sprite);
			this.VertexCount += sprite.VertexCount;
			this.TriangleCount += sprite.TriangleCount;
			this.SpriteOrderDirty = true;

			return true;
		}

		/// <summary>
		/// Disposes the sprite renderer.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Removes a Sprite from the Renderer.
		/// </summary>
		/// <param name="sprite">The sprite to remove.</param>
		/// <returns><c>true</c> if the sprite has been removed successfully; otherwise, <c>false</c>.</returns>
		public virtual bool RemoveSprite(Sprite sprite)
		{
			if (sprite == null || !this.SpriteList.Remove(sprite))
			{
				return false;
			}

			this.TriangleCount -= sprite.TriangleCount;
			this.VertexCount -= sprite.VertexCount;
			Debug.Assert(this.TriangleCount >= 0, "TriangleCount inconsistency.");
			return true;
		}

		/// <summary>
		/// Renders the sprites.
		/// </summary>
		public abstract void RenderSprites();

		/// <summary>
		/// Sort the sprites.
		/// </summary>
		public virtual void SortSprites()
		{
			this.spriteList.Sort(new Sprite.SpriteZOrderComparer());
			this.SpriteOrderDirty = false;
			this.BufferDirty = true;
		}

		/// <summary>
		/// Transforms a screen coordinate to a viewport coordinate.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <returns><c>true</c> if the coordinate is inside the viewport; otherwise, <c>false</c>.</returns>
		public virtual bool TransformCoordinate(ref int x, ref int y)
		{
			var offset = this.Viewport.Offset;
			x -= offset.X;
			y -= offset.Y;
			return true;
		}

		/// <summary>
		/// Transforms a screen coordinate to a viewport coordinate.
		/// </summary>
		/// <param name="p">The coordinate.</param>
		/// <returns>The transformed coordinate.</returns>
		public Point TransformCoordinate(Point p)
		{
			int x = p.X, y = p.Y;
			this.TransformCoordinate(ref x, ref y);
			return new Point(x, y);
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Disposes the sprite renderer.
		/// </summary>
		/// <param name="disposing">Whether Dispose has been called.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.SpriteList.Clear();
				this.RenderManager = null;
			}
		}

		/// <summary>
		/// Called when <see cref="CacheToTexture"/> changes.
		/// </summary>
		protected virtual void OnCacheToTextureChanged()
		{
		}

		/// <summary>
		/// Called when <see cref="Viewport"/> changes.
		/// </summary>
		protected virtual void OnViewportChanged()
		{
		}

		#endregion Protected Methods

		#endregion Methods
	}
}