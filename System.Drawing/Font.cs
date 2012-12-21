using System;
using System.Runtime.Serialization;

#if MONOMAC
using MonoMac.CoreGraphics;
#else
using MonoTouch.CoreGraphics;
#endif

namespace System.Drawing
{

	public sealed class Font : MarshalByRefObject, ISerializable, ICloneable, IDisposable {
		const byte DefaultCharSet = 1;

		CGFont font;


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
			throw new NotImplementedException ();
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
		public void Dispose ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

