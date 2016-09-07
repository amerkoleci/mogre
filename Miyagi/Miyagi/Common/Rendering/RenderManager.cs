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

	using Miyagi.Common.Serialization;

	/// <summary>
	/// The base class for render manager.
	/// </summary>
	public abstract class RenderManager : IManager
	{
		#region Fields

		/// <summary>
		/// Gets the name of the opaque texture.
		/// </summary>
		internal const string OpaqueTexture = "Miyagi_Opaque";

		/// <summary>
		/// Gets the name of the transparent texture.
		/// </summary>
		internal const string TransparentTexture = "Miyagi_Transparent";

		private static int zorderCount;

		private IViewport mainViewport;
		private List<ISpriteRenderer> spriteRenderers;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the RenderManager class.
		/// </summary>
		/// <param name="miyagiSystem">The MiyagiSystem.</param>
		protected RenderManager(MiyagiSystem miyagiSystem)
		{
			this.MiyagiSystem = miyagiSystem;
			this.spriteRenderers = new List<ISpriteRenderer>();
			this.CreateBlankTextures();
		}

		/// <summary>
		/// Finalizes an instance of the RenderManager class.
		/// </summary>
		~RenderManager()
		{
			this.Dispose(false);
		}

		#endregion Constructors

		#region Events

		/// <summary>
		/// Occurs when the manager is disposing.
		/// </summary>
		public event EventHandler Disposing;

		#endregion Events

		#region Properties

		#region Public Static Properties

		/// <summary>
		/// Gets or sets the horizontal texel offset.
		/// </summary>
		public static float HorizontalTexelOffset
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets handle of the solid texture.
		/// </summary>
		public static object OpaqueTextureHandle
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the handle of the transparent texture.
		/// </summary>
		public static object TransparentTextureHandle
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets the vertical texel offset.
		/// </summary>
		/// <remarks>This should not be changed by user code.</remarks>
		public static float VerticalTexelOffset
		{
			get;
			protected set;
		}

		#endregion Public Static Properties

		#region Public Properties

		/// <summary>
		/// Gets the hardware buffer capacity of all SpriteRenderers.
		/// </summary>
		public int HardwareBufferCapacity
		{
			get
			{
				return this.MainRenderer.HardwareBufferCapacity
					   + this.SpriteRenderers.Sum(rend => rend.HardwareBufferCapacity);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the manager has been disposed.
		/// </summary>
		/// <value></value>
		public bool IsDisposed
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the main sprite renderer.
		/// </summary>
		public SpriteRenderer MainRenderer
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets the main viewport.
		/// </summary>
		/// <remarks>This viewport is used for the cursor, dialogboxes and tooltips, and it is the default for new SpriteRenderers.</remarks>
		public IViewport MainViewport
		{
			get
			{
				return this.mainViewport;
			}

			set
			{
				if (this.mainViewport != value)
				{
					this.mainViewport = value;
					if (this.MainRenderer != null)
					{
						this.MainRenderer.Viewport = value;
					}
				}
			}
		}

		/// <summary>
		/// Gets the MiyagiSystem.
		/// </summary>
		public MiyagiSystem MiyagiSystem
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the type of the manager.
		/// </summary>
		public string Type
		{
			get
			{
				return "Render";
			}
		}

		#endregion Public Properties

		#region Protected Properties

		/// <summary>
		/// Gets a list of sprite renderers.
		/// </summary>
		protected IList<ISpriteRenderer> SpriteRenderers
		{
			get
			{
				return this.spriteRenderers;
			}
		}

		#endregion Protected Properties

		#endregion Properties

		#region Methods

		#region Explicit Interface Methods

		void IManager.LoadSerializationData(SerializationData data)
		{
		}

		void IManager.NotifyManagerRegistered(IManager manager)
		{
		}

		void IManager.SaveSerializationData(SerializationData data)
		{
		}

		#endregion Explicit Interface Methods

		#region Public Methods

		/// <summary>
		/// Creates a renderer.
		/// </summary>
		/// <returns>The newly created renderer.</returns>
		public ISpriteRenderer Create2DRenderer()
		{
			var retValue = this.Create2DRendererCore();
			retValue.ZOrder = zorderCount++;

			this.SpriteRenderers.Add(retValue);
			this.SortRenderers();

			return retValue;
		}

		/// <summary>
		/// Creates a renderer.
		/// </summary>
		/// <returns>The newly created renderer.</returns>
		public ISpriteRenderer3D Create3DRenderer()
		{
			var retValue = this.Create3DRendererCore();
			retValue.ZOrder = zorderCount++;

			this.SpriteRenderers.Add(retValue);
			this.SortRenderers();

			return retValue;
		}

		/// <summary>
		/// Removes and disposes the specified renderer.
		/// </summary>
		/// <param name="renderer">The renderer to remove.</param>
		public void DestroyRenderer(ISpriteRenderer renderer)
		{
			renderer.Dispose();
			this.SpriteRenderers.Remove(renderer);
		}

		/// <summary>
		/// Disposes the render manager.
		/// </summary>
		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			if (this.Disposing != null)
			{
				this.Disposing(this, EventArgs.Empty);
			}

			this.Dispose(true);
			GC.SuppressFinalize(this);
			this.IsDisposed = true;
			this.MiyagiSystem.UnregisterManager(this);
			this.MiyagiSystem = null;
		}

		/// <summary>
		/// Performs a rendering operation.
		/// </summary>
		public abstract void DoRender();

		/// <summary>
		/// Initializes the manager.
		/// </summary>
		public virtual void Initialize()
		{
		}

		/// <summary>
		/// Sorts the renderers.
		/// </summary>
		public void SortRenderers()
		{
			this.spriteRenderers.Sort(new SpriteRendererZOrderComparer());
		}

		/// <summary>
		/// Updates the manager.
		/// </summary>
		public virtual void Update()
		{
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Creates a 2D renderer.
		/// </summary>
		/// <returns>The newly created renderer.</returns>
		protected abstract ISpriteRenderer Create2DRendererCore();

		/// <summary>
		/// Creates a 3D renderer.
		/// </summary>
		/// <returns>The newly created renderer.</returns>
		protected abstract ISpriteRenderer3D Create3DRendererCore();

		/// <summary>
		/// Creates the opaque texture.
		/// </summary>
		/// <param name="opaqueTextureName">Name of the opaque texture.</param>
		/// <returns>The handle of the opaque texture.</returns>
		protected abstract object CreateOpaqueTexture(string opaqueTextureName);

		/// <summary>
		/// Creates the transparent texture.
		/// </summary>
		/// <param name="transparentTextureName">Name of the transparent texture.</param>
		/// <returns>The handle of the opaque texture.</returns>
		protected abstract object CreateTransparentTexture(string transparentTextureName);

		/// <summary>
		/// Disposes the render manager.
		/// </summary>
		/// <param name="disposing">Indicates whether Dispose has been called.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.MainRenderer.Dispose();
				this.spriteRenderers.ForEach(rend => rend.Dispose());
				this.spriteRenderers.Clear();
				this.spriteRenderers = null;
			}

			this.Disposing = null;
		}

		#endregion Protected Methods

		#region Private Methods

		private void CreateBlankTextures()
		{
			TransparentTextureHandle = this.CreateTransparentTexture(TransparentTexture);
			OpaqueTextureHandle = this.CreateOpaqueTexture(OpaqueTexture);
		}

		#endregion Private Methods

		#endregion Methods

		#region Nested Types

		private sealed class SpriteRendererZOrderComparer : IComparer<ISpriteRenderer>
		{
			#region Methods

			#region Explicit Interface Methods

			int IComparer<ISpriteRenderer>.Compare(ISpriteRenderer x, ISpriteRenderer y)
			{
				return x.ZOrder.CompareTo(y.ZOrder);
			}

			#endregion Explicit Interface Methods

			#endregion Methods
		}

		#endregion Nested Types
	}
}