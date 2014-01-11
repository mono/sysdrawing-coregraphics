using System;

#if MONOMAC
namespace System.DrawingNative  {
#else
namespace System.Drawing {
	#endif
	public class Locale
	{
		public static string GetText (string format)
		{
			return format;
		}
		
		public static string GetText (string format, params object [] args)
		{
			return string.Format (format, args);
		}
	}
}

