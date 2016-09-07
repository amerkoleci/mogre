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
	using Miyagi.Common.Resources;

	/// <summary>
	/// The base class for rendering back-ends.
	/// </summary>
	public abstract class Backend
	{
		#region Constructors

		static Backend()
		{
			DoNormalizeFilePath = System.IO.Path.GetFileName;
		}

		/// <summary>
		/// Initializes a new instance of the Backend class.
		/// </summary>
		/// <param name="system">The MiyagiSystem.</param>
		protected Backend(MiyagiSystem system, int width, int height)
		{
			this.MiyagiSystem = system;
			this.ResourceGroupName = "General";
			Width = width;
			Height = height;
		}

		#endregion Constructors

		#region Properties

		#region Public Properties

		/// <summary>
		/// Gets the render manager of the back-end.
		/// </summary>
		public abstract RenderManager RenderManager
		{
			get;
		}

		/// <summary>
		/// Gets or sets the name of the resource group.
		/// </summary>
		/// <value>The name of the resource group.</value>
		public string ResourceGroupName
		{
			get;
			set;
		}

		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		#endregion Public Properties

		#region Protected Static Properties

		/// <summary>
		/// Gets or sets method used to normalize file path.
		/// </summary>
		protected static Func<string, string> DoNormalizeFilePath
		{
			get;
			set;
		}

		#endregion Protected Static Properties

		#region Protected Properties

		/// <summary>
		/// Gets the MiyagiSystem.
		/// </summary>
		protected MiyagiSystem MiyagiSystem
		{
			get;
			private set;
		}

		#endregion Protected Properties

		#endregion Properties

		#region Methods

		#region Public Static Methods

		/// <summary>
		/// Normalizes the file path.
		/// </summary>
		/// <param name="file">The file that should be normalized.</param>
		/// <returns>The normalizerd file path.</returns>
		public static string NormalizeFilePath(string file)
		{
			return DoNormalizeFilePath(file);
		}

		#endregion Public Static Methods

		#region Public Methods

		/// <summary>
		/// Creates a GPU program.
		/// </summary>
		/// <param name="gpuProg">The GpuProgram.</param>
		public abstract void CreateGpuProgram(GpuProgram gpuProg);

		/// <summary>
		/// Creates a render texture.
		/// </summary>
		/// <param name="name">The name of the render texture.</param>
		/// <param name="size">The size of the render texture.</param>
		/// <param name="backgroundColour">The background colour of the render texture.</param>
		/// <param name="camera">The backend camera.</param>
		public abstract void CreateRenderTexture(string name, Size size, Colour backgroundColour, object camera);

		/// <summary>
		/// Creates a texture.
		/// </summary>
		/// <param name="name">The name of the texture.</param>
		/// <param name="size">The size of the texture.</param>
		public abstract object CreateTexture(string name, Size size);

		/// <summary>
		/// Gets the alpha value of the specified texture at the specified point.
		/// </summary>
		/// <param name="textureHandle">The handle.</param>
		/// <param name="p">The point.</param>
		/// <returns>The alpha value</returns>
		public abstract float GetTextureAlpha(object textureHandle, Point p);

		/// <summary>
		/// Gets the size of a texture.
		/// </summary>
		/// <param name="handle">The handle of the texture.</param>
		/// <returns>A Size representing the size of a texture.</returns>
		public abstract Size GetTextureSize(object textureHandle);

		/// <summary>
		/// Loads the texture.
		/// </summary>
		/// <param name="name">The name of the texture.</param>
		/// <returns>An <see cref="object"/> representing the handle of the texture.</returns>
		public abstract object LoadTexture(string name);

		/// <summary>
		/// Updates the current window.
		/// </summary>
		/// <returns><c>true</c> if the window is open; otherwise, <c>false</c>.</returns>
		public abstract bool MessagePump();

		/// <summary>
		/// Removes a texture.
		/// </summary>
		/// <param name="name">The name of the texture.</param>
		public abstract void RemoveTexture(string name);

		/// <summary>
		/// Sets the context.
		/// </summary>
		/// <param name="args">The args.</param>
		public abstract void SetContext(object args);

		/// <summary>
		/// Gets whether a texture exists.
		/// </summary>
		/// <param name="name">The name of the texture.</param>
		/// <returns><c>true</c> if the texture exists; otherwise, <c>false</c>.</returns>
		public abstract bool TextureExists(string name);

		/// <summary>
		/// Converts a byte array to a texture.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="textureHandle">The backend handle of the texture.</param>
		public abstract void WriteToTexture(byte[] bytes, object textureHandle);

		#endregion Public Methods

		#endregion Methods
	}
}