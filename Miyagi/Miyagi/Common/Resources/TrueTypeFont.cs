#if USE_SYSTEM_DRAWING

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
	using System.Drawing.Drawing2D;
	using System.Drawing.Text;
	using System.IO;
	using System.Linq;
	using System.Xml;
	using System.Xml.Linq;

	using Miyagi.Common.Data;
	using Miyagi.Internals;

	using RectangleF = Miyagi.Common.Data.RectangleF;

	using SD = System.Drawing;

	using Size = Miyagi.Common.Data.Size;

	/// <summary>
	/// A font based on a ttf file.
	/// </summary>
	public sealed class TrueTypeFont : Font
	{
		#region Fields

		private static readonly List<Range> DefaultCodePoints = new List<Range>
																{
																	new Range(32, 255)
																};
		private static readonly Dictionary<string, IDictionary<char, RectangleF>> GlobalGlyphCoordinates = new Dictionary<string, IDictionary<char, RectangleF>>();
		private static readonly Dictionary<string, int> GlobalLeadings = new Dictionary<string, int>();

		private IList<Range> codePoints;

		#endregion Fields

		#region Constructors

		private TrueTypeFont(string name)
			: base(name)
		{
		}

		#endregion Constructors

		#region Properties

		#region Public Properties

		/// <summary>
		/// Gets the FontStyle.
		/// </summary>
		public SD.FontStyle FontStyle
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the resolution of the font in dpi.
		/// </summary>
		public int Resolution
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the size of the font in points.
		/// </summary>
		public int Size
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the name of the texture of the font.
		/// </summary>
		public override string TextureName
		{
			get
			{
				return Path.GetFileName(this.FileName) + "_Texture_" + this.Size + "_" + this.Resolution + "_" + this.FontStyle;
			}
		}

		public object TextureHandle
		{
			get;
			private set;
		}

		#endregion Public Properties

		#endregion Properties

		#region Methods

		#region Public Static Methods

		/// <summary>
		/// Creates a new TrueTypeFont.
		/// </summary>
		/// <param name="system">The MiyagiSystem.</param>
		/// <param name="name">The name of the TrueTypeFont.</param>
		/// <param name="fontFileName">The filename of the font.</param>
		/// <param name="size">The desired size in points.</param>
		/// <param name="resolution">The resolution in dpi.</param>
		/// <param name="fontStyle">The style of the font.</param>
		/// <returns>The created TrueType font.</returns>
		public static TrueTypeFont Create(MiyagiSystem system, string name, string fontFileName, int size, int resolution, SD.FontStyle fontStyle)
		{
			return Create(system, name, fontFileName, size, resolution, fontStyle, DefaultCodePoints);
		}

		/// <summary>
		/// Creates a new TrueTypeFont.
		/// </summary>
		/// <param name="system">The MiyagiSystem.</param>
		/// <param name="name">The name of the TrueTypeFont.</param>
		/// <param name="fontFileName">The filename of the font.</param>
		/// <param name="size">The desired size in points.</param>
		/// <param name="resolution">The resolution in dpi.</param>
		/// <param name="fontStyle">The style of the font.</param>
		/// <param name="codePoints">A list codepoint ranges.</param>
		/// <returns>The created TrueType font.</returns>
		public static TrueTypeFont Create(MiyagiSystem system, string name, string fontFileName, int size, int resolution, SD.FontStyle fontStyle, IList<Range> codePoints)
		{
			if (codePoints == null)
			{
				codePoints = DefaultCodePoints;
			}

			var retValue = new TrueTypeFont(name)
			{
				FileName = fontFileName,
				Size = size,
				Resolution = resolution,
				FontStyle = fontStyle,
				codePoints = codePoints,
			};

			CreateFont(system, retValue);

			return retValue;
		}

		/// <summary>
		/// Creates TrueTypeFonts from XML.
		/// </summary>
		/// <param name="fileName">The location of the XML file.</param>
		/// <param name="system">The system.</param>
		/// <returns>The newly created TrueTypeFonts.</returns>
		public static IList<TrueTypeFont> CreateFromXml(string fileName, MiyagiSystem system)
		{
			var xDoc = XDocument.Load(fileName, LoadOptions.SetBaseUri);

			var elements = xDoc.Descendants("TrueTypeFont");
			var retValue = new List<TrueTypeFont>(elements.Count());
			retValue.AddRange(elements.Select(ele => CreateFromXml(ele, system)));

			return retValue;
		}

		/// <summary>
		/// Creates a TrueTypeFont from XML.
		/// </summary>
		/// <param name="xElement">An XElement that contains the XML representation of the TrueTypeFont.</param>
		/// <param name="system">The MiyagiSystem.</param>
		/// <returns>
		/// The newly created TrueTypeFont or null if the XML file is invalid.
		/// </returns>
		public static TrueTypeFont CreateFromXml(XElement xElement, MiyagiSystem system)
		{
			TrueTypeFont retValue;

			var xv = new XmlValidator();
			if (xv.ValidateAgainstInternalSchema(xElement, "TrueTypeFont"))
			{
				// create the TrueTypeFont using the Name attribute
				retValue = new TrueTypeFont(xElement.Attribute("Name").Value)
				{
					FileName = (string)xElement.Element("FileName"),
					FontStyle = xElement.Element("FontStyle").Value.ParseEnum<SD.FontStyle>(),
					Resolution = (int)xElement.Element("Resolution"),
					Size = (int)xElement.Element("Size"),
					TabSize = xElement.Element("TabSize") != null ? (int)xElement.Element("TabSize") : 3,
					Tracking = xElement.Element("Tracking") != null ? (int)xElement.Element("Tracking") : 0,
				};

				var leading = xElement.Element("Leading");
				if (leading != null)
				{
					retValue.Leading = (int)leading;
				}

				var spaceWidth = xElement.Element("SpaceWidth");
				if (spaceWidth != null)
				{
					retValue.SpaceWidth = (int)spaceWidth;
				}

				// get CodePoints
				IList<Range> codePoints;
				if (xElement.Element("CodePoints") == null)
				{
					codePoints = DefaultCodePoints;
				}
				else
				{
					codePoints = new List<Range>(
						from codePointNode in xElement.Descendants("CodePoint")
						let codePoint = new Range(
							(int)codePointNode.Element("First"),
							(int)codePointNode.Element("Last"))
						select codePoint);
				}

				retValue.codePoints = codePoints;

				CreateFont(system, retValue);
			}
			else
			{
				throw new XmlException("Invalid font XElement");
			}

			return retValue;
		}

		/// <summary>
		/// Converts a TrueType to an image font.
		/// </summary>
		/// <param name="outputPath">The output path.</param>
		/// <param name="ttfFileName">The name of the TTF file.</param>
		/// <param name="fontStyle">The font style.</param>
		/// <param name="fontSize">Size of the font.</param>
		/// <param name="fontResolution">The font resolution.</param>
		/// <param name="leading">The leading.</param>
		/// <param name="tracking">The tracking.</param>
		/// <param name="spaceWidth">The width of the space character.</param>
		/// <param name="codePoints">The code points.</param>
		/// <remarks></remarks>
		public static void TrueTypeToImageFont(string outputPath, string ttfFileName, SD.FontStyle fontStyle, int fontSize, int fontResolution, int leading = 0, int tracking = 0, int spaceWidth = 0, IList<Range> codePoints = null)
		{
			if (codePoints == null)
			{
				codePoints = DefaultCodePoints;
			}

			using (var pfc = new PrivateFontCollection())
			{
				pfc.AddFontFile(ttfFileName);
				var glyphs = new Dictionary<char, RectangleF>();
				using (var bitmap = TrueTypeFont.CreateFontBitmap(pfc.Families[0], fontStyle, fontSize, fontResolution, ref leading, codePoints, glyphs))
				{
					string name = Path.GetFileName(ttfFileName) + "_" + fontStyle + "_" + fontSize + "_" + fontResolution;
					var imagefont = new ImageFont(name, glyphs, name + ".png", leading, tracking, new Size(bitmap.Size.Width, bitmap.Size.Height));

					if (!Directory.Exists(outputPath))
					{
						Directory.CreateDirectory(outputPath);
					}

					imagefont.ToXElement().Save(Path.Combine(outputPath, name + ".xml"));
					bitmap.Save(Path.Combine(outputPath, name + ".png"));
				}
			}
		}

		#endregion Public Static Methods

		#region Public Methods

		/// <summary>
		/// Returns a string representing the TrueTypeFont.
		/// </summary>
		/// <returns>A string representing the TrueTypeFont.</returns>
		public override string ToString()
		{
			return "TrueTypeFont(" + this.Name + "," + this.Size + " pts," + this.Resolution + " dpi," + this.FontStyle + ")";
		}

		/// <summary>
		/// Converts the font to an XElement.
		/// </summary>
		/// <returns>An <see cref="XElement"/> representing the font.</returns>
		public override XElement ToXElement()
		{
			return new XElement(
				"TrueTypeFont",
				new XAttribute("Name", this.Name),
				new XElement("FileName", this.FileName),
				new XElement("FontStyle", this.FontStyle),
				new XElement("Resolution", this.Resolution),
				new XElement("Size", this.Size),
				new XElement("Leading", this.Leading),
				new XElement("SpaceWidth", this.SpaceWidth),
				new XElement("TabSize", this.TabSize),
				new XElement("Tracking", this.Tracking),
				new XElement("CodePoints", this.codePoints.Select(cp => new XElement("CodePoint", new XElement("First", cp.First), new XElement("Last", cp.Last)))));
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Creates the font.
		/// </summary>
		/// <param name="system">The MiyagiSystem.</param>
		protected override void CreateFont(MiyagiSystem system)
		{
			this.CreateFontTexture(system, this.FontStyle);
			this.TextureSize = system.Backend.GetTextureSize(this.TextureHandle);
		}

		#endregion Protected Methods

		#region Private Static Methods

		private static void CreateFont(MiyagiSystem system, TrueTypeFont font)
		{
			if (!system.Backend.TextureExists(font.TextureName))
			{
				font.CreateFont(system);
			}
			else
			{
				font.Leading = GlobalLeadings[font.TextureName];
				font.GlyphCoordinates = GlobalGlyphCoordinates[font.TextureName];
				font.TextureSize = system.Backend.GetTextureSize(font.TextureName);
			}
		}

		/// <exception cref="ArgumentException">Invalid font style</exception>
		private static SD.Bitmap CreateFontBitmap(SD.FontFamily fontFamily, SD.FontStyle fontStyle, int fontSize, int resolution, ref int leading, IEnumerable<Range> codePoints, IDictionary<char, RectangleF> glyphs)
		{
			if (!fontFamily.IsStyleAvailable(fontStyle))
			{
				throw new ArgumentException("Invalid font style", "fontStyle");
			}

			using (var font = new SD.Font(fontFamily, fontSize, fontStyle, SD.GraphicsUnit.Point))
			{
				if (leading == 0)
				{
					leading = (int)Math.Ceiling(font.GetHeight());
				}

				using (var sf = new StringFormat(StringFormat.GenericTypographic))
				{
					sf.FormatFlags ^= StringFormatFlags.NoClip;

					IEnumerable<int> codePointList = GetCodePointList(codePoints);
					float x = 0, y = 0;

					// get texture dimensions
					var texSize = GetTexSize(codePointList, resolution, font, sf, leading);

					// draw font texture
					var fontBitmap = new SD.Bitmap(texSize.Width, texSize.Height);
					fontBitmap.SetResolution(resolution, resolution);
					float italicPadding = 0f;//pkubat added
					if (font.Italic)
					{
						italicPadding = fontSize / 7f;
					}

					using (var g = SD.Graphics.FromImage(fontBitmap))
					{
						using (var brush = new SD.SolidBrush(SD.Color.White))
						{
							g.Clear(SD.Color.Transparent);
							//g.InterpolationMode = InterpolationMode.Low;
							//g.CompositingQuality = CompositingQuality.HighSpeed;
							//g.SmoothingMode = SmoothingMode.HighSpeed;

							g.InterpolationMode = InterpolationMode.HighQualityBicubic;
							g.CompositingQuality = CompositingQuality.HighQuality;
							g.SmoothingMode = SmoothingMode.HighQuality;

							g.PixelOffsetMode = PixelOffsetMode.HighQuality;
							g.TextRenderingHint = TextRenderingHint.AntiAlias;
							g.PageUnit = SD.GraphicsUnit.Pixel;

							foreach (int i in codePointList)
							{
								string c = char.ConvertFromUtf32(i);
								var size = g.MeasureString(c, font, texSize.Width, sf);
								size.Width += italicPadding;

								if (x + size.Width >= texSize.Width)
								{
									x = 0;
									y += leading + 2;
								}

								g.DrawString(c, font, brush, new SD.RectangleF(x, y, size.Width, leading), sf);

								// fill glyphdict
								glyphs[(char)i] = new RectangleF(
									x / texSize.Width,
									y / texSize.Height,
									size.Width / texSize.Width,
									(float)leading / texSize.Height);

								x += size.Width + 2;
							}

							// get space width
							var spaceSize = g.MeasureString(" ", font);
							glyphs[' '] = new RectangleF(0, 0, spaceSize.Width / texSize.Width, (float)leading / texSize.Height);
						}

						return fontBitmap;
					}
				}
			}
		}

		private static IEnumerable<int> GetCodePointList(IEnumerable<Range> codePoints)
		{
			var codePointList =
				from codePointCouple in codePoints
				let codePointRange = Enumerable.Range(codePointCouple.First, codePointCouple.Last - codePointCouple.First + 1)
				from codePoint in codePointRange
				orderby codePoint
				select codePoint;

			codePointList = codePointList.Distinct();
			return codePointList;
		}

		private static Size GetTexSize(IEnumerable<int> codePointList, int resolution, System.Drawing.Font font, SD.StringFormat sf, int leading)
		{
			int texWidth, texHeight;
			float totalWidth = 0;
			using (var fontBitmap = new SD.Bitmap(1, 1))
			{
				fontBitmap.SetResolution(resolution, resolution);
				using (var g = SD.Graphics.FromImage(fontBitmap))
				{
					g.InterpolationMode = InterpolationMode.Low;
					g.CompositingQuality = CompositingQuality.HighSpeed;
					g.SmoothingMode = SmoothingMode.HighSpeed;
					g.PixelOffsetMode = PixelOffsetMode.None;
					g.TextRenderingHint = TextRenderingHint.AntiAlias;
					g.PageUnit = SD.GraphicsUnit.Pixel;

					foreach (int i in codePointList)
					{
						string c = char.ConvertFromUtf32(i);
						var size = g.MeasureString(c, font, 4096, sf);
						totalWidth += size.Width + 2;
					}

					double s = totalWidth * ((leading * 1.5) + 2);
					texWidth = Math.Sqrt(s).NextPowerOfTwo();
					texHeight = (s / texWidth).NextPowerOfTwo();
				}
			}

			return new Size(texWidth, texHeight);
		}

		#endregion Private Static Methods

		#region Private Methods

		private void CreateFontTexture(MiyagiSystem system, SD.FontStyle fontStyle)
		{
			using (var pfc = new PrivateFontCollection())
			{
				pfc.AddFontFile(this.FileName);

				int leading = this.Leading;
				using (SD.Bitmap fontBitmap = CreateFontBitmap(pfc.Families[0], fontStyle, this.Size, this.Resolution, ref leading, this.codePoints, this.GlyphCoordinates))
				{
					this.Leading = leading;
					GlobalGlyphCoordinates[this.TextureName] = this.GlyphCoordinates;
					GlobalLeadings[this.TextureName] = this.Leading;
					this.TextureHandle = system.Backend.CreateTexture(this.TextureName, new Size(fontBitmap.Size.Width, fontBitmap.Size.Height));
					system.Backend.WriteToTexture(fontBitmap.ToByteArray(), this.TextureHandle);
				}
			}
		}

		#endregion Private Methods

		#endregion Methods
	}
}

#endif