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

	using Miyagi.Common;
	using Miyagi.Common.Data;
	using Miyagi.Common.Rendering;

	/// <summary>
	/// A RenderManager.
	/// </summary>
	public sealed class MogreRenderManager : RenderManager
	{
		#region Fields

		private static readonly TextureUnitState.UVWAddressingMode Uvw;

		private readonly MaterialPtr materialPtr;

		private static Texture opaqueTexture;
		private static uint renderManagerCount;
		private static Texture transparentTexture;

		private RenderSystem rs = Root.Singleton.RenderSystem;
		private SceneManager sceneManager;

		#endregion Fields

		#region Constructors

		static MogreRenderManager()
		{
			HorizontalTexelOffset = Root.Singleton.RenderSystem.HorizontalTexelOffset;
			VerticalTexelOffset = Root.Singleton.RenderSystem.VerticalTexelOffset;
			MinimalHardwareBufferSize = 2400;
			Uvw = new TextureUnitState.UVWAddressingMode
			{
				u = TextureUnitState.TextureAddressingMode.TAM_WRAP,
				v = TextureUnitState.TextureAddressingMode.TAM_WRAP,
				w = TextureUnitState.TextureAddressingMode.TAM_WRAP
			};
		}

		/// <summary>
		/// Initializes a new instance of the RenderManager class.
		/// </summary>
		internal MogreRenderManager(MiyagiSystem miyagiSystem)
			: base(miyagiSystem)
		{
			renderManagerCount++;

			// get SceneManager
			using (SceneManagerEnumerator.SceneManagerIterator sci = Root.Singleton.GetSceneManagerIterator())
			{
				sci.MoveNext();
				this.SceneManager = sci.Current;
			}

			// get Camera
			if (this.SceneManager != null)
			{
				using (SceneManager.CameraIterator ci = this.SceneManager.GetCameraIterator())
				{
					ci.MoveNext();
					if (ci.Current != null)
					{
						this.MainViewport = new MogreViewport(ci.Current, miyagiSystem.Backend.Width, miyagiSystem.Backend.Height);
					}
				}
			}

			this.DefaultRenderQueue = (byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY;
			this.MainRenderer = new MogreSpriteRenderer2D(this);
			((IMogreSpriteRenderer)this.MainRenderer).RenderOnRenderQueueEnded = true;

			this.materialPtr = MaterialManager.Singleton.CreateOrRetrieve(
				"Miyagi_Material" + renderManagerCount,
				this.MiyagiSystem.Backend.ResourceGroupName).first;
			this.PreparePass();
		}

		#endregion Constructors

		#region Properties

		#region Public Static Properties

		/// <summary>
		/// Gets or sets the minimal hardware buffer size.
		/// </summary>
		/// <value>The minimal hardware buffer size. Default is 240.</value>
		public static int MinimalHardwareBufferSize
		{
			get;
			set;
		}

		#endregion Public Static Properties

		#region Public Properties

		/// <summary>
		/// Gets or sets the RenderQueueID of the RenderQueue in which the rendering should happen (default is RenderQueueGroupID.RENDER_QUEUE_OVERLAY).
		/// </summary>
		/// <value>The RenderQueueID of the RenderQueue.</value>
		public byte DefaultRenderQueue
		{
			get;
			set;
		}

		public Viewport MogreViewport
		{
			get
			{
				return (Viewport)this.MainViewport.Native;
			}
		}

		public Camera MogreCamera
		{
			get
			{
				return (Camera)((MogreViewport)this.MainViewport).Camera;
			}
		}

		/// <summary>
		/// Gets or sets the Mogre SceneManager.
		/// </summary>
		public SceneManager SceneManager
		{
			get
			{
				return this.sceneManager;
			}

			set
			{
				if (this.sceneManager != value)
				{
					if (this.sceneManager != null)
					{
						try
						{
							this.sceneManager.RenderQueueStarted -= this.RenderQueueStarted;
							this.sceneManager.RenderQueueEnded -= this.RenderQueueEnded;
						}
						catch (NullReferenceException)
						{
						}
					}

					if (value != null)
					{
						value.RenderQueueStarted += this.RenderQueueStarted;
						value.RenderQueueEnded += this.RenderQueueEnded;
					}

					this.sceneManager = value;
				}
			}
		}

		#endregion Public Properties

		#region Internal Properties

		internal Pass Pass
		{
			get;
			private set;
		}

		#endregion Internal Properties

		#endregion Properties

		#region Methods

		#region Public Methods

		public override void DoRender()
		{
			this.PrepareRenderSystem();

			foreach (var renderer in this.SpriteRenderers)
			{
				renderer.RenderSprites();
			}

			this.MainRenderer.RenderSprites();
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Creates a Renderer.
		/// </summary>
		/// <returns>The newly created Renderer.</returns>
		protected override ISpriteRenderer Create2DRendererCore()
		{
			return new MogreSpriteRenderer2D(this);
		}

		protected override ISpriteRenderer3D Create3DRendererCore()
		{
			return new MogreSpriteRenderer3D(this);
		}

		protected override object CreateOpaqueTexture(string opaqueTextureName)
		{
			if (!TextureManager.Singleton.ResourceExists(opaqueTextureName))
			{
				opaqueTexture = TextureManager.Singleton.CreateManual(
					opaqueTextureName,
					this.MiyagiSystem.Backend.ResourceGroupName,
					TextureType.TEX_TYPE_2D,
					32,
					32,
					0,
					PixelFormat.PF_A8R8G8B8,
					(int)TextureUsage.TU_DEFAULT);

				MogreBackend.ClearTexture(opaqueTexture, Colours.White);
			}

			return opaqueTexture;
		}

		protected override object CreateTransparentTexture(string transparentTextureName)
		{
			if (!TextureManager.Singleton.ResourceExists(transparentTextureName))
			{
				transparentTexture = TextureManager.Singleton.CreateManual(
					transparentTextureName,
					this.MiyagiSystem.Backend.ResourceGroupName,
					TextureType.TEX_TYPE_2D,
					32,
					32,
					0,
					PixelFormat.PF_A8R8G8B8,
					(int)TextureUsage.TU_DEFAULT);

				MogreBackend.ClearTexture(transparentTexture, Colours.Transparent);
			}

			return transparentTexture;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				this.rs._setTextureUnitFiltering(0, FilterOptions.FO_NONE, FilterOptions.FO_NONE, FilterOptions.FO_NONE);

				this.SceneManager = null;
				this.MainViewport = null;
				this.rs = null;

				this.materialPtr.Dispose();
				MaterialManager.Singleton.Remove(this.materialPtr.Handle);
			}

			renderManagerCount--;
			if (renderManagerCount == 0)
			{
				//transparentTexture.Unload();
				//TextureManager.Singleton.Remove(transparentTexture.Handle);
				transparentTexture.Dispose();

				//opaqueTexture.Unload();
				//TextureManager.Singleton.Remove(opaqueTexture.Handle);
				opaqueTexture.Dispose();
			}
		}

		#endregion Protected Methods

		#region Private Methods

		private void FireMainRenderer(byte renderQueue, bool queueEnded)
		{
			this.PrepareRenderSystem();
			this.FireSpriteRenderer((MogreSpriteRenderer2D)this.MainRenderer, renderQueue, queueEnded);
		}

		private void FireSpriteRenderer(IMogreSpriteRenderer renderer, byte renderQueue, bool queueEnded)
		{
			if (renderer.RenderQueue == renderQueue
				&& renderer.RenderOnRenderQueueEnded == queueEnded
				&& this.sceneManager.CurrentViewport.Equals(renderer.Viewport.Native))
			{
				renderer.RenderSprites();
			}
		}

		private void FireSpriteRenderers(byte renderQueue, bool queueEnded)
		{
			this.PrepareRenderSystem();
			int count = this.SpriteRenderers.Count;

			for (int i = 0; i < count; i++)
			{
				this.FireSpriteRenderer((MogreSpriteRenderer2D)this.SpriteRenderers[i], renderQueue, queueEnded);
			}
		}

		private void PreparePass()
		{
			Technique technique = this.materialPtr.CreateTechnique();///UNDONE setShadowCasterMaterial
			this.Pass = technique.CreatePass();
			this.Pass.CullingMode = CullingMode.CULL_NONE;
			this.Pass.DepthCheckEnabled = false;
			this.Pass.DepthWriteEnabled = false;
			this.Pass.LightingEnabled = false;

			this.Pass.SetSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);

			TextureUnitState tus = this.Pass.CreateTextureUnitState();
			tus.SetTextureAddressingMode(TextureUnitState.TextureAddressingMode.TAM_WRAP);
		}

		private void PrepareRenderSystem()
		{
			this.rs.UnbindGpuProgram(global::Mogre.GpuProgramType.GPT_FRAGMENT_PROGRAM);
			this.rs.UnbindGpuProgram(global::Mogre.GpuProgramType.GPT_VERTEX_PROGRAM);

			this.SceneManager._setPass(this.Pass, true, false);

			this.rs._setSeparateSceneBlending(
				SceneBlendFactor.SBF_SOURCE_ALPHA,
				SceneBlendFactor.SBF_ONE_MINUS_SOURCE_ALPHA,
				SceneBlendFactor.SBF_ONE,
				SceneBlendFactor.SBF_ONE);

			this.rs._setTextureAddressingMode(0, Uvw);
		}

		private void RenderQueueEnded(byte queueGroupId, string invocation, out bool repeatThisInvocation)
		{
			repeatThisInvocation = false; // shut up compiler
			this.FireSpriteRenderers(queueGroupId, true);
			this.FireMainRenderer(queueGroupId, true);
		}

		private void RenderQueueStarted(byte queueGroupId, string invocation, out bool skipThisInvocation)
		{
			skipThisInvocation = false; // shut up compiler
			this.FireSpriteRenderers(queueGroupId, false);
			this.FireMainRenderer(queueGroupId, false);
		}

		#endregion Private Methods

		#endregion Methods
	}
}