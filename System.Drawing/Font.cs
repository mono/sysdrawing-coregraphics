using System;
using System.Runtime.Serialization;

#if MONOMAC
using MonoMac.CoreGraphics;
using MonoMac.CoreText;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.CoreText;
#endif

namespace System.Drawing
{

	public sealed class Font : MarshalByRefObject, ISerializable, ICloneable, IDisposable {
		const byte DefaultCharSet = 1;

		internal CTFont nativeFont;
		float sizeInPoints = 0;
		GraphicsUnit unit = GraphicsUnit.Point;
		float size;
		bool underLine = false;
		bool strikeThrough = false;

		static float dpiScale = 96f / 72f;

		public Font (FontFamily family, float emSize,  GraphicsUnit unit)
			: this (family, emSize, FontStyle.Regular, unit, DefaultCharSet, false)
		{
		}

		public Font (string familyName, float emSize,  GraphicsUnit unit)
			: this (new FontFamily (familyName), emSize, FontStyle.Regular, unit, DefaultCharSet, false)
		{
		}

		public Font (FontFamily family, float emSize)
			: this (family, emSize, FontStyle.Regular, GraphicsUnit.Point, DefaultCharSet, false)
		{
		}

		public Font (FontFamily family, float emSize, FontStyle style)
			: this (family, emSize, style, GraphicsUnit.Point, DefaultCharSet, false)
		{
		}

		public Font (FontFamily family, float emSize, FontStyle style, GraphicsUnit unit)
			: this (family, emSize, style, unit, DefaultCharSet, false)
		{
		}

		public Font (FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
			: this (family, emSize, style, unit, gdiCharSet, false)
		{
		}

		public Font (FontFamily family, float emSize, FontStyle style,
				GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
			: this (family.Name, emSize, style, unit, gdiCharSet, gdiVerticalFont)
		{

		}

		public Font (string familyName, float emSize)
			: this (familyName, emSize, FontStyle.Regular, GraphicsUnit.Point, DefaultCharSet, false)
		{
		}

		public Font (string familyName, float emSize, FontStyle style)
			: this (familyName, emSize, style, GraphicsUnit.Point, DefaultCharSet, false)
		{
		}

		public Font (string familyName, float emSize, FontStyle style, GraphicsUnit unit)
			: this (familyName, emSize, style, unit, DefaultCharSet, false)
		{
		}

		public Font (string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
			: this (familyName, emSize, style, unit, gdiCharSet, false)
		{
		}

		public Font (string familyName, float emSize, FontStyle style,
		             GraphicsUnit unit, byte gdiCharSet, bool  gdiVerticalFont )
		{
			if (emSize <= 0)
				throw new ArgumentException("emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.","emSize");


			// convert to 96 Dpi to be consistent with Windows
			var dpiSize = emSize * dpiScale;

			try {
				nativeFont = new CTFont(familyName,dpiSize);
			}
			catch
			{
				//nativeFont = new CTFont("Lucida Grande",emSize);
				nativeFont = new CTFont("Helvetica",dpiSize);
			}

			CTFontSymbolicTraits tMask = CTFontSymbolicTraits.None;

			if ((style & FontStyle.Bold) == FontStyle.Bold)
				tMask |= CTFontSymbolicTraits.Bold;
			if ((style & FontStyle.Italic) == FontStyle.Italic)
				tMask |= CTFontSymbolicTraits.Italic;
			strikeThrough = (style & FontStyle.Strikeout) == FontStyle.Strikeout;
			underLine = (style & FontStyle.Underline) == FontStyle.Underline;

			var nativeFont2 = nativeFont.WithSymbolicTraits(dpiSize,tMask,tMask);

			if (nativeFont2 != null)
				nativeFont = nativeFont2;

			sizeInPoints = emSize;
			this.unit = unit;
			
			// FIXME
			// I do not like the hard coded 72 but am just trying to boot strap the Font class right now
			size = ConversionHelpers.GraphicsUnitConversion(GraphicsUnit.Point, unit, 72.0f, sizeInPoints); 
		}

		#region ISerializable implementation
		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException ();
		}
		#endregion

		#region ICloneable implementation
		public object Clone ()
		{
			throw new NotImplementedException ();
		}
		#endregion

		#region IDisposable implementation
		~Font ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
		}
		
		internal void Dispose (bool disposing)
		{
			if (disposing){
				if (nativeFont != null){
					nativeFont.Dispose ();
					nativeFont = null;
				}
			}
		}
		
#endregion
		
		public float SizeInPoints 
		{
			get { return sizeInPoints; }
		}
		
		public GraphicsUnit Unit 
		{
			get { return unit; }
			
		}
		
		public float Size 
		{
			get { 
				return size; 
			}
			
		}

		public bool Bold 
		{ 
			get { return (nativeFont.SymbolicTraits & ~CTFontSymbolicTraits.Bold) == CTFontSymbolicTraits.Bold; }
		}

		public bool Italic
		{ 
			get { return (nativeFont.SymbolicTraits & ~CTFontSymbolicTraits.Italic) == CTFontSymbolicTraits.Italic; }
		}

		public bool Underline
		{ 
			get { return underLine; }
		}

		public bool Strikeout
		{ 
			get { return strikeThrough; }
		}

		public int Height {
			get { return (int)Math.Round (GetHeight ()); }
		}

		/**
		 * 
		 * Returns: The line spacing, in pixels, of this font.
		 * 
		 * The line spacing of a Font is the vertical distance between the base lines of 
		 * two consecutive lines of text. Thus, the line spacing includes the blank space 
		 * between lines along with the height of the character itself.
		 * 
		 * If the Unit property of the font is set to anything other than GraphicsUnit.Pixel, 
		 * the height (in pixels) is calculated using the vertical resolution of the 
		 * screen display. For example, suppose the font unit is inches and the font size 
		 * is 0.3. Also suppose that for the corresponding font family, the em-height 
		 * is 2048 and the line spacing is 2355. For a screen display that has a vertical 
		 * resolution of 96 dots per inch, you can calculate the height as follows:
		 * 
		 * 2355*(0.3/2048)*96 = 33.11719
		 * 
		 **/
		public float GetHeight() 
		{


			// Documentation for Accessing Font Metrics
			// http://developer.apple.com/library/ios/#documentation/StringsTextFonts/Conceptual/CoreText_Programming/Operations/Operations.html
			float lineHeight = 0;
			lineHeight += nativeFont.AscentMetric;
			lineHeight += nativeFont.DescentMetric;
			lineHeight += nativeFont.LeadingMetric;


			// Still have not figured this out yet!!!!
			return lineHeight;
		}
	}
}

