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
	using System.Linq;

	using Miyagi.Common.Data;
	using Miyagi.Common.Resources;

	/// <summary>
	/// A representation of a Sprite.
	/// </summary>
	public class Sprite
	{
		#region Fields

		private readonly Backend backend;
		private readonly Primitive[] primitives;

		private IList<GpuProgram> gpuPrograms;
		private float opacity;
		private TextureFiltering texFilter;
		private object textureHandle;
		private bool visible;
		private int zorder;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Sprite class.
		/// </summary>
		/// <param name="spriteRenderer">The owning <see cref="ISpriteRenderer"/>.</param>
		/// <param name="primitives">The primitives.</param>
		public Sprite(ISpriteRenderer spriteRenderer, params Primitive[] primitives)
		{
			if (primitives == null || primitives.Length == 0)
			{
				throw new ArgumentNullException("primitives");
			}

			this.SpriteRenderer = spriteRenderer;
			this.ViewportSize = spriteRenderer.Viewport.Size;
			this.backend = spriteRenderer.RenderManager.MiyagiSystem.Backend;
			this.primitives = primitives;

			foreach (var pri in primitives)
			{
				this.TriangleCount += pri.TriangleCount;
				this.VertexCount += pri.VertexCount;
			}

			spriteRenderer.AddSprite(this);
			this.textureHandle = RenderManager.TransparentTextureHandle;
		}

		#endregion Constructors

		#region Properties

		#region Public Properties

		/// <summary>
		/// Gets or sets the list of GpuPrograms.
		/// </summary>
		public IList<GpuProgram> GpuPrograms
		{
			get
			{
				return this.gpuPrograms;
			}

			set
			{
				if (this.gpuPrograms != value)
				{
					this.gpuPrograms = value;
					this.SpriteRenderer.BufferDirty = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the opacity of the Sprite.
		/// </summary>
		/// <value>A float representing the opacity of the Sprite.</value>
		public float Opacity
		{
			get
			{
				return this.opacity;
			}

			set
			{
				if (this.opacity != value)
				{
					this.opacity = value;
					this.SpriteRenderer.BufferDirty = true;
				}
			}
		}

		/// <summary>
		/// Gets the primitive count.
		/// </summary>
		public int PrimitiveCount
		{
			get
			{
				return this.primitives.Length;
			}
		}

		/// <summary>
		/// Gets or sets the texture filtering of the Sprite.
		/// </summary>
		/// <value>A TextureFiltering enum representing the texture filtering of the Sprite.</value>
		public TextureFiltering TexFilter
		{
			get
			{
				return this.texFilter;
			}

			set
			{
				if (this.texFilter != value)
				{
					this.texFilter = value;
					this.SpriteRenderer.BufferDirty = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the texture handle of the Sprite.
		/// </summary>
		/// <value>An <see cref="long"/> representing the texture handle of the Sprite.</value>
		public object TextureHandle
		{
			get
			{
				return this.textureHandle;
			}

			set
			{
				if (this.textureHandle != value)
				{
					this.textureHandle = value;
					this.SpriteRenderer.BufferDirty = true;
				}
			}
		}

		/// <summary>
		/// Gets the triangle count.
		/// </summary>
		public int TriangleCount
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the vertex count.
		/// </summary>
		public int VertexCount
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the Sprite is visible.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public bool Visible
		{
			get
			{
				return this.visible && this.Opacity != 0;
			}

			set
			{
				if (this.visible != value)
				{
					this.visible = value;
					this.SpriteRenderer.BufferDirty = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the ZOrder of the Sprite.
		/// </summary>
		/// <value>An <see cref="int"/> representing the ZOrder of the Sprite.</value>
		/// <remarks>In order for changes to take effect, <see cref="Rendering.SpriteRenderer.SortSprites"/> of the responsible <see cref="Rendering.SpriteRenderer"/> has to be called.</remarks>
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
					this.SpriteRenderer.SpriteOrderDirty = true;
				}
			}
		}

		#endregion Public Properties

		#region Protected Properties

		/// <summary>
		/// Gets the backend.
		/// </summary>
		protected Backend Backend
		{
			get
			{
				return this.backend;
			}
		}

		/// <summary>
		/// Gets the sprite renderer.
		/// </summary>
		protected ISpriteRenderer SpriteRenderer
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the aspect ratio of the viewport.
		/// </summary>
		protected float ViewportAspectRatio
		{
			get
			{
				return (float)this.ViewportSize.Width / this.ViewportSize.Height;
			}
		}

		/// <summary>
		/// Gets the size of the viewport.
		/// </summary>
		protected Size ViewportSize
		{
			get;
			private set;
		}

		#endregion Protected Properties

		#endregion Properties

		#region Methods

		#region Public Methods

		/// <summary>
		/// Gets the primitive at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The primitive at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
		public Primitive GetPrimitive(int index)
		{
			if (index > this.primitives.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}

			return this.primitives[index];
		}

		/// <summary>
		/// Gets a value indicating whether the provided coordinates are inside the control.
		/// </summary>
		/// <param name="p">The coordinate.</param>
		/// <returns>
		/// <c>true</c> if the coordinates are inside the control; otherwise, <c>false</c>.
		/// </returns>
		public bool HitTest(Point p)
		{
			float x = (((float)p.X / this.ViewportSize.Width) * 2) - 1;
			float y = -((((float)p.Y / this.ViewportSize.Height) * 2) - 1);

			return this.HitTest(x, y) != null;
		}

		/// <summary>
		/// Moves the sprite.
		/// </summary>
		/// <param name="offset">A <see cref="Point"/> representing the offset.</param>
		public void Move(Point offset)
		{
			var offsetf = new PointF(
				((float)offset.X / this.ViewportSize.Width) * 2,
				-(((float)offset.Y / this.ViewportSize.Height) * 2));
			this.ForEachPrimitive(q => q.Move(offsetf));
			this.SpriteRenderer.BufferDirty = true;
		}

		/// <summary>
		/// Removes the sprite from its renderer.
		/// </summary>
		public void RemoveFromRenderer()
		{
			this.SpriteRenderer.RemoveSprite(this);
			this.SpriteRenderer.BufferDirty = true;
		}

		/// <summary>
		/// Resizes the sprite.
		/// </summary>
		/// <param name="offset">A <see cref="Point"/> representing the offset.</param>
		public void Resize(Point offset)
		{
			var offsetf = new PointF(
				((float)offset.X / this.ViewportSize.Width) * 2,
				-(((float)offset.Y / this.ViewportSize.Height) * 2));
			this.ForEachPrimitive(q => q.Resize(offsetf));
			this.SpriteRenderer.BufferDirty = true;
		}

		/// <summary>
		/// Sets the colour.
		/// </summary>
		/// <param name="colourDefinition">The colour definition.</param>
		public void SetColour(ColourDefinition colourDefinition)
		{
			this.ForEachPrimitive(q => q.SetVertexColours(colourDefinition));
			this.SpriteRenderer.BufferDirty = true;
		}

		/// <summary>
		/// Sets the texture of the Sprite.
		/// </summary>
		/// <param name="texName">The name of the texture.</param>
		public void SetTexture(string texName)
		{
			this.TextureHandle = !string.IsNullOrEmpty(texName)
								 ? this.Backend.LoadTexture(texName) : RenderManager.TransparentTextureHandle;
		}

		/// <summary>
		/// Sets the UV.
		/// </summary>
		/// <param name="uvs">The uvs.</param>
		public void SetUV(params PointF[] uvs)
		{
			foreach (var prim in this.primitives)
			{
				if (!this.SpriteRenderer.BufferDirty)
				{
					if (!uvs.SequenceEqual(prim.VertexUV))
					{
						prim.SetVertexUVs(uvs);
						this.SpriteRenderer.BufferDirty = true;
					}
				}
				else
				{
					prim.SetVertexUVs(uvs);
				}
			}
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Executes an action for each Primitive.
		/// </summary>
		/// <param name="action">The action to perform.</param>
		protected void ForEachPrimitive(Action<Primitive> action)
		{
			int count = this.PrimitiveCount;
			for (int j = 0; j < count; j++)
			{
				action(this.GetPrimitive(j));
			}
		}

		/// <summary>
		/// Performs a hit test.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <returns><c>true</c> if the primitive is hit; otherwise, <c>false</c>.</returns>
		protected Primitive HitTest(float x, float y)
		{
			for (int i = 0; i < this.primitives.Length; i++)
			{
				if (this.primitives[i].HitTest(x, y))
				{
					return this.primitives[i];
				}
			}

			return null;
		}

		#endregion Protected Methods

		#endregion Methods

		#region Nested Types

		internal sealed class SpriteZOrderComparer : IComparer<Sprite>
		{
			#region Methods

			#region Explicit Interface Methods

			int IComparer<Sprite>.Compare(Sprite x, Sprite y)
			{
				return x.ZOrder.CompareTo(y.ZOrder);
			}

			#endregion Explicit Interface Methods

			#endregion Methods
		}

		#endregion Nested Types
	}
}