
using System;
using System.Collections.Generic;
using System.Linq;
using System.DrawingNative;
using System.DrawingNative.Drawing2D;
using System.DrawingNative.Text;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace FontTest
{
	public partial class DrawingView : MonoMac.AppKit.NSView, Form
	{

		#region Constructors
		
		// Called when created from unmanaged code
		public DrawingView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public DrawingView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
			BackColor = Color.Wheat;
			// Set Form1 size:
			//			this.Width = 350;
			//			this.Height = 300;

		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion
		#region Form interface
		public Rectangle ClientRectangle 
		{
			get {
				return new Rectangle((int)Bounds.X,
				                     (int)Bounds.Y,
				                     (int)Bounds.Width,
				                     (int)Bounds.Height);
			}
		}

		Color backColor = Color.White;
		public Color BackColor 
		{
			get {
				return backColor;
			}
			
			set {
				backColor = value;
			}
		}

		Font font;
		public Font Font
		{
			get {
				if (font == null)
					font = new Font(FontFamily.GenericSansSerif,20, FontStyle.Italic | FontStyle.Bold | FontStyle.Underline);
				return font;
			}
			set 
			{
				font = value;
			}
		}
		#endregion

		Font anyKeyFont = new Font("Chalkduster", 18, FontStyle.Bold);
		Font clipFont = new Font(FontFamily.GenericSansSerif,12, FontStyle.Bold);


		int currentView = 0;
		int totalViews = 4;

		public override bool AcceptsFirstResponder ()
		{
			return true;
		}

		public override void KeyDown (NSEvent theEvent)
		{
			currentView++;
			currentView %= totalViews;
			//Console.WriteLine("Current View: {0}", currentView);
			this.NeedsDisplay = true;
		}

		string title = string.Empty;

		public override void DrawRect (RectangleF dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			g.Clear(backColor);

			switch (currentView) 
			{
			case 0:
				AvailableFonts (g);
				break;
			case 1:
				PrivateFonts (g);
				break;
			case 2:
				CreatePrivateFontCollection (g);
				break;
			case 3:
				ObtainFontMetrics (g);
				break;

			}

			g.ResetTransform ();
			Brush sBrush = Brushes.Black;

			if (!g.IsClipEmpty) 
			{
			var clipPoint = PointF.Empty;
			var clipString = string.Format("Clip-{0}", g.ClipBounds);
			g.ResetClip ();
			var clipSize = g.MeasureString(clipString, clipFont);
			clipPoint.X = (ClientRectangle.Width / 2) - (clipSize.Width / 2);
			clipPoint.Y = 5;
			g.DrawString(clipString, clipFont, sBrush, clipPoint );

			}

			var anyKeyPoint = PointF.Empty;
			var anyKey = "Press any key to continue.";
			var anyKeySize = g.MeasureString(anyKey, anyKeyFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y = ClientRectangle.Height - (anyKeySize.Height + 10);
			g.DrawString(anyKey, anyKeyFont, sBrush, anyKeyPoint );

			anyKeySize = g.MeasureString(title, anyKeyFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y -= anyKeySize.Height;
			g.DrawString(title, anyKeyFont, sBrush, anyKeyPoint );

			g.Dispose();
		}



		void ObtainFontMetrics(Graphics g)
		{
			string infoString = "";  // enough space for one line of output 
			int ascent;             // font family ascent in design units 
			float ascentPixel;      // ascent converted to pixels 
			int descent;            // font family descent in design units 
			float descentPixel;     // descent converted to pixels 
			int lineSpacing;        // font family line spacing in design units 
			float lineSpacingPixel; // line spacing converted to pixels

			FontStyle fontStyle = FontStyle.Regular; 
			//fontStyle = FontStyle.Italic | FontStyle.Bold;
			FontFamily fontFamily = new FontFamily("arial");
			//fontFamily = FontFamily.GenericSansSerif;

			Font font = new Font(
				fontFamily,
				16, fontStyle,
				GraphicsUnit.Pixel);
			PointF pointF = new PointF(10, 10);
			SolidBrush solidBrush = new SolidBrush(Color.Black);

			// Display the font size in pixels.
			infoString = "font family : " + font.FontFamily.Name + " " + fontStyle + ".";
			g.DrawString(infoString, font, solidBrush, pointF);

			// Move down one line.
			pointF.Y += font.Height;

			// Display the font size in pixels.
			infoString = "font.Size returns " + font.Size + ".";
			g.DrawString(infoString, font, solidBrush, pointF);

			// Move down one line.
			pointF.Y += font.Height;

			// Display the font family em height in design units.
			infoString = "fontFamily.GetEmHeight() returns " +
			             fontFamily.GetEmHeight(fontStyle) + ".";
			g.DrawString(infoString, font, solidBrush, pointF);

			// Move down two lines.
			pointF.Y += 2 * font.Height;

			// Display the ascent in design units and pixels.
			ascent = fontFamily.GetCellAscent(fontStyle);

			// 14.484375 = 16.0 * 1854 / 2048
			ascentPixel =
				font.Size * ascent / fontFamily.GetEmHeight(fontStyle);
			infoString = "The ascent is " + ascent + " design units, " + ascentPixel +
			             " pixels.";
			g.DrawString(infoString, font, solidBrush, pointF);

			// Move down one line.
			pointF.Y += font.Height;

			// Display the descent in design units and pixels.
			descent = fontFamily.GetCellDescent(fontStyle);

			// 3.390625 = 16.0 * 434 / 2048
			descentPixel =
				font.Size * descent / fontFamily.GetEmHeight(fontStyle);
			infoString = "The descent is " + descent + " design units, " +
			             descentPixel + " pixels.";
			g.DrawString(infoString, font, solidBrush, pointF);

			// Move down one line.
			pointF.Y += font.Height;

			// Display the line spacing in design units and pixels.
			lineSpacing = fontFamily.GetLineSpacing(fontStyle);

			// 18.398438 = 16.0 * 2355 / 2048
			lineSpacingPixel =
				font.Size * lineSpacing / fontFamily.GetEmHeight(fontStyle);
			infoString = "The line spacing is " + lineSpacing + " design units, " +
			             lineSpacingPixel + " pixels.";
			g.DrawString(infoString, font, solidBrush, pointF);

			title = "ObtainFontMetrics";
		}


		private void AvailableFonts(Graphics g)
		{
			var installedFonts = new InstalledFontCollection ();

			foreach ( FontFamily ff in installedFonts.Families )
			{
				Console.WriteLine(ff.ToString());

				foreach (var style in Enum.GetValues(typeof(FontStyle)) )
				{
					if (ff.IsStyleAvailable((FontStyle)style))
						Console.WriteLine(ff.ToString() + " - " + (FontStyle)style);
				}
			}

			var format = StringFormat.GenericDefault;
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			g.DrawString ("Please see console.", anyKeyFont, Brushes.Blue, ClientRectangle,format);

			title = "AvailableFonts";
		}

		private void PrivateFonts(Graphics g)
		{
			var privateFonts = new PrivateFontCollection ();

			privateFonts.AddFontFile ("A Damn Mess.ttf");
			privateFonts.AddFontFile ("Abberancy.ttf");
			privateFonts.AddFontFile ("Abduction.ttf");
			privateFonts.AddFontFile ("American Typewriter.ttf");
			privateFonts.AddFontFile ("Paint Boy.ttf");

			foreach ( FontFamily ff in privateFonts.Families )
			{
				Console.WriteLine(ff.ToString());
				foreach (var style in Enum.GetValues(typeof(FontStyle)) )
				{
					if (ff.IsStyleAvailable((FontStyle)style))
						Console.WriteLine(ff.ToString() + " - " + (FontStyle)style);
				}
			}

			var format = StringFormat.GenericDefault;
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			g.DrawString ("Please see console.", anyKeyFont, Brushes.Blue, ClientRectangle,format);

			title = "PrivateFontCollection";

		}


		void CreatePrivateFontCollection(Graphics g)
		{

			PointF pointF = new PointF(10, 20);
			SolidBrush solidBrush = new SolidBrush(Color.Black);

			int count = 0;
			string familyName = "";
			string familyNameAndStyle;
			FontFamily[] fontFamilies;
			PrivateFontCollection privateFontCollection = new PrivateFontCollection();

			// Add three font files to the private collection.

//			var path = Environment.ExpandEnvironmentVariables("%SystemRoot%\\Fonts\\");
//
//			privateFontCollection.AddFontFile(System.IO.Path.Combine(path,"Arial.ttf"));
//			privateFontCollection.AddFontFile(System.IO.Path.Combine(path,"CourBI.ttf"));
//			//privateFontCollection.AddFontFile(System.IO.Path.Combine(path, "Courier New.ttf"));
//			privateFontCollection.AddFontFile(System.IO.Path.Combine(path, "TimesBD.ttf"));
			privateFontCollection.AddFontFile ("A Damn Mess.ttf");
			privateFontCollection.AddFontFile ("Abberancy.ttf");
			privateFontCollection.AddFontFile ("Abduction.ttf");
			privateFontCollection.AddFontFile ("American Typewriter.ttf");
			privateFontCollection.AddFontFile ("Paint Boy.ttf");


			// Get the array of FontFamily objects.
			fontFamilies = privateFontCollection.Families;

			// How many objects in the fontFamilies array?
			count = fontFamilies.Length;

			var fontSize = 20;

			// Display the name of each font family in the private collection 
			// along with the available styles for that font family. 
			for (int j = 0; j < count; ++j)
			{
				// Get the font family name.
				familyName = fontFamilies[j].Name;

				// Is the regular style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Regular))
				{
					familyNameAndStyle = "";
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Regular";

					Font regFont = new Font(
						familyName,
						fontSize,
						FontStyle.Regular,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						regFont,
						solidBrush,
						pointF);

					pointF.Y += regFont.Height;
				}

				// Is the bold style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Bold))
				{
					familyNameAndStyle = "";
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Bold";

					Font boldFont = new Font(
						familyName,
						fontSize,
						FontStyle.Bold,
						GraphicsUnit.Pixel);

					g.DrawString(familyNameAndStyle, boldFont, solidBrush, pointF);

					pointF.Y += boldFont.Height;
				}
				// Is the italic style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Italic))
				{
					familyNameAndStyle = "";
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Italic";

					Font italicFont = new Font(
						familyName,
						fontSize,
						FontStyle.Italic,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						italicFont,
						solidBrush,
						pointF);

					pointF.Y += italicFont.Height;
				}

				// Is the bold italic style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Italic) &&
					fontFamilies[j].IsStyleAvailable(FontStyle.Bold))
				{
					familyNameAndStyle = "";
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + "BoldItalic";

					Font italicFont = new Font(
						familyName,
						26,
						FontStyle.Italic | FontStyle.Bold,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						italicFont,
						solidBrush,
						pointF);

					pointF.Y += italicFont.Height;
				}
				// Is the underline style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Underline))
				{
					familyNameAndStyle = "";
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Underline";

					Font underlineFont = new Font(
						familyName,
						fontSize,
						FontStyle.Underline,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						underlineFont,
						solidBrush,
						pointF);

					pointF.Y += underlineFont.Height;
				}

				// Is the strikeout style available? 
				if (fontFamilies[j].IsStyleAvailable(FontStyle.Strikeout))
				{
					familyNameAndStyle = "";
					familyNameAndStyle = familyNameAndStyle + familyName;
					familyNameAndStyle = familyNameAndStyle + " Strikeout";

					Font strikeFont = new Font(
						familyName,
						fontSize,
						FontStyle.Strikeout,
						GraphicsUnit.Pixel);

					g.DrawString(
						familyNameAndStyle,
						strikeFont,
						solidBrush,
						pointF);

					pointF.Y += strikeFont.Height;
				}

				// Separate the families with white space.
				pointF.Y += 10;

			} // for

			title = "CreatePrivateFontCollection";
		}


//				public override bool IsFlipped {
//					get {
//						//return base.IsFlipped;
//						return true;
//					}
//				}
	}


}

public interface Form
{
	Rectangle ClientRectangle { get; }
	
	Color BackColor { get; set; } 
	
	Font Font { get; set; }
}