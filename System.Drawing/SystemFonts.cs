using System;
using Foundation;


namespace System.Drawing {

	public sealed partial class SystemFonts {
		static NSString dispatcher = new NSString ("");
		static SystemFonts ()
		{
		}

		public static Font GetFontByName (string systemFontName)
		{
			if (systemFontName == "CaptionFont")
				return CaptionFont;

			if (systemFontName == "DefaultFont")
				return DefaultFont;

			if (systemFontName == "DialogFont")
				return DialogFont;	

			if (systemFontName == "IconTitleFont")
				return IconTitleFont;

			if (systemFontName == "MenuFont")
				return MenuFont;

			if (systemFontName == "MessageBoxFont")
				return MessageBoxFont;

			if (systemFontName == "SmallCaptionFont")
				return SmallCaptionFont;

			if (systemFontName == "StatusFont")
				return StatusFont;			

			return null;
		}

		static Font captionFont;
		static Font defaultFont;
		static Font dialogFont;
		static Font iconTitleFont;
		static Font menuFont;
		static Font messageBoxFont;
		static Font smallCaptionFont;
		static Font statusFont;	
	}
}
