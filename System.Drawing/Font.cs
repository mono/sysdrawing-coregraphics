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
		{
			if (family == null)
				throw new ArgumentNullException ("family");
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


			try {
				nativeFont = new CTFont(familyName,emSize);
			}
			catch
			{
				//nativeFont = CGFont.CreateWithFontName("Lucida Grande");
				nativeFont = new CTFont("Helvetica",emSize);
			}

			CTFontSymbolicTraits tMask = CTFontSymbolicTraits.None;

			if ((style & FontStyle.Bold) == FontStyle.Bold)
				tMask |= CTFontSymbolicTraits.Bold;
			if ((style & FontStyle.Italic) == FontStyle.Italic)
				tMask |= CTFontSymbolicTraits.Italic;

			var nativeFont2 = nativeFont.WithSymbolicTraits(emSize,tMask,tMask);

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
	}
}

