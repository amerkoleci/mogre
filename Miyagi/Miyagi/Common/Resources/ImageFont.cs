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
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml;
	using System.Xml.Linq;

	using Miyagi.Common.Data;
	using Miyagi.Common.Rendering;
	using Miyagi.Internals;

	/// <summary>
	/// A font based on an image file.
	/// </summary>
	public sealed class ImageFont : Font
	{
		#region Constructors

		internal ImageFont(string name, IDictionary<char, RectangleF> glyphs, string filename, int leading, int tracking, Size textureSize)
			: this(name)
		{
			this.GlyphCoordinates = glyphs;
			this.FileName = filename;
			this.Leading = leading;
			this.Tracking = tracking;
			this.TextureSize = textureSize;
		}

		private ImageFont(string name)
			: base(name)
		{
		}

		#endregion Constructors

		#region Properties

		#region Public Properties

		/// <summary>
		/// Gets the name of the texture of the font.
		/// </summary>
		public override string TextureName
		{
			get
			{
				return this.FileName;
			}
		}

		#endregion Public Properties

		#endregion Properties

		#region Methods

		#region Public Static Methods

		/// <summary>
		/// Creates a new Font for an image font.
		/// </summary>
		/// <param name="system">The MiyagiSystem.</param>
		/// <param name="name">The name of the ImageFont.</param>
		/// <param name="fontFileName">The filename of the ImageFont.</param>
		/// <param name="glyphCoordinates">The glyph coordinates of the ImageFont.</param>
		/// <returns>The newly created ImageFont.</returns>
		public static ImageFont Create(MiyagiSystem system, string name, string fontFileName, IDictionary<char, RectangleF> glyphCoordinates)
		{
			ImageFont retValue = new ImageFont(name)
			{
				FileName = fontFileName,
				GlyphCoordinates = glyphCoordinates
			};
			retValue.CreateFont(system);
			return retValue;
		}

		/// <summary>
		/// Creates ImageFonts from XML.
		/// </summary>
		/// <param name="fileName">The location of the XML file.</param>
		/// <param name="system">The system.</param>
		/// <returns>The newly created ImageFonts.</returns>
		public static IList<ImageFont> CreateFromXml(string fileName, MiyagiSystem system)
		{
			var xDoc = XDocument.Load(fileName, LoadOptions.SetBaseUri);

			var elements = xDoc.Descendants("ImageFont");
			var retValue = new List<ImageFont>(elements.Count());
			retValue.AddRange(elements.Select(ele => CreateFromXml(ele, system)));

			return retValue;
		}

		/// <summary>
		/// Creates a ImageFont from XML.
		/// </summary>
		/// <param name="xElement">An XElement that contains the XML representation of the ImageFont.</param>
		/// <param name="system">The MiyagiSystem.</param>
		/// <returns>The newly created ImageFont or null if the XML file is invalid.</returns>
		public static ImageFont CreateFromXml(XElement xElement, MiyagiSystem system)
		{
			ImageFont retValue = null;

			var xv = new XmlValidator();
			if (xv.ValidateAgainstInternalSchema(xElement, "ImageFont"))
			{
				// create the ImageFont using the Name attribute
				retValue = new ImageFont(xElement.Attribute("Name").Value)
				{
					FileName = (string)xElement.Element("FileName"),
					TabSize = xElement.Element("TabSize") != null ? (int)xElement.Element("TabSize") : 3,
					Tracking = xElement.Element("Tracking") != null ? (int)xElement.Element("Tracking") : 0,
					GlyphCoordinates = GetGlyphCoordinates(xElement)
				};

				var leading = xElement.Element("Leading");
				if (leading != null)
				{
					retValue.Leading = (int)leading;
				}

				var spaceWidth = xElement.Element("SpaceWidth");
				if (leading != null)
				{
					retValue.SpaceWidth = (int)spaceWidth;
				}

				retValue.CreateFont(system);
			}
			else
			{
				throw new XmlException("Invalid font XElement");
			}

			return retValue;
		}

		#endregion Public Static Methods

		#region Public Methods

		/// <summary>
		/// Returns a string representing the ImageFont.
		/// </summary>
		/// <returns>A string representing the ImageFont.</returns>
		public override string ToString()
		{
			return "ImageFont(" + this.Name + ")";
		}

		/// <summary>
		/// Converts the font to an XElement.
		/// </summary>
		/// <returns>An <see cref="XElement"/> representing the font.</returns>
		public override XElement ToXElement()
		{
			return new XElement(
				"ImageFont",
				new XAttribute("Name", this.Name),
				new XElement("FileName", this.FileName),
				new XElement("Leading", this.Leading),
				new XElement("SpaceWidth", this.SpaceWidth),
				new XElement("TabSize", this.TabSize),
				new XElement("Tracking", this.Tracking),
				new XElement(
					"Glyphs",
					this.GlyphCoordinates.Select(glyph => new XElement(
															  "Glyph",
															  new XAttribute("Char", glyph.Key),
															  glyph.Value.ToXElement("UV")))));
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Creates the font.
		/// </summary>
		/// <param name="system">The MiyagiSystem.</param>
		protected override void CreateFont(MiyagiSystem system)
		{
			Backend backend = system.Backend;
			this.TextureSize = backend.GetTextureSize(this.TextureName);
		}

		#endregion Protected Methods

		#region Private Static Methods

		private static IDictionary<char, RectangleF> GetGlyphCoordinates(XContainer xElement)
		{
			return
				xElement.Descendants("Glyph").Select(glyphNode => new
				{
					glyphNode,
					glyph = new
					{
						Char = glyphNode.Attribute("Char").Value[0],
						UV = RectangleF.FromXElement(glyphNode.Element("UV"))
					}
				}).Select(t => t.glyph).ToDictionary(kvp => kvp.Char, kvp => kvp.UV);
		}

		#endregion Private Static Methods

		#endregion Methods
	}
}