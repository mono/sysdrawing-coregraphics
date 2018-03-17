using System;
using AppKit;
using CoreGraphics;
using CoreText;

namespace System.Drawing.Mac
{
	public partial class Extensions
	{
		public static NSFont ToNativeFont (this Font f)
		{
			return ObjCRuntime.Runtime.GetNSObject (f.nativeFont.Handle) as NSFont;
		}

		public static CTFont ToCTFont (this NSFont f)
		{
			// CTFont and NSFont are toll-free bridged
			return (CTFont)Activator.CreateInstance (
				typeof (CTFont),
				Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Instance,
				null,
				new object [] { f.Handle },
				null);
		}

		public static NSTextAlignment ToNSTextAlignment (this ContentAlignment a)
		{
			return (NSTextAlignment)ToCTTextAlignment (a);
		}

		public static NSColor ToNativeColor (this Color c)
		{
			return NSColor.FromDeviceRgba (c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
		}

		public static Color ToSDColor (this NSColor color)
		{
			var convertedColor = color.UsingColorSpace (NSColorSpace.GenericRGBColorSpace);
			if (convertedColor != null) {
				nfloat r, g, b, a;
				convertedColor.GetRgba (out r, out g, out b, out a);
				return Color.FromArgb ((int)(a * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255));
			}

			var cgColor = color.CGColor; // 10.8+
			if (cgColor != null) {
				if (cgColor.NumberOfComponents == 4)
					return Color.FromArgb (
						(int)(cgColor.Components [3] * 255),
						(int)(cgColor.Components [0] * 255),
						(int)(cgColor.Components [1] * 255),
						(int)(cgColor.Components [2] * 255));

				if (cgColor.NumberOfComponents == 2)
					return Color.FromArgb (
						(int)(cgColor.Components [1] * 255),
						(int)(cgColor.Components [0] * 255),
						(int)(cgColor.Components [0] * 255),
						(int)(cgColor.Components [0] * 255));
			}

			return Color.Transparent;
		}

		public static int ToArgb (this NSColor color)
		{
			return color.ToSDColor ().ToArgb ();
		}

		public static uint ToUArgb (this NSColor color)
		{
			return (uint)color.ToSDColor ().ToArgb ();
		}

		public static NSImage ToNSImage (this Image image)
		{
			if (image.NativeCGImage != null)
				return new NSImage (image.NativeCGImage, CGSize.Empty);

			return new NSImage (image.ToNSData (Imaging.ImageFormat.Png));
		}

		public static CGContext CGContext (this NSGraphicsContext context)
		{
			return context.CGContext;
		}
	}
}
