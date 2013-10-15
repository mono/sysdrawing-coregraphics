
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;

namespace GraphicsPathTests
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


			// We need to set these two view properites so that when we clear an infinite region
			// it actually clears to the background color.
			// If not set this may set the rectangle to Black depending on the context
			// passed.  
			this.WantsLayer = true;
			Layer.BackgroundColor = new CGColor (0, 0, 0, 0);


			var mainBundle = NSBundle.MainBundle;
//			var filePath = mainBundle.PathForResource("bitmap50","png");
//			bmp = Bitmap.FromFile(filePath);
//
//			filePath = mainBundle.PathForResource("bitmap25","png");
//			bmp2 = Bitmap.FromFile(filePath);
		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion

		Font anyKeyFont = new Font("Chalkduster", 18, FontStyle.Bold);
		Font clipFont = new Font("Helvetica",12, FontStyle.Bold);

		Image bmp;
		Image bmp2;


		Rectangle pathRect1 = new Rectangle(50, 50, 100, 100);
		RectangleF pathRectF1 = new RectangleF(110, 60, 100, 100);
		Rectangle pathRect2 = new Rectangle(110, 60, 100, 100);
		RectangleF pathRectF2 = new RectangleF(110, 60, 100, 100);
		Rectangle pathRect3 = new Rectangle(50, 50, 50, 100);
		RectangleF pathRectF3 = new RectangleF(50, 50, 50, 100);
		Rectangle pathRect4 = new Rectangle(110, 60, 100, 50);
		RectangleF pathRectF4 = new RectangleF(110, 60, 100, 50);


		int currentView = 45;
		int totalViews = 50;

		public Rectangle ClientRectangle 
		{
			get {
				return new Rectangle((int)Bounds.X,
				                     (int)Bounds.Y,
				                     (int)Bounds.Width,
				                     (int)Bounds.Height);
			}
		}

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

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();
			g.InterpolationMode = InterpolationMode.NearestNeighbor;

			g.Clear(Color.White);
			//g.SmoothingMode = SmoothingMode.HighQuality;
			switch (currentView) 
			{
			case 0:
				AddArcRectangle (g);
				break;
			case 1:
				AddArcRectangleF (g);
				break;
			case 2:
				AddArc1 (g);
				break;
			case 3:
				AddArc2 (g);
				break;
			case 4:
				AddBezier1 (g);
				break;
			case 5:
				AddBezier2 (g);
				break;
			case 6:
				AddBezier3 (g);
				break;
			case 7:
				AddBezier4 (g);
				break;
			case 8:
				AddBeziers1 (g);
				break;
			case 9:
				AddBeziers2 (g);
				break;
			case 10:
				AddClosedCurve1 (g);
				break;
			case 11:
				AddClosedCurve2 (g);
				break;
			case 12:
				AddCurve1 (g);
				break;
			case 13:
				AddCurve2 (g);
				break;
			case 14:
				AddEllipse1 (g);
				break;
			case 15:
				AddEllipse2 (g);
				break;
			case 16:
				AddEllipse3 (g);
				break;
			case 17:
				AddEllipse4 (g);
				break;
			case 18:
				AddLine1 (g);
				break;
			case 19:
				AddLine2 (g);
				break;
			case 20:
				AddLine3 (g);
				break;
			case 21:
				AddLine4 (g);
				break;
			case 22:
				AddLines1 (g);
				break;
			case 23:
				AddLines2 (g);
				break;
			case 24:
				AddPie1 (g);
				break;
			case 25:
				AddPie2 (g);
				break;
			case 26:
				AddPie3 (g);
				break;
			case 27:
				AddPolygon1 (g);
				break;
			case 28:
				AddPolygon2 (g);
				break;
			case 29:
				AddRectangle1 (g);
				break;
			case 30:
				AddRectangle2 (g);
				break;
			case 31:
				AddRectangles1 (g);
				break;
			case 32:
				AddRectangles2 (g);
				break;
			case 33:
				AddPath1 (g);
				break;
			case 34:
				CloseAllFigures1 (g);
				break;
			case 35:
				GetLastPoint1 (g);
				break;
			case 36:
				GetLastPoint2 (g);
				break;
			case 37:
				Reset1 (g);
				break;
			case 38:
				Reverse1 (g);
				break;
			case 39:
				SetMarkers1 (g);
				break;
			case 40:
				SetMarkers2 (g);
				break;
			case 41:
				ClearMarkers1 (g);
				break;
			case 42:
				ClearMarkers2 (g);
				break;
			case 43:
				TransformPath (g);
				break;
			case 44:
				StartFigure (g);
				break;
			case 45:
				GetBounds (g);
				break;
			case 46:
				Flatten1 (g);
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

		private void AddArcRectangle (Graphics g)
		{

			// Create a GraphicsPath object.
			GraphicsPath myPath = new GraphicsPath();

			// Set up and call AddArc, and close the figure.
			myPath.StartFigure();
			myPath.AddArc(pathRect3, 0, 180);
			myPath.CloseFigure();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Red, 3), myPath);

			title = "AddArcRectangle";

		}

		private void AddArcRectangleF (Graphics g)
		{

			// Create a GraphicsPath object.
			GraphicsPath myPath = new GraphicsPath();

			// Set up and call AddArc, and close the figure.
			myPath.StartFigure();
			myPath.AddArc(pathRectF3, 0, 180);
			myPath.CloseFigure();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Red, 3), myPath);

			title = "AddArcRectangleF";

		}

		private void AddArc1 (Graphics g)
		{

			// Create a GraphicsPath object.
			GraphicsPath myPath = new GraphicsPath();

			// Set up and call AddArc, and close the figure.
			myPath.StartFigure();
			myPath.AddArc(110,60,50,100, 0, 180);
			myPath.CloseFigure();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Red, 3), myPath);

			title = "AddArcInt32";

		}

		private void AddArc2 (Graphics g)
		{

			// Create a GraphicsPath object.
			GraphicsPath myPath = new GraphicsPath();

			// Set up and call AddArc, and close the figure.
			myPath.StartFigure();
			myPath.AddArc(110.5f,60.5f,50.5f,100.5f, 0, 180);
			myPath.CloseFigure();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Red, 3), myPath);

			title = "AddArcSingle";

		}

		private void AddBezier1(Graphics g)
		{

			// Create a new Path.
			GraphicsPath myPath = new GraphicsPath();

			// Call AddBezier.
			myPath.StartFigure();
			myPath.AddBezier(50, 50, 70, 0, 100, 120, 150, 50);

			// Close the curve.
			myPath.CloseFigure();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Red, 2), myPath);

			title = "AddBezierInt32";
		}

		private void AddBezier2(Graphics g)
		{

			// Create a new Path.
			GraphicsPath myPath = new GraphicsPath();

			// Call AddBezier.
			myPath.StartFigure();
			myPath.AddBezier(new Point(50, 50), new Point(70, 0), new Point(100, 120), new Point(150, 50));

			// Close the curve.
			myPath.CloseFigure();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Red, 2), myPath);

			title = "AddBezierPoint";
		}

		private void AddBezier3(Graphics g)
		{

			// Create a new Path.
			GraphicsPath myPath = new GraphicsPath();

			// Call AddBezier.
			myPath.StartFigure();
			myPath.AddBezier(new PointF(50.5f, 50.5f), new PointF(70.5f, 0), new PointF(100.5f, 120.5f), new PointF(150.5f, 50.5f));

			// Close the curve.
			myPath.CloseFigure();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Red, 2), myPath);

			title = "AddBezierPointF";
		}

		private void AddBezier4(Graphics g)
		{

			// Create a new Path.
			GraphicsPath myPath = new GraphicsPath();

			// Call AddBezier.
			myPath.StartFigure();
			myPath.AddBezier(50.5f, 50.5f, 70.5f, 0, 100.5f, 120.5f, 150.5f, 50.5f);

			// Close the curve.
			myPath.CloseFigure();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Red, 2), myPath);

			title = "AddBezierSingle";
		}

		private void AddBeziers1(Graphics g)
		{

			// Adds two Bezier curves.
			Point[] myArray =
			{
				new Point(20, 100),
				new Point(40, 75),
				new Point(60, 125),
				new Point(80, 100),
				new Point(100, 50),
				new Point(120, 150),
				new Point(140, 100)
			};

			// Create the path and add the curves.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddBeziers(myArray);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddBeziersPoint";
		}

		private void AddBeziers2(Graphics g)
		{

			// Adds two Bezier curves.
			PointF[] myArray =
			{
				new PointF(20, 100),
				new PointF(40, 75),
				new PointF(60, 125),
				new PointF(80, 100),
				new PointF(100, 50),
				new PointF(120, 150),
				new PointF(140, 100)
			};

			// Create the path and add the curves.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddBeziers(myArray);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddBeziersPointF";
		}

		
		private void AddClosedCurve1(Graphics g)
		{

			// Creates a symetrical, closed curve.
			Point[] myArray =
			{
				new Point(20,100),
				new Point(40,150),
				new Point(60,125),
				new Point(40,100),
				new Point(60,75),
				new Point(40,50)
			};

			// Create a new path and add curve.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddClosedCurve(myArray, 0.5f);
			Pen myPen = new Pen(Color.Black, 2);

			// Draw the path to screen.
			g.DrawPath(myPen, myPath);

			title = "AddClosedCurvePoint";
		}

		private void AddClosedCurve2(Graphics g)
		{

			// Creates a symetrical, closed curve.
			PointF[] myArray =
			{
				new PointF(20,100),
				new PointF(40,150),
				new PointF(60,125),
				new PointF(40,100),
				new PointF(60,75),
				new PointF(40,50)
			};

			// Create a new path and add curve.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddClosedCurve(myArray);
			Pen myPen = new Pen(Color.Black, 2);

			// Draw the path to screen.
			g.DrawPath(myPen, myPath);

			title = "AddClosedCurvePointF";
		}

		private void AddCurve1(Graphics g)
		{

			// Create some points.
			Point point1 = new Point(20, 20);
			Point point2 = new Point(40, 0);
			Point point3 = new Point(60, 40);
			Point point4 = new Point(80, 20);

			// Create an array of the points.
			Point[] curvePoints = {point1, point2, point3, point4};

			// Create a GraphicsPath object and add a curve.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddCurve(curvePoints, 0, 3, 0.8f);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddCurvePoint";
		}

		private void AddCurve2(Graphics g)
		{

			// Create some points.
			PointF point1 = new PointF(20, 20);
			PointF point2 = new PointF(40, 0);
			PointF point3 = new PointF(60, 40);
			PointF point4 = new PointF(80, 20);

			// Create an array of the points.
			PointF[] curvePoints = {point1, point2, point3, point4};

			// Create a GraphicsPath object and add a curve.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddCurve(curvePoints, 0.8f);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddCurvePointF";
		}

		private void AddEllipse1(Graphics g)
		{

			// Create a path and add an ellipse.
			Rectangle myEllipse = new Rectangle(20, 20, 100, 50);
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddEllipse(myEllipse);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);
			title = "AddEllipseRectangle";
		}

		private void AddEllipse2(Graphics g)
		{

			// Create a path and add an ellipse.
			RectangleF myEllipse = new RectangleF(20.5f, 20.5f, 100.5f, 50.5f);
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddEllipse(myEllipse);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);
			title = "AddEllipseRectangleF";
		}

		private void AddEllipse3(Graphics g)
		{

			// Create a path and add an ellipse.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddEllipse(20, 20, 100, 50);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);
			title = "AddEllipseInt32";
		}

		
		private void AddEllipse4(Graphics g)
		{

			// Create a path and add an ellipse.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddEllipse(20.5f, 20.5f, 100.5f, 50.5f);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);
			title = "AddEllipseSingle";
		}

		private void AddLine1(Graphics g)
		{

			//Create a path and add a symetrical triangle using AddLine.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(30, 30, 60, 60);
			myPath.AddLine(60, 60, 0, 60);
			myPath.AddLine(0, 60, 30, 30);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddLineInt32";
		}

		private void AddLine2(Graphics g)
		{

			//Create a path and add a symetrical triangle using AddLine.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(30.5f, 30.5f, 60.5f, 60.5f);
			myPath.AddLine(60.5f, 60.5f, 0, 60.5f);
			myPath.AddLine(0, 60.5f, 30.5f, 30.5f);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddLineSingle";
		}

		private void AddLine3(Graphics g)
		{

			//Create a path and add a symetrical triangle using AddLine.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(new Point(30, 30), new Point(60, 60));
			myPath.AddLine(new Point(60, 60), new Point(0, 60));
			myPath.AddLine(new Point(0, 60), new Point(30, 30));

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);
			
			title = "AddLinePoint";
		}

		private void AddLine4(Graphics g)
		{

			//Create a path and add a symetrical triangle using AddLine.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(new PointF(30.5f, 30.5f), new PointF(60.5f, 60.5f));
			myPath.AddLine(new PointF(60.5f, 60.5f), new PointF(0, 60.5f));
			myPath.AddLine(new PointF(0, 60.5f), new PointF(30.5f, 30.5f));

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);
			title = "AddLinePointF";
		}
		
		private void AddLines1(Graphics g)
		{

			
			// Create a symetrical triangle using an array of points.
			Point[] myArray =
			{
				new Point(30,30),
				new Point(60,60),
				new Point(0,60),
				new Point(30,30)
			};

			//Create a path and add lines.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLines(myArray);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddLinesPoint";
		}
		
		private void AddLines2(Graphics g)
		{


			// Create a symetrical triangle using an array of points.
			PointF[] myArray =
			{
				new PointF(30,30),
				new PointF(60,60),
				new PointF(0,60),
				new PointF(30,30)
			};

			//Create a path and add lines.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLines(myArray);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddLinesPointF";
		}

		private void AddPie1(Graphics g)
		{

			// Create a pie slice of a circle using the AddPie method.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddPie(20, 20, 70, 70, -45, 90);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddPieInt32";
		}

		private void AddPie2(Graphics g)
		{

			// Create a pie slice of a circle using the AddPie method.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddPie(20.5f, 20.5f, 70.5f, 70.5f, -45, 90);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);
			
			title = "AddPieSingle";
		}

		private void AddPie3(Graphics g)
		{

			// Create a pie slice of a circle using the AddPie method.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddPie(new Rectangle(20, 20, 70, 70), -45, 90);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddPieRectangle";
		}

		private void AddPolygon1(Graphics g)
		{

			// Create an array of points.
			Point[] myArray =
			{
				new Point(23, 20),
				new Point(40, 10),
				new Point(57, 20),
				new Point(50, 40),
				new Point(30, 40)
			};

			// Create a GraphicsPath object and add a polygon.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddPolygon(myArray);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddPolygonPoint";
		}	
		
		private void AddPolygon2(Graphics g)
		{

			// Create an array of points.
			PointF[] myArray =
			{
				new PointF(23, 20),
				new PointF(40, 10),
				new PointF(57, 20),
				new PointF(50, 40),
				new PointF(30, 40)
			};

			// Create a GraphicsPath object and add a polygon.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddPolygon(myArray);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddPolygonPointF";
		}		
	
		private void AddRectangle1(Graphics g)
		{

			// Create a GraphicsPath object and add a rectangle to it.
			GraphicsPath myPath = new GraphicsPath();
			Rectangle pathRect = new Rectangle(20, 20, 100, 200);
			myPath.AddRectangle(pathRect);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);
			
			title = "AddRectangle";
		}
		
		private void AddRectangle2(Graphics g)
		{

			// Create a GraphicsPath object and add a rectangle to it.
			GraphicsPath myPath = new GraphicsPath();
			RectangleF pathRect = new RectangleF(20, 20, 100, 200);
			myPath.AddRectangle(pathRect);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddRectangleF";
		}

		private void AddRectangles1(Graphics g)
		{

			// Adds a pattern of rectangles to a GraphicsPath object.
			GraphicsPath myPath = new GraphicsPath();
			Rectangle[] pathRects =
			{
				new Rectangle(20,20,100,200),
				new Rectangle(40,40,120,220),
				new Rectangle(60,60,240,140)
			};
			myPath.AddRectangles(pathRects);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddRectanglesRectangle";

		}

		private void AddRectangles2(Graphics g)
		{

			// Adds a pattern of rectangles to a GraphicsPath object.
			GraphicsPath myPath = new GraphicsPath();
			RectangleF[] pathRects =
			{
				new RectangleF(20,20,100,200),
				new RectangleF(40,40,120,220),
				new RectangleF(60,60,240,140)
			};
			myPath.AddRectangles(pathRects);

			// Draw the path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddRectanglesRectangleF";

		}

		private void AddPath1(Graphics g)
		{

			// Create the first pathright side up triangle.
			Point[] myArray =
			{
				new Point(30,30),
				new Point(60,60),
				new Point(0,60),
				new Point(30,30)
			};
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLines(myArray);

			// Create the second pathinverted triangle.
			Point[] myArray2 =
			{
				new Point(30,30),
				new Point(0,0),
				new Point(60,0),
				new Point(30,30)
			};
			GraphicsPath myPath2 = new GraphicsPath();
			myPath2.AddLines(myArray2);

			// Add the second path to the first path.
			myPath.AddPath(myPath2,true);

			// Draw the combined path to the screen.
			Pen myPen = new Pen(Color.Black, 2);
			g.DrawPath(myPen, myPath);

			title = "AddPath";
		}

		private void CloseAllFigures1(Graphics g)
		{

			// Create a path containing several open-ended figures.
			GraphicsPath myPath = new GraphicsPath();
			myPath.StartFigure();
			myPath.AddLine(new Point(10, 10), new Point(150, 10));
			myPath.AddLine(new Point(150, 10), new Point(10, 150));
			myPath.StartFigure();
			myPath.AddArc(200, 200, 100, 100, 0, 90);
			myPath.StartFigure();
			Point point1 = new Point(300, 300);
			Point point2 = new Point(400, 325);
			Point point3 = new Point(400, 375);
			Point point4 = new Point(300, 400);
			Point[] points = {point1, point2, point3, point4};
			myPath.AddCurve(points);

			// Close all the figures.
			myPath.CloseAllFigures();

			// Draw the path to the screen.
			g.DrawPath(new Pen(Color.Black, 3), myPath);

			title = "CloseAllFigures";
		}

		private void GetLastPoint1(Graphics g)
		{
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(100, 100, 300, 100);
			PointF lastPoint = myPath.GetLastPoint();
			if(!lastPoint.IsEmpty)
			{
				string lastPointXString = lastPoint.X.ToString();
				string lastPointYString = lastPoint.Y.ToString();
				Console.WriteLine(lastPointXString + ", " + lastPointYString);
			}
			else
				Console.WriteLine("lastPoint is empty");

			// Draw the path to the screen.
			g.DrawPath(new Pen(Color.Black, 2), myPath);

			title = "GetLastPoint - Console";
		}

		
		private void GetLastPoint2(Graphics g)
		{
			GraphicsPath myPath = new GraphicsPath();

			try
			{
				PointF lastPoint = myPath.GetLastPoint();
				if(!lastPoint.IsEmpty)
				{
					string lastPointXString = lastPoint.X.ToString();
					string lastPointYString = lastPoint.Y.ToString();
					Console.WriteLine(lastPointXString + ", " + lastPointYString);
				}
				else
					Console.WriteLine("lastPoint is empty");
			}
			catch (ArgumentException ae) 
			{
				Console.WriteLine (ae.Message);
			}

			// Draw the path to the screen.
			g.DrawPath(new Pen(Color.Black, 2), myPath);

			title = "GetLastPointEmptyPath - Console";
		}

		public void Reset1(Graphics g)
		{
			Font myFont = new Font("Arial", 8);

			// Create a path and add a line, an ellipse, and an arc.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(new Point(0, 0), new Point(100, 100));
			myPath.AddEllipse(100, 100, 200, 250);
			myPath.AddArc(300, 250, 100, 100, 0, 90);

			// Draw the pre-reset path to the screen
			g.DrawPath (Pens.Blue, myPath);

			// Draw the pre-reset points array to the screen.
			DrawPoints1(g, myPath.PathPoints, 20);

			// Reset the path.
			myPath.Reset();

			// See if any points remain. 
			if(myPath.PointCount > 0)
			{

				// Draw the post-reset points array to the screen.
				DrawPoints1(g, myPath.PathPoints, 150);
			}
			else 

				// If there are no points, say so.
				g.DrawString("No Points",
				                      myFont,
				                      Brushes.Black,
				                      150,
				                      20);

			title = "Reset";
		} 
		//End GraphicsPathResetExample 

		// A helper function GraphicsPathResetExample uses to draw the points. 

		// to the screen. 
		public void DrawPoints1(Graphics g, PointF[] pathPoints, int xOffset)
		{
			int y = 20;
			Font myFont = new Font("Arial", 8);
			for(int i=0;i < pathPoints.Length; i++)
			{
				g.DrawString(pathPoints[i].X.ToString() + ", " +
				                      pathPoints[i].Y.ToString(),
				                      myFont,
				                      Brushes.Black,
				                      xOffset,
				                      y);
				y += 20;
			}
		} 
		// End DrawPoints

		public void Reverse1(Graphics g)
		{

			// Create a path and add a line, ellipse, and arc.
			GraphicsPath myPath = new GraphicsPath(FillMode.Winding);
			myPath.AddLine(new Point(0, 0), new Point(100, 100));
			myPath.AddEllipse(100, 100, 200, 250);
			myPath.AddArc(300, 250, 100, 100, 0, 90);

			g.DrawPath (Pens.Red, myPath);
			g.FillPath (Brushes.Red, myPath);

			var pathPoints = myPath.PathPoints;
			var pathTypes = myPath.PathTypes;

			Console.WriteLine ("Before reverse");
			for(int i=0;i < myPath.PathTypes.Length; i++)
			{
				Console.WriteLine ("{0} - {1},{2}", (PathPointType)pathTypes [i], pathPoints[i].X,pathPoints[i].Y);
			}

			// Draw the first set of points to the screen.
			DrawPoints2(g, myPath.PathPoints, 20);

			// Call GraphicsPath.Reverse.
			myPath.Reverse();

			g.DrawPath (Pens.Blue, myPath);
			g.FillPath (Brushes.Blue, myPath);

			pathPoints = myPath.PathPoints;
			pathTypes = myPath.PathTypes;


			Console.WriteLine ("After reverse");
			for(int i=0;i < myPath.PathTypes.Length; i++)
			{
				Console.WriteLine ("{0} - {1},{2}", (PathPointType)pathTypes [i], pathPoints[i].X,pathPoints[i].Y);
			}

			// Draw the reversed set of points to the screen.
			DrawPoints2(g, myPath.PathPoints, 150);

			title = "Reverse";
		}
		//End GraphicsPathReverseExample. 

		// A helper function GraphicsPathReverseExample is used to draw the 

		// points to the screen. 
		public void DrawPoints2(Graphics g, PointF[] pathPoints, int xOffset)
		{
			int y = 20;
			Font myFont = new Font("Arial", 8);
			for(int i=0;i < pathPoints.Length; i++)
			{
				g.DrawString(pathPoints[i].X.ToString() + ", " +
				                      pathPoints[i].Y.ToString(),
				                      myFont,
				                      Brushes.Black,
				                      xOffset,
				                      y);
				y += 20;
			}
		} 
		// End DrawPoints

		private void SetMarkers1(Graphics g)
		{

			// Create a path and set two markers.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(new Point(0, 0), new Point(50, 50));
			myPath.SetMarkers();
			Rectangle rect = new Rectangle(50, 50, 50, 50);
			myPath.AddRectangle(rect);
			myPath.SetMarkers();
			myPath.AddEllipse(100, 100, 100, 50);

			var pathPoints = myPath.PathPoints;
			var pathTypes = myPath.PathTypes;

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Black, 2), myPath);


			title = "SetMarkers";
		}

		private void SetMarkers2(Graphics g)
		{

			// Create a path and set two markers.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(new Point(0, 0), new Point(50, 50));
			myPath.SetMarkers();
			Rectangle rect = new Rectangle(50, 50, 50, 50);
			myPath.AddRectangle(rect);
			myPath.SetMarkers();
			myPath.AddEllipse(100, 100, 100, 50);

			var pathPoints = myPath.PathPoints;
			var pathTypes = myPath.PathTypes;

			Console.WriteLine("SetMarkers Before reverse");
			for (int i = 0; i < myPath.PathTypes.Length; i++)
			{
				Console.WriteLine("{0} - {1},{2}", (PathPointType)pathTypes[i], pathPoints[i].X, pathPoints[i].Y);
			}

			// Draw the path to screen.
			g.FillPath(Brushes.Red, myPath);
			g.DrawPath(new Pen(Color.Black, 2), myPath);

			// Draw the first set of points to the screen.
			DrawPoints2(g, myPath.PathPoints, 20);

			// Call GraphicsPath.Reverse.
			myPath.Reverse();

			pathPoints = myPath.PathPoints;
			pathTypes = myPath.PathTypes;


			Console.WriteLine("SetMarkers After reverse");
			for (int i = 0; i < myPath.PathTypes.Length; i++)
			{
				Console.WriteLine("{0} - {1},{2}", (PathPointType)pathTypes[i], pathPoints[i].X, pathPoints[i].Y);
			}

			// Draw the path to screen.
			g.FillPath(Brushes.CornflowerBlue, myPath);
			g.DrawPath(new Pen(Color.Black, 2), myPath);


			// Draw the reversed set of points to the screen.
			DrawPoints2(g, myPath.PathPoints, 150);

			title = "SetMarkersReverse";
		}

		
		private void ClearMarkers1(Graphics g)
		{

			// Create a path and set two markers.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(new Point(0, 0), new Point(50, 50));
			myPath.SetMarkers();
			Rectangle rect = new Rectangle(50, 50, 50, 50);
			myPath.AddRectangle(rect);
			myPath.SetMarkers();
			myPath.AddEllipse(100, 100, 100, 50);

			myPath.ClearMarkers ();

			// Draw the path to screen.
			g.DrawPath(new Pen(Color.Black, 2), myPath);


			title = "ClearMarkers";
		}

		private void ClearMarkers2(Graphics g)
		{

			// Create a path and set two markers.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddLine(new Point(0, 0), new Point(50, 50));
			myPath.SetMarkers();
			Rectangle rect = new Rectangle(50, 50, 50, 50);
			myPath.AddRectangle(rect);
			myPath.SetMarkers();
			myPath.AddEllipse(100, 100, 100, 50);

			var pathPoints = myPath.PathPoints;
			var pathTypes = myPath.PathTypes;

			Console.WriteLine("ClearMarkers Before reverse");
			for (int i = 0; i < myPath.PathTypes.Length; i++)
			{
				Console.WriteLine("{0} - {1},{2}", (PathPointType)pathTypes[i], pathPoints[i].X, pathPoints[i].Y);
			}

			// Draw the path to screen.
			g.FillPath(Brushes.Red, myPath);
			g.DrawPath(new Pen(Color.Black, 2), myPath);

			// Draw the first set of points to the screen.
			DrawPoints2(g, myPath.PathPoints, 20);

			// Call GraphicsPath.Reverse.
			myPath.Reverse();

			pathPoints = myPath.PathPoints;
			pathTypes = myPath.PathTypes;


			Console.WriteLine("ClearMarkers After reverse");
			for (int i = 0; i < myPath.PathTypes.Length; i++)
			{
				Console.WriteLine("{0} - {1},{2}", (PathPointType)pathTypes[i], pathPoints[i].X, pathPoints[i].Y);
			}

			// Call GraphicsPath.ClearMarkers.
			myPath.ClearMarkers();

			pathPoints = myPath.PathPoints;
			pathTypes = myPath.PathTypes;


			Console.WriteLine("ClearMarkers After Clear");
			for (int i = 0; i < myPath.PathTypes.Length; i++)
			{
				Console.WriteLine("{0} - {1},{2}", (PathPointType)pathTypes[i], pathPoints[i].X, pathPoints[i].Y);
			}
			// Draw the path to screen.
			g.FillPath(Brushes.CornflowerBlue, myPath);
			g.DrawPath(new Pen(Color.Black, 2), myPath);


			// Draw the reversed set of points to the screen.
			DrawPoints2(g, myPath.PathPoints, 150);

			title = "ClearMarkersReverse";
		}

		private void TransformPath(Graphics g)
		{

			// Create a path and add and ellipse.
			GraphicsPath myPath = new GraphicsPath();
			myPath.AddEllipse(50, 50, 100, 200);

			// Draw the starting position to screen.
			g.DrawPath(Pens.Black, myPath);

			// Move the ellipse 100 points to the right.
			Matrix translateMatrix = new Matrix();
			translateMatrix.Translate(100, 0);
			myPath.Transform(translateMatrix);

			// Draw the transformed ellipse to the screen.
			g.DrawPath(new Pen(Color.Red, 2), myPath);

			title = "TransformPath";

		}

		public void StartFigure(Graphics g)
		{

			// Create a GraphicsPath object.
			GraphicsPath myPath = new GraphicsPath();

			// First set of figures.
			myPath.StartFigure();
			myPath.AddArc(10, 10, 50, 50, 0, 270);
			myPath.AddLine(new Point(50, 0), new Point(100, 50));
			myPath.AddArc(50, 100, 75, 75, 0, 270);
			myPath.CloseFigure();
			myPath.StartFigure();
			myPath.AddArc(100, 10, 50, 50, 0, 270);

			// Second set of figures.
			myPath.StartFigure();
			myPath.AddArc(10, 200, 50, 50, 0, 270);
			myPath.CloseFigure();
			myPath.StartFigure();
			myPath.AddLine(new Point(60, 200), new Point(110, 250));
			myPath.AddArc(50, 300, 75, 75, 0, 270);
			myPath.CloseFigure();
			myPath.StartFigure();
			myPath.AddArc(100, 200, 50, 50, 0, 270);

			// Draw the path to the screen.
			g.DrawPath(new Pen(Color.Black), myPath);

			title = "StartFigure";
		} 

		public void GetBounds(Graphics g)
		{

			// Create path number 1 and a Pen for drawing.
			GraphicsPath myPath = new GraphicsPath();
			Pen pathPen = new Pen(Color.Black, 1);

			// Add an Ellipse to the path and Draw it (circle in start 

			// position).
			myPath.AddEllipse(20, 20, 100, 100);
			g.DrawPath(pathPen, myPath);

			// Get the path bounds for Path number 1 and draw them.
			RectangleF boundRect = myPath.GetBounds();
			g.DrawRectangle(new Pen(Color.Red, 1),
			                         boundRect.X,
			                         boundRect.Y,
			                         boundRect.Height,
			                         boundRect.Width);

			// Create a second graphics path and a wider Pen.
			GraphicsPath myPath2 = new GraphicsPath();
			Pen pathPen2 = new Pen(Color.Black, 10);

			// Create a new ellipse with a width of 10.
			myPath2.AddEllipse(150, 20, 100, 100);
			//myPath2.Widen(pathPen2);
			g.FillPath(Brushes.Black, myPath2);

			// Get the second path bounds.
			RectangleF boundRect2 = myPath2.GetBounds();

			// Draw the bounding rectangle.
			g.DrawRectangle(new Pen(Color.Red, 1),
			                         boundRect2.X,
			                         boundRect2.Y,
			                         boundRect2.Height,
			                         boundRect2.Width);

			// Display the rectangle size.
			//MessageBox.Show(boundRect2.ToString());
			title = "GetBounds";
		}

		private void Flatten1 (Graphics g)
		{
			GraphicsPath myPath = new GraphicsPath();
			Matrix translateMatrix = new Matrix();
			translateMatrix.Translate(0, 10);
			Point point1 = new Point(20, 100);
			Point point2 = new Point(70, 10);
			Point point3 = new Point(130, 200);
			Point point4 = new Point(180, 100);
			Point[] points = {point1, point2, point3, point4};
			myPath.AddCurve(points);
			g.DrawPath(new Pen(Color.Black, 2), myPath);
			myPath.Flatten(translateMatrix, 10f);
			g.DrawPath(new Pen(Color.Red, 1), myPath);

			title = "Flatten";
		}
	}
}

