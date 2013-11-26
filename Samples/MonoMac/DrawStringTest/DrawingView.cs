
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;

namespace DrawStringTest
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
					font = new Font("Lucida Grande",20, FontStyle.Italic | FontStyle.Bold | FontStyle.Underline);
				return font;
			}
			set 
			{
				font = value;
			}
		}
		#endregion

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			g.Clear(backColor);
//			PointF point = new PointF(50,50);
//			var rect = new Rectangle(50,50,20,20);
//			//g.RotateTransform(-20);
//			//g.TranslateTransform(5,5);
//			var size = g.MeasureString("A", this.Font);
//			rect.Height = (int)size.Height;
//			rect.Width = (int)size.Width*20;
//
//			int nextLine = (int)(size.Height * 1.2);
//
//			g.DrawRectangle(Pens.Red, rect);
//			g.DrawString("Test2 without line feed", this.Font, Brushes.Blue, point);
//
//			StringFormat format = new StringFormat();
//			format.Alignment = StringAlignment.Center;
//
//			rect.Y += nextLine;
//			rect.Height = (int)size.Height;
//			rect.Width = (int)size.Width*20;
//			g.DrawRectangle(Pens.Red, rect);
//			g.DrawString("Test2 Centered", this.Font, Brushes.Blue, rect, format);
//
//			format.Alignment = StringAlignment.Far;
//			rect.Y += nextLine;
//			rect.Height = (int)size.Height;
//			rect.Width = (int)size.Width*20;
//			g.DrawRectangle(Pens.Red, rect);
//			g.DrawString("Test2 Far", this.Font, Brushes.Blue, rect, format);
//
//			format.Alignment = StringAlignment.Far;
//			rect.Y += nextLine;
//			rect.Height = (int)size.Height;
//			rect.Width = (int)size.Width*3;
//			g.DrawRectangle(Pens.Red, rect);
//			g.DrawString("Test2 Far", this.Font, Brushes.Blue, rect, format);
//

			AvailableFonts ();
			//PrivateFonts ();
			g.Dispose();
		}

		private void AvailableFonts()
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

		}

		private void PrivateFonts()
		{
			var privateFonts = new PrivateFontCollection ();

			privateFonts.AddFontFile ("Paint Boy.ttf");

			foreach ( FontFamily ff in privateFonts.Families )
			{
				Console.WriteLine(ff.ToString());
				// Something like this would be nice, but AFAIK nothing similar exists
				/*												
	foreach ( FontStyle style in ff.Styles )
		Console.WriteLine(style.ToString());
	*/
			}
		}


		void CreatePrivateFontCollection(Graphics g)
		{

			PointF pointF = new PointF(10, 0);
			SolidBrush solidBrush = new SolidBrush(Color.Black);

			int count = 0;
			string familyName = "";
			string familyNameAndStyle;
			FontFamily[] fontFamilies;
			PrivateFontCollection privateFontCollection = new PrivateFontCollection();

			// Add three font files to the private collection.

			var path = Environment.ExpandEnvironmentVariables("%SystemRoot%\\Fonts\\");

			privateFontCollection.AddFontFile(System.IO.Path.Combine(path,"Arial.ttf"));
			privateFontCollection.AddFontFile(System.IO.Path.Combine(path,"CourBI.ttf"));
			//privateFontCollection.AddFontFile(System.IO.Path.Combine(path, "Courier New.ttf"));
			privateFontCollection.AddFontFile(System.IO.Path.Combine(path, "TimesBD.ttf"));

			// Get the array of FontFamily objects.
			fontFamilies = privateFontCollection.Families;

			// How many objects in the fontFamilies array?
			count = fontFamilies.Length;

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
						16,
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
						16,
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
						16,
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
						16,
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
						16,
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
						16,
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