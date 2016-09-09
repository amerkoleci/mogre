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
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.InteropServices;

	using global::Mogre;

	using GpuProgramType = global::Mogre.GpuProgramType;

	using Miyagi.Common;
	using Miyagi.Common.Data;
	using Miyagi.Common.Rendering;

	public class MogreBackend : Backend
	{
		MogreRenderManager _renderManager;

		#region Constructors

		public MogreBackend(MiyagiSystem system, int width, int height)
			: base(system, width, height)
		{

		}


		#endregion Constructors

		#region Properties

		#region Public Properties

		public override RenderManager RenderManager
		{
			get
			{
				return _renderManager ?? (_renderManager = new MogreRenderManager(this.MiyagiSystem));
			}
		}

		#endregion Public Properties

		#endregion Properties

		#region Methods

		#region Public Methods

		public override void CreateGpuProgram(Miyagi.Common.Resources.GpuProgram gpuProg)
		{
			if (!HighLevelGpuProgramManager.Singleton.ResourceExists(gpuProg.Name))
			{
				using (HighLevelGpuProgramPtr hlgp = HighLevelGpuProgramManager.Singleton.CreateProgram(
					gpuProg.Name,
					this.ResourceGroupName,
					gpuProg.Language,
					gpuProg.Type == Miyagi.Common.GpuProgramType.Fragment
						? GpuProgramType.GPT_FRAGMENT_PROGRAM
						: GpuProgramType.GPT_VERTEX_PROGRAM))
				{
					foreach (KeyValuePair<string, string> kvp in gpuProg.Parameters)
					{
						hlgp.SetParameter(kvp.Key, kvp.Value);
					}

					hlgp.SourceFile = gpuProg.SourceFile;
					hlgp.Load();
				}
			}
		}

		public override void CreateRenderTexture(string name, Size size, Colour backgroundColour, object camera)
		{
			if (!TextureManager.Singleton.ResourceExists(name))
			{
				uint width = (uint)System.Math.Max(size.Width, 0);
				uint height = (uint)System.Math.Max(size.Height, 0);

				Texture tex = TextureManager.Singleton.CreateManual(
					name,
					this.ResourceGroupName,
					TextureType.TEX_TYPE_2D,
					width,
					height,
					0,
					PixelFormat.PF_A8R8G8B8,
					(int)TextureUsage.TU_RENDERTARGET);


				//using (var buf = tex.GetBuffer())
				//{
				//    using (var renderTexture = buf.GetRenderTarget())
				//    {
				//        renderTexture.AddViewport((Camera)camera);
				//        renderTexture.GetViewport(0).SetClearEveryFrame(true);
				//        renderTexture.GetViewport(0).BackgroundColour = new ColourValue(backgroundColour.Red / 255f, backgroundColour.Green / 255f, backgroundColour.Blue / 255f, backgroundColour.Alpha / 255f);
				//    }
				//}
			}
		}

		public override object CreateTexture(string name, Size size)
		{
			Texture texture = TextureManager.Singleton.GetByName(name);
			if (texture != null)
			{
				return texture;
			}

			var width = (uint)System.Math.Max(size.Width, 0);
			var height = (uint)System.Math.Max(size.Height, 0);

			texture = TextureManager.Singleton.CreateManual(
				name,
				ResourceGroupName,
				TextureType.TEX_TYPE_2D,
				width,
				height,
				0,
				PixelFormat.PF_A8R8G8B8);
			ClearTexture(texture, Colours.Transparent);
			return texture;
		}

		public override float GetTextureAlpha(object textureHandle, Point p)
		{
			Texture texture = SafeResolveTexture(textureHandle);
			if (p.X > texture.Width || p.Y > texture.Height)
			{
				return 0;
			}

			using (var buffer = texture.GetBuffer())
			{
				var pb = buffer.Lock(new Box(p.X, p.Y, p.X + 1, p.Y + 1), HardwareBuffer.LockOptions.HBL_DISCARD);
				unsafe
				{
					ColourValue cv;
					ColourValue* cvptr = &cv;
					PixelUtil.UnpackColour(cvptr, PixelFormat.PF_BYTE_RGBA, pb.data.ToPointer());
					buffer.Unlock();
					return cv.A;
				}

			}
		}

		public override Size GetTextureSize(object textureHandle)
		{
			Texture texture = SafeResolveTexture(textureHandle);
			return new Size((int)texture.Width, (int)texture.Height);
		}

		public override object LoadTexture(string name)
		{
			var texture = TextureManager.Singleton.GetByName(name);
			if (texture == null)
			{
				texture = TextureManager.Singleton.Load(name, this.ResourceGroupName);
			}

			return texture;
		}

		public override bool MessagePump()
		{
			var rt = ((Viewport)_renderManager.MainViewport.Native).Target as RenderWindow;

			// check if primary window has been closed
			if (rt == null || rt.IsClosed)
			{
				return false;
			}

			WindowEventUtilities.MessagePump();
			Root.Singleton.RenderOneFrame();

			return true;
		}

		public override void RemoveTexture(string name)
		{
			if (TextureManager.Singleton.ResourceExists(name))
			{
				using (Texture texturePtr = TextureManager.Singleton.GetByName(name))
				{
					// handle render textures
					if (texturePtr.Usage == (int)TextureUsage.TU_RENDERTARGET)
					{
						using (HardwarePixelBufferSharedPtr buf = texturePtr.GetBuffer())
						{
							using (RenderTexture renderTexture = buf.GetRenderTarget())
							{
								renderTexture.RemoveAllViewports();
							}
						}
					}

					texturePtr.Unload();
					TextureManager.Singleton.Remove(name);
				}
			}
		}

		public override void SetContext(object args)
		{
		}

		public override bool TextureExists(string name)
		{
			return TextureManager.Singleton.ResourceExists(name);
		}

		public override unsafe void WriteToTexture(byte[] bytes, object textureHandle)
		{
			// draw bitmap to texture
			Texture texture = SafeResolveTexture(textureHandle);
			using (HardwarePixelBufferSharedPtr texBuffer = texture.GetBuffer())
			{
				texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
				PixelBox pb = texBuffer.CurrentLock;

				Debug.Assert(texture.Width == texture.SrcWidth && texture.Height == texture.SrcHeight, "Texture dimensions altered by render system.");
				Marshal.Copy(bytes, 0, pb.data, bytes.Length);

				texBuffer.Unlock();
			}
		}

		#endregion Public Methods

		internal Texture SafeResolveTexture(object textureHandle)
		{
			var textureName = textureHandle as string;
			if (textureName != null)
			{
				return (Texture)LoadTexture(textureName);
			}

			return (Texture)textureHandle;
		}

		internal static unsafe void ClearTexture(Texture texture, Colour colour)
		{
			using (HardwarePixelBufferSharedPtr hbuf = texture.GetBuffer())
			{
				hbuf.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
				PixelBox pb = hbuf.CurrentLock;

				byte* destData = (byte*)pb.data;
				uint destPixelSize = PixelUtil.GetNumElemBytes(pb.format);
				uint destRowPitchBytes = pb.rowPitch * destPixelSize;

				ColourValue cv = new ColourValue(colour.Red / 255f, colour.Green / 255f, colour.Blue / 255f, colour.Alpha / 255f);
				for (uint i = 0; i < texture.Height; i++)
				{
					for (uint j = 0; j < texture.Width; j++)
					{
						uint offset = (i * destRowPitchBytes) + (j * destPixelSize);
						PixelUtil.PackColour(cv, pb.format, &destData[offset]);
					}
				}

				hbuf.Unlock();
			}
		}

		#endregion Methods
	}
}