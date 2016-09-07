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
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;

	using global::Mogre;

	using GpuProgramType = global::Mogre.GpuProgramType;

	using Miyagi.Common;
	using Miyagi.Common.Data;
	using Miyagi.Common.Rendering;

	using Vector3 = global::Mogre.Vector3;

	/// <summary>
	/// A sprite renderer.
	/// </summary>
	internal class MogreSpriteRenderer2D : SpriteRenderer, IMogreSpriteRenderer
	{
		#region Fields

		private const int VerticesPerTriangle = 3;

		private static LayerBlendModeEx_NativePtr alphaBlendMode;

		private Guid guid = Guid.NewGuid();
		private HardwareVertexBufferSharedPtr hardwareBuffer;
		private Matrix4 projMatrix;
		private List<Chunk> renderChunks = new List<Chunk>();
		private RenderOperation renderOp;
		private RenderSystem renderSystem = Root.Singleton.RenderSystem;
		private Matrix4 viewMatrix;

		#endregion Fields

		#region Constructors

		static MogreSpriteRenderer2D()
		{
			alphaBlendMode = LayerBlendModeEx_NativePtr.Create();
			alphaBlendMode.alphaArg1 = 0;
			alphaBlendMode.source1 = LayerBlendSource.LBS_TEXTURE;
			alphaBlendMode.source2 = LayerBlendSource.LBS_MANUAL;
			alphaBlendMode.blendType = LayerBlendType.LBT_ALPHA;
			alphaBlendMode.operation = LayerBlendOperationEx.LBX_MODULATE;
		}

		/// <summary>
		/// Initializes a new instance of the SpriteRenderer class.
		/// </summary>
		internal MogreSpriteRenderer2D(MogreRenderManager owner)
			: base(owner)
		{
			this.Viewport = owner.MainViewport;
			this.MogreRenderManager = owner;
			this.RenderQueue = owner.DefaultRenderQueue;
		}

		#endregion Constructors

		#region Properties

		#region Public Properties

		/// <summary>
		/// Gets the hardware buffer capacity.
		/// </summary>
		/// <value>An int representing the hardware buffer capacity.</value>
		public override int HardwareBufferCapacity
		{
			get
			{
				if (this.hardwareBuffer != null)
				{
					return (int)this.hardwareBuffer.NumVertices;
				}

				return 0;
			}
		}

		public bool RenderOnRenderQueueEnded
		{
			get;
			set;
		}

		public byte RenderQueue
		{
			get;
			set;
		}

		#endregion Public Properties

		#region Protected Properties

		protected MogreRenderManager MogreRenderManager
		{
			get;
			private set;
		}

		#endregion Protected Properties

		#endregion Properties

		#region Methods

		#region Public Methods

		/// <summary>
		/// Adds a Sprite to the Renderer.
		/// </summary>
		/// <param name="sprite">The sprites to add.</param>
		public override bool AddSprite(Sprite sprite)
		{
			if (base.AddSprite(sprite))
			{
				this.CheckBufferSize();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Renders the sprites.
		/// </summary>
		public override sealed void RenderSprites()
		{
			if (this.SpriteList.Count == 0)
			{
				return;
			}

			this.PrepareMatrices();

			Debug.Assert(this.hardwareBuffer.NumVertices >= this.TriangleCount * VerticesPerTriangle, "Assert hardwarebuffer size");

			if (this.SpriteOrderDirty)
			{
				this.SortSprites();
			}

			if (this.BufferDirty)
			{
				// write quads to the hardware buffer, and remember chunks
				this.renderChunks = this.GetRenderChunks();
				if (this.CacheToTexture)
				{
					this.CacheSpritesToTexture(this.guid.ToString());
				}
			}

			this.DoRender();
			this.RestoreMatrices();
		}

		#endregion Public Methods

		#region Protected Methods

		protected virtual MogreVertex CreateVertex(Vertex vertex)
		{
			uint val32;
			this.renderSystem.ConvertColourValue(
				new ColourValue(vertex.Colour.Red / 255f, vertex.Colour.Green / 255f, vertex.Colour.Blue / 255f, vertex.Colour.Alpha / 255f),
				out val32);

			MogreVertex v;
			v.Location = new Vector3(vertex.Location.X, vertex.Location.Y, 0);
			v.UV = new Vector2(vertex.UV.X, vertex.UV.Y);
			v.Colour = val32;
			return v;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				this.MogreRenderManager = null;
				this.renderSystem = null;

				if (this.hardwareBuffer != null)
				{
					this.DestroyHardwareBuffer();
				}

				// remove cache texture
				if (!TextureManager.Singleton.ResourceExists(this.guid.ToString()))
				{
					TextureManager.Singleton.Remove(this.guid.ToString());
				}
			}
		}

		protected override sealed void OnCacheToTextureChanged()
		{
			if (this.CacheToTexture)
			{
				this.CreateCacheTexture();
			}
			else
			{
				if (!TextureManager.Singleton.ResourceExists(this.guid.ToString()))
				{
					TextureManager.Singleton.Remove(this.guid.ToString());
				}
			}
		}

		protected override sealed void OnViewportChanged()
		{
			if (!TextureManager.Singleton.ResourceExists(this.guid.ToString()))
			{
				TextureManager.Singleton.Remove(this.guid.ToString());
			}

			if (this.CacheToTexture)
			{
				this.CreateCacheTexture();
			}
		}

		protected virtual void PrepareMatrices()
		{
			Camera cam = this.MogreRenderManager.MogreCamera;
			this.viewMatrix = cam.ViewMatrix;
			this.projMatrix = cam.ProjectionMatrixRS;
			this.renderSystem._setWorldMatrix(Matrix4.IDENTITY);
			this.renderSystem._setViewMatrix(Matrix4.IDENTITY);
			this.renderSystem._setProjectionMatrix(Matrix4.IDENTITY);
		}

		protected virtual void RestoreMatrices()
		{
			this.renderSystem._setViewMatrix(this.viewMatrix);
			this.renderSystem._setProjectionMatrix(this.projMatrix);
		}

		#endregion Protected Methods

		#region Private Static Methods

		private static RectangleF GetScreenRect(Size screenResolution, bool flip)
		{
			float x1 = RenderManager.HorizontalTexelOffset;
			float x2 = screenResolution.Width + RenderManager.HorizontalTexelOffset;
			float y1 = RenderManager.VerticalTexelOffset;
			float y2 = screenResolution.Height + RenderManager.VerticalTexelOffset;

			x1 = ((x1 / screenResolution.Width) * 2) - 1;
			x2 = ((x2 / screenResolution.Width) * 2) - 1;
			y1 = -(((y1 / screenResolution.Height) * 2) - 1);
			y2 = -(((y2 / screenResolution.Height) * 2) - 1);

			return flip ? RectangleF.FromLTRB(x1, y2, x2, y1) : RectangleF.FromLTRB(x1, y1, x2, y2);
		}

		private static uint NextPowerOfTwo(uint self)
		{
			return (uint)System.Math.Pow(2, System.Math.Ceiling(System.Math.Log(self, 2)));
		}

		#endregion Private Static Methods

		#region Private Methods

		private void BindGpuProgramParameters(GpuProgramType type, Miyagi.Common.Resources.GpuProgram gpuProg)
		{
			if (gpuProg.NamedConstants.Count > 0)
			{
				using (GpuProgramParametersSharedPtr gpuParas = type == GpuProgramType.GPT_FRAGMENT_PROGRAM
																	? this.MogreRenderManager.Pass.GetFragmentProgramParameters()
																	: this.MogreRenderManager.Pass.GetVertexProgramParameters())
				{
					foreach (KeyValuePair<string, object> kvp in gpuProg.NamedConstants)
					{
						object o = kvp.Value;

						if (o is ColourValue)
						{
							gpuParas.SetNamedConstant(kvp.Key, (ColourValue)o);
						}
						else if (o is int)
						{
							gpuParas.SetNamedConstant(kvp.Key, (int)o);
						}
						else if (o is float)
						{
							gpuParas.SetNamedConstant(kvp.Key, (float)o);
						}
						else if (o is Matrix4)
						{
							gpuParas.SetNamedConstant(kvp.Key, (Matrix4)o);
						}
						else if (o is Vector3)
						{
							gpuParas.SetNamedConstant(kvp.Key, (Vector3)o);
						}
						else if (o is Vector4)
						{
							gpuParas.SetNamedConstant(kvp.Key, (Vector4)o);
						}
					}

					this.renderSystem.BindGpuProgramParameters(type, gpuParas, (ushort)GpuParamVariability.GPV_ALL);
				}
			}
		}

		private void CacheSpritesToTexture(string texName)
		{
			using (var tex = TextureManager.Singleton.GetByName(texName))
			{
				using (var buf = tex.GetBuffer())
				{
					using (var renderTexture = buf.GetRenderTarget())
					{
						this.renderSystem._setRenderTarget(renderTexture);
						this.renderSystem.ClearFrameBuffer((uint)FrameBufferType.FBT_COLOUR, new ColourValue(0, 0, 0, 0));
						this.DoRender();

						this.renderChunks = new List<Chunk>
							{
								new Chunk
								{
									Opacity = 1f,
									TexFilter = TextureFiltering.Point,
									TexHandle = tex,
									VertexCount = 6
								}
							};

						unsafe
						{
							var buffer = (MogreVertex*)this.hardwareBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
							var quad = new Quad(
								Colours.White,
								GetScreenRect(this.Viewport.Size, renderTexture.RequiresTextureFlipping()),
								new RectangleF(0, 0, 1, 1));
							this.DrawQuad(quad, ref buffer);
							this.hardwareBuffer.Unlock();
						}

						this.renderSystem._setRenderTarget(((Viewport)this.Viewport.Native).Target);
					}
				}
			}
		}

		private void CheckBufferSize()
		{
			uint size = (uint)this.TriangleCount * VerticesPerTriangle;
			uint minSize = (uint)MogreRenderManager.MinimalHardwareBufferSize;
			uint newSize = size < minSize ? minSize : size;

			// grow hardware buffer if needed
			if (this.hardwareBuffer == null || this.hardwareBuffer.NumVertices < newSize)
			{
				if (this.hardwareBuffer != null)
				{
					this.DestroyHardwareBuffer();
				}

				this.CreateHardwareBuffer(newSize);
			}
		}

		private void CreateCacheTexture()
		{
			string texName = this.guid.ToString();
			if (!TextureManager.Singleton.ResourceExists(texName))
			{
				var width = (uint)System.Math.Max(this.Viewport.Size.Width, 0);
				var height = (uint)System.Math.Max(this.Viewport.Size.Height, 0);

				if (!this.renderSystem.Capabilities.HasCapability(Capabilities.RSC_NON_POWER_OF_2_TEXTURES))
				{
					width = NextPowerOfTwo(width);
					height = NextPowerOfTwo(height);
				}

				TexturePtr tex = TextureManager.Singleton.CreateManual(
					texName,
					this.RenderManager.MiyagiSystem.Backend.ResourceGroupName,
					TextureType.TEX_TYPE_2D,
					width,
					height,
					0,
					PixelFormat.PF_A8R8G8B8,
					(int)TextureUsage.TU_RENDERTARGET);

				using (var buf = tex.GetBuffer())
				{
					using (var renderTexture = buf.GetRenderTarget())
					{
						//renderTexture.IsAutoUpdated = false;
					}
				}

				tex.Dispose();
			}
		}

		private void CreateHardwareBuffer(uint size)
		{
			this.renderOp = new RenderOperation
			{
				vertexData = new VertexData
				{
					vertexStart = 0
				}
			};

			VertexDeclaration vd = this.renderOp.vertexData.vertexDeclaration;
			vd.AddElement(
				0,
				0,
				VertexElementType.VET_FLOAT3,
				VertexElementSemantic.VES_POSITION);

			vd.AddElement(
				0,
				VertexElement.GetTypeSize(VertexElementType.VET_FLOAT3),
				VertexElementType.VET_FLOAT2,
				VertexElementSemantic.VES_TEXTURE_COORDINATES);

			vd.AddElement(
				0,
				VertexElement.GetTypeSize(VertexElementType.VET_FLOAT3) + VertexElement.GetTypeSize(VertexElementType.VET_FLOAT2),
				VertexElementType.VET_COLOUR,
				VertexElementSemantic.VES_DIFFUSE);

			this.hardwareBuffer = HardwareBufferManager.Singleton.CreateVertexBuffer(
				vd.GetVertexSize(0),
				size,
				HardwareBuffer.Usage.HBU_DYNAMIC_WRITE_ONLY,
				false);

			this.renderOp.vertexData.vertexBufferBinding.SetBinding(0, this.hardwareBuffer);

			this.renderOp.operationType = RenderOperation.OperationTypes.OT_TRIANGLE_LIST;
			this.renderOp.useIndexes = false;
		}

		private void DestroyHardwareBuffer()
		{
			this.renderOp.vertexData.Dispose();
			this.renderOp.Dispose();
			this.renderOp = null;

			this.hardwareBuffer.Dispose();
			this.hardwareBuffer = null;
		}

		private void DoRender()
		{
			this.renderOp.vertexData.vertexStart = 0;

			int count = this.renderChunks.Count;

			for (int j = 0; j < count; j++)
			{
				Chunk currChunk = this.renderChunks[j];
				this.renderOp.vertexData.vertexCount = currChunk.VertexCount;

				var gpuPrograms = currChunk.GpuPrograms;
				if (gpuPrograms != null && gpuPrograms.Count > 0)
				{
					this.SetGpuPrograms(currChunk);
				}

				this.SetTexture(0, currChunk.TexHandle, currChunk.TexFilter, currChunk.Opacity);

				this.renderSystem._render(this.renderOp);
				this.renderOp.vertexData.vertexStart += currChunk.VertexCount;

				if (gpuPrograms != null && gpuPrograms.Count > 0)
				{
					this.RemoveGpuPrograms();
				}
			}
		}

		private unsafe void DrawQuad(Primitive primitive, ref MogreVertex* buffer)
		{
			var v0 = this.CreateVertex(primitive.GetVertex(0));
			var v1 = this.CreateVertex(primitive.GetVertex(1));
			var v2 = this.CreateVertex(primitive.GetVertex(2));
			var v3 = this.CreateVertex(primitive.GetVertex(3));

			*buffer++ = v0;
			*buffer++ = v1;
			*buffer++ = v2;
			*buffer++ = v0;
			*buffer++ = v3;
			*buffer++ = v2;
		}

		private unsafe void DrawTriangle(Primitive primitive, ref MogreVertex* buffer)
		{
			var v0 = this.CreateVertex(primitive.GetVertex(0));
			var v1 = this.CreateVertex(primitive.GetVertex(1));
			var v2 = this.CreateVertex(primitive.GetVertex(2));

			*buffer++ = v0;
			*buffer++ = v1;
			*buffer++ = v2;
		}

		private List<Chunk> GetRenderChunks()
		{
			var chunk = new Chunk();
			var retValue = new List<Chunk>();

			unsafe
			{
				var buffer = (MogreVertex*)this.hardwareBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);

				Sprite currentSprite;
				int spriteCount = this.SpriteList.Count;

				for (int batchIndex = 0; batchIndex < spriteCount; batchIndex++)
				{
					currentSprite = this.SpriteList[batchIndex];

					// ignore transparent sprites
					if (currentSprite.Visible
						&& currentSprite.TextureHandle != Common.Rendering.RenderManager.TransparentTextureHandle)
					{
						chunk.Opacity = currentSprite.Opacity;
						chunk.TexFilter = currentSprite.TexFilter;
						chunk.TexHandle = (TexturePtr)currentSprite.TextureHandle;
						chunk.GpuPrograms = currentSprite.GpuPrograms;

						int count = currentSprite.PrimitiveCount;
						for (int index = 0; index < count; index++)
						{
							var primitive = currentSprite.GetPrimitive(index);
							if (!primitive.SkipRender)
							{
								chunk.VertexCount += (uint)primitive.TriangleCount * VerticesPerTriangle;

								switch (primitive.Type)
								{
									case PrimitiveType.Triangle:
										this.DrawTriangle(primitive, ref buffer);
										break;
									case PrimitiveType.Quad:
										this.DrawQuad(primitive, ref buffer);
										break;
									case PrimitiveType.Custom:
										break;
									default:
										throw new ArgumentOutOfRangeException();
								}
							}
						}

						if (chunk.VertexCount > 0)
						{
							// check if we can include the next one in the batch
							bool skip = false;
							int nextBatchIndex = batchIndex + 1;
							if (nextBatchIndex < spriteCount)
							{
								var nextSprite = this.SpriteList[nextBatchIndex];
								skip = nextSprite.TextureHandle == currentSprite.TextureHandle
									   && nextSprite.Opacity == currentSprite.Opacity
									   && nextSprite.Visible
									   && nextSprite.TexFilter == currentSprite.TexFilter
									   && (nextSprite.GpuPrograms == null || nextSprite.GpuPrograms.Count == 0);
							}

							if (!skip)
							{
								retValue.Add(chunk);
								chunk.VertexCount = 0;
							}
						}
					}
				}

				this.hardwareBuffer.Unlock();
			}

			this.BufferDirty = false;
			return retValue;
		}

		private void RemoveGpuPrograms()
		{
			this.MogreRenderManager.Pass.SetFragmentProgram(string.Empty);
			this.MogreRenderManager.Pass.SetVertexProgram(string.Empty);
			this.MogreRenderManager.SceneManager._setPass(this.MogreRenderManager.Pass, true, false);
		}

		private void SetGpuPrograms(Chunk chunk)
		{
			foreach (var gpuProg in chunk.GpuPrograms)
			{
				switch (gpuProg.Type)
				{
					case Miyagi.Common.GpuProgramType.Fragment:
						this.MogreRenderManager.Pass.SetFragmentProgram(gpuProg.Name);
						this.BindGpuProgramParameters(GpuProgramType.GPT_FRAGMENT_PROGRAM, gpuProg);
						break;

					case Miyagi.Common.GpuProgramType.Vertex:
						this.MogreRenderManager.Pass.SetVertexProgram(gpuProg.Name);
						this.BindGpuProgramParameters(GpuProgramType.GPT_VERTEX_PROGRAM, gpuProg);
						break;
				}
			}

			this.MogreRenderManager.SceneManager._setPass(this.MogreRenderManager.Pass, true, false);

			foreach (var gpuProg in chunk.GpuPrograms)
			{
				if (gpuProg.Type == Miyagi.Common.GpuProgramType.Fragment && gpuProg.AdditonalTextures.Count > 0)
				{
					for (int i = 0; i < gpuProg.AdditonalTextures.Count; i++)
					{
						this.SetTexture((uint)i + 1, gpuProg.AdditonalTextures[i], chunk.TexFilter, chunk.Opacity);
					}
				}
			}
		}

		private void SetTexture(uint index, string name, TextureFiltering texFilter, float alpha)
		{
			this.SetTexture(index, (uint)this.RenderManager.MiyagiSystem.Backend.LoadTexture(name), texFilter, alpha);
		}

		private void SetTexture(uint index, uint handle, TextureFiltering texFilter, float alpha)
		{
			using (TexturePtr tp = TextureManager.Singleton.GetByHandle(handle))
			{
				this.SetTexture(index, tp, texFilter, alpha);
			}
		}

		private void SetTexture(uint index, TexturePtr tp, TextureFiltering texFilter, float alpha)
		{
			this.renderSystem._setTexture(index, true, tp);
			this.SetTextureFiltering(index, texFilter);
			this.SetTextureAlpha(index, alpha);
		}

		private void SetTextureAlpha(uint index, float alpha)
		{
			alphaBlendMode.alphaArg2 = alpha;
			alphaBlendMode.factor = alpha;
			this.renderSystem._setTextureBlendMode(index, alphaBlendMode);
		}

		/// <exception cref="InvalidEnumArgumentException">texFilter</exception>
		private void SetTextureFiltering(uint index, TextureFiltering texFilter)
		{
			switch (texFilter)
			{
				case TextureFiltering.None:
					this.renderSystem._setTextureUnitFiltering(index, FilterOptions.FO_NONE, FilterOptions.FO_NONE, FilterOptions.FO_NONE);
					break;
				case TextureFiltering.Linear:
					this.renderSystem._setTextureUnitFiltering(index, FilterOptions.FO_LINEAR, FilterOptions.FO_LINEAR, FilterOptions.FO_NONE);
					break;
				case TextureFiltering.Point:
					this.renderSystem._setTextureUnitFiltering(index, FilterOptions.FO_POINT, FilterOptions.FO_POINT, FilterOptions.FO_NONE);
					break;
				case TextureFiltering.Anisotropic:
					this.renderSystem._setTextureUnitFiltering(index, FilterOptions.FO_ANISOTROPIC, FilterOptions.FO_ANISOTROPIC, FilterOptions.FO_NONE);
					break;
				default:
					throw new InvalidEnumArgumentException("texFilter", (int)texFilter, typeof(TextureFiltering));
			}
		}

		#endregion Private Methods

		#endregion Methods
	}
}