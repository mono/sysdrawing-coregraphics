
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;

namespace BitmapTests
{
	public partial class DrawingView : MonoMac.AppKit.NSView
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
		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion

		Font Font = new System.Drawing.Font("Helvetica",12);

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();

			g.Clear(Color.Wheat);

			var mainBundle = NSBundle.MainBundle;
			var filePath = mainBundle.PathForResource("CocoaMono","png");

			var bitmap = Image.FromFile(filePath);

			filePath = mainBundle.PathForResource("tiger-Q300","png");

			var tiger = Image.FromFile(filePath);

			using (var ig = Graphics.FromImage(bitmap)) 
			{
				//ig.Clear(Color.Red);
				var pen = new Pen (Brushes.Yellow, 20);
				var rec = new SizeF (200, 200);
				var recp = new PointF (bitmap.Width - rec.Width - pen.Width / 2, bitmap.Height - rec.Height - pen.Width / 2);
				ig.DrawEllipse (pen, new RectangleF (recp, rec));
			}

			g.DrawImage(bitmap, new Point(50,50));
			g.DrawImage(tiger, new Point(200,200));

			// To test Save uncomment the following lines
//			var destopDirectory = Environment.GetFolderPath (Environment.SpecialFolder.DesktopDirectory);
//			var finalPath = System.IO.Path.Combine (destopDirectory, "cocoa-monowithcircle.jpg");
//
//			bitmap.Save(finalPath);

			using (SolidBrush brush = new SolidBrush(BACKCOLOR))
			{
				Image pic = GetCircleImage(); //get circle image
				Size newSize = new Size(pic.Size.Width * _scale, pic.Size.Height * _scale);//calculate new size of circle
				g.FillEllipse(brush, new Rectangle(_circleLocation, newSize));//draw the shape background
				g.DrawImage(pic, new Rectangle(_circleLocation, newSize));//draw the hatch style
			}
			
			g.Dispose();
			
		}
		
		/// <summary>
		/// Get the initial image
		/// </summary>
		/// <returns></returns>
		private Image GetCircleImage()
		{
			if (_circleImage == null)
			{
				//draw the initial image programmatically
				_circleImage = new Bitmap(WIDTH, HEIGHT);
				Graphics g = Graphics.FromImage(_circleImage);
				
				//draw the shape hatch style, not draw backgound
				using (HatchBrush brush = new HatchBrush(HatchStyle.Wave, FORECOLOR, Color.Transparent))
				{
					g.FillEllipse(brush, new Rectangle(Point.Empty, new Size(WIDTH, HEIGHT)));
				}
				g.Dispose();
			}
			
			return _circleImage;
		}
		
		private Image _circleImage = null;
		private Point _circleLocation = new Point(0, 0);
		private int _scale = 4;
		
		private const int HEIGHT = 100;
		private const int WIDTH = 100;
		
		private Color BACKCOLOR = Color.LightCoral;
		private Color FORECOLOR = Color.Blue;		
	}
}

