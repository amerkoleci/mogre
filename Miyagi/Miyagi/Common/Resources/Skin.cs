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
namespace Miyagi.Common.Resources
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Security.Permissions;
	using System.Xml.Linq;

	using Miyagi.Common.Rendering;
	using Miyagi.Common.Serialization;
	using Miyagi.Internals;
	using Miyagi.Internals.Resources.Gfx;
	using Miyagi.Internals.ThirdParty;

	using RectangleF = Miyagi.Common.Data.RectangleF;

	/// <summary>
	/// A Skin represents different sub skins and their textures.
	/// </summary>
	[SerializableType]
	public sealed class Skin : IDeepCopiable<Skin>, IXmlWritable, INamable
	{
		#region Fields

		internal const string CheckedSkin = ".Checked";
		internal const string DisabledSkin = ".Disabled";
		internal const string FocusedSkin = ".Focused";
		internal const string IndeterminateSkin = ".Indeterminate";

		private readonly FastSmartWeakEvent<EventHandler> subSkinChanged = new FastSmartWeakEvent<EventHandler>();

		private TextureCollection subSkins;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Skin class.
		/// </summary>
		/// <param name="name">The name of the Skin.</param>
		public Skin(string name)
		{
			this.Name = name;
			this.SubSkins = new TextureCollection();
		}

		#endregion Constructors

		#region Events

		internal event EventHandler SubSkinChanged
		{
			add
			{
				this.subSkinChanged.Add(value);
			}

			remove
			{
				this.subSkinChanged.Remove(value);
			}
		}

		#endregion Events

		#region Properties

		#region Public Properties

		/// <summary>
		/// Gets or sets the name of the Skin.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the collection of textures.
		/// </summary>
		public TextureCollection SubSkins
		{
			get
			{
				return this.subSkins;
			}

			private set
			{
				if (this.subSkins != null)
				{
					this.subSkins.TextureChanged -= this.SkinTextureChanged;
				}

				this.subSkins = value;
				this.subSkins.TextureChanged += this.SkinTextureChanged;
			}
		}

		#endregion Public Properties

		#endregion Properties

		#region Methods

		#region Public Static Methods

		/// <summary>
		/// Creates a Skin from XML.
		/// </summary>
		/// <param name="xElement">An XElement that contains the XML representation of the Skin.</param>
		/// <param name="system">The MiyagiSystem.</param>
		/// <returns>
		/// The newly created Skin or null if the XML file is invalid.
		/// </returns>
		public static Skin CreateFromXml(XElement xElement, MiyagiSystem system)
		{
			Skin retValue = null;

			var xv = new XmlValidator();
			if (xv.ValidateAgainstInternalSchema(xElement, "Skin"))
			{
				// create the skin using the Name attribute
				retValue = new Skin(xElement.Attribute("Name").Value)
				{
					SubSkins = new TextureCollection(
								   from subSkinNode in xElement.Descendants("SubSkin")
								   select new KeyValuePair<string, Texture>(
									   subSkinNode.Attribute("Name").Value,
									   Texture.CreateFromXml(subSkinNode, system)))
				};
			}

			return retValue;
		}

		/// <summary>
		/// Creates Skins from XML.
		/// </summary>
		/// <param name="fileName">The location of the XML file.</param>
		/// <param name="system">The system.</param>
		/// <returns>The newly created Skins.</returns>
		public static IList<Skin> CreateFromXml(string fileName, MiyagiSystem system = null)
		{
			var xDoc = XDocument.Load(fileName, LoadOptions.SetBaseUri);

			var elements = xDoc.Descendants("Skin");
			var retValue = new List<Skin>(elements.Count());
			retValue.AddRange(elements.Select(ele => CreateFromXml(ele, system)));

			return retValue;
		}

		#endregion Public Static Methods

		#region Public Methods

		/// <summary>
		/// Creates a deep copy of the skin.
		/// </summary>
		/// <returns>A deep copy of the skin.</returns>
		public Skin CreateDeepCopy()
		{
			Skin retValue = new Skin(this.Name);
			foreach (var skin in this.SubSkins)
			{
				retValue.SubSkins[skin.Key] = skin.Value.CreateDeepCopy();
			}

			return retValue;
		}

		/// <summary>
		/// Gets a value indicating whether a skin is defined.
		/// </summary>
		/// <param name="subSkin">The name of the skin.</param>
		/// <returns><c>true</c> if the skin is defined; otherwise, <c>false</c>.</returns>
		public bool IsSubSkinDefined(string subSkin)
		{
			return this.SubSkins.ContainsKey(subSkin);
		}

		/// <summary>
		/// Returns the name of the skin.
		/// </summary>
		/// <returns>The name of the skin.</returns>
		public override string ToString()
		{
			return "Skin(" + this.Name + ")";
		}

		/// <summary>
		/// Converts the skin to an XElement.
		/// </summary>
		/// <returns>An <see cref="XElement"/> representing the skin.</returns>
		public XElement ToXElement()
		{
			return new XElement("Skin", new XAttribute("Name", this.Name), this.GetSubSkins());
		}

		#endregion Public Methods

		#region Internal Static Methods

		[SecurityPermission(SecurityAction.LinkDemand)]
		internal static Skin CreateForDialogBox(MiyagiSystem system)
		{
			const string TexName = "Miyagi_DialogBox";
			LoadDefaultTexture(TexName, system.Backend, Assembly.GetExecutingAssembly().GetManifestResourceStream("Miyagi.Internals.Resources.Gfx.DialogBox.png"));

			var retValue = new Skin("DialogBox");
			retValue.SubSkins["DialogBox"] = new Texture(TexName, GetUV(DialogBoxSkin.UV_Panel));
			return retValue;
		}

		[SecurityPermission(SecurityAction.LinkDemand)]
		internal static Skin CreateForDialogBoxButton(MiyagiSystem system)
		{
			const string TexName = "Miyagi_DialogBox";
			LoadDefaultTexture(TexName, system.Backend, Assembly.GetExecutingAssembly().GetManifestResourceStream("Miyagi.Internals.Resources.Gfx.DialogBox.png"));

			var retValue = new Skin("DialogBox.Button");
			retValue.SubSkins["DialogBox.Button"] = retValue.SubSkins["DialogBox.Button.MouseUp"] = retValue.SubSkins["DialogBox.Button.MouseLeave"] = new Texture(TexName, GetUV(DialogBoxSkin.UV_Button));
			retValue.SubSkins["DialogBox.Button.MouseDown"] = new Texture(TexName, GetUV(DialogBoxSkin.UV_Button_MouseDown));
			retValue.SubSkins["DialogBox.Button.MouseEnter"] = new Texture(TexName, GetUV(DialogBoxSkin.UV_Button_MouseEnter));
			return retValue;
		}

		[SecurityPermission(SecurityAction.LinkDemand)]
		internal static Texture CreateForToolTip(MiyagiSystem system)
		{
			const string TexName = "Miyagi_Tooltip";
			LoadDefaultTexture(TexName, system.Backend, Assembly.GetExecutingAssembly().GetManifestResourceStream("Miyagi.Internals.Resources.Gfx.ToolTip.png"));

			return new Texture(TexName);
		}

		#endregion Internal Static Methods

		#region Private Static Methods

		private static RectangleF GetUV(string s)
		{
			var ic = CultureInfo.InvariantCulture;
			string[] uv = s.Split(',');
			return new RectangleF(
				float.Parse(uv[0], ic),
				float.Parse(uv[1], ic),
				float.Parse(uv[2], ic),
				float.Parse(uv[3], ic));
		}

		[SecurityPermission(SecurityAction.LinkDemand)]
		private static void LoadDefaultTexture(string texName, Backend backend, Stream stream)
		{
			if (backend.TextureExists(texName))
			{
				return;
			}

			var image = (Bitmap)Image.FromStream(stream);
			var size = new Miyagi.Common.Data.Size(image.Size.Width, image.Size.Height);
			var texture = backend.CreateTexture(texName, size);
			backend.WriteToTexture(image.ToByteArray(), texture);

			stream.Close();
		}

		#endregion Private Static Methods

		#region Private Methods

		private XElement GetSubSkins()
		{
			var retValue = new XElement("SubSkins");
			foreach (var subskin in this.SubSkins)
			{
				var skinEle = subskin.Value.ToXElement("SubSkin");
				skinEle.Add(new XAttribute("Name", subskin.Key));
				retValue.Add(skinEle);
			}

			return retValue;
		}

		private void SkinTextureChanged(object sender, EventArgs e)
		{
			if (this.subSkinChanged != null)
			{
				this.subSkinChanged.Raise(this, e);
			}
		}

		#endregion Private Methods

		#endregion Methods
	}
}