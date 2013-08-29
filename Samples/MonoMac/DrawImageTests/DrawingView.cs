
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using MonoMac.Foundation;
using MonoMac.AppKit;

namespace DrawImageTests
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

		Image bmp;
		Image bmp2;

		Rectangle src = new Rectangle(0, 0, 50, 50);
		RectangleF srcF = new Rectangle(0, 0, 50, 50);
		Rectangle dst = new Rectangle(170, 170, 100, 100);
		RectangleF dstF = new Rectangle(270, 270, 100, 100);

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = new Graphics();
			g.InterpolationMode = InterpolationMode.NearestNeighbor;

			g.Clear(Color.Wheat);

			var mainBundle = NSBundle.MainBundle;
			var filePath = mainBundle.PathForResource("bitmap25","png");

			var bmp = Image.FromFile(filePath);

			filePath = mainBundle.PathForResource("bitmap50","png");

			var bmp2 = Image.FromFile(filePath);


			//g.DrawImage(bmp, new Point[]{new Point(170,10), new Point(250,0), new Point(100,100)}, src, GraphicsUnit.Pixel );
			//g.DrawImage(bmp, new PointF[]{new PointF(70,10), new PointF(150,0), new PointF(10,100)}, srcF, GraphicsUnit.Pixel );
			// Create parallelogram for drawing image.
			Point ulCorner = new Point(110, 100);
			Point urCorner = new Point(350, 100);
			Point llCorner = new Point(100, 200);
			Point[] destPara = {ulCorner, urCorner, llCorner};

			g.DrawImage(bmp2, destPara);

			//g.DrawImage (bmp, dst);


			g.Dispose();
			
		}
	}
}

