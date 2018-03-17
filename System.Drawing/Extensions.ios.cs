using System;
using UIKit;
using CoreGraphics;
using CoreText;

namespace System.Drawing.Mac
{
	public partial class Extensions
	{
		public static UIFont ToNativeFont (this Font f)
		{
			return ObjCRuntime.Runtime.GetNSObject (f.nativeFont.Handle) as UIFont;
		}

		public static CTFont ToCTFont (this UIFont f)
		{
			// CTFont and NSFont are toll-free bridged
			return (CTFont)Activator.CreateInstance (
				typeof (CTFont),
				Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Instance,
				null,
				new object [] { f.Handle },
				null);
		}

		public static UIColor ToNativeColor (this Color c)
		{
			return UIColor.FromRGBA (c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
		}

		public static Color ToSDColor (this UIColor color)
		{
			throw new NotImplementedException ();
		}

		public static int ToArgb (this UIColor color)
		{
			return color.ToSDColor ().ToArgb ();
		}

		public static uint ToUArgb (this UIColor color)
		{
			return (uint)color.ToSDColor ().ToArgb ();
		}

	}
}
