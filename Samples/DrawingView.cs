
using System;
using System.Collections.Generic;

using System.Drawing;
using System.Drawing.Drawing2D;

using Foundation;
using CoreGraphics;

#if __MAC__
using AppKit;
#endif

#if __IOS__
using UIKit;
#endif

namespace DrawingShared
{
    [Register("DrawingView")]
    #if __MAC__
    public partial class DrawingView : AppKit.NSView
    #endif
    #if __IOS__
    public partial class DrawingView : UIKit.UIView
    #endif
    {

        public event PaintEventHandler Paint;


        public Action<Graphics>[] paintViewActions;

        int currentView = 0;

        // When true will save the graphics to a file on the desktop
        bool saveCurrentView = false;

        #if __MAC__
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

            PlatformInitialize();
        }

        public DrawingView (CGRect rect) : base (rect)
        {
            Initialize();
        }

        #endregion
        #endif

        #if __IOS__

        #region Constructors

        // Called when created from unmanaged code
        public DrawingView (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        public DrawingView (CGRect rect) : base (rect)
        {
            Initialize ();
        }

        // Shared initialization code
        void Initialize ()
        {
            BackColor = Color.Wheat;

            PlatformInitialize();
        }
        #endregion

        #endif


        #region Panel interface
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
                    font = new Font("Helvetica",12);
                return font;
            }
            set 
            {
                font = value;
            }
        }

        public int Left 
        {
            get { 

                return (int)Frame.Left; 
            }

            set {
                var location = new CGPoint (value, Frame.Y);
                Frame = new CGRect(location, Frame.Size);
            }

        }

        public int Right 
        {
            get { return (int)Frame.Right; }

            set { 
                var size = Frame;
                size.Width = size.X - value;
                Frame = size;
            }

        }

        public int Top
        {
            get { return (int)Frame.Top; }
            set { 
                var location = new CGPoint (Frame.X, value);
                Frame = new CGRect(location, Frame.Size);

            }
        }

        public int Bottom
        {
            get { return (int)Frame.Bottom; }
            set { 
                var frame = Frame;
                frame.Height = frame.Y - value;
                Frame = frame;

            }
        }

        public int Width 
        {
            get { return (int)Frame.Width; }
            set { 
                var frame = Frame;
                frame.Width = value;
                Frame = frame;
            }
        }

        public int Height
        {
            get { return (int)Frame.Height; }
            set { 
                var frame = Frame;
                frame.Height = value;
                Frame = frame;
            }
        }
        #endregion

        #if __MAC__
        public override void DrawRect (CGRect dirtyRect)
        #else
        public override void Draw(CoreGraphics.CGRect dirtyRect)
        #endif
        {
            Graphics g = Graphics.FromCurrentContext();
            g.Clear(backColor);

            Rectangle clip = new Rectangle((int)dirtyRect.X,
                (int)dirtyRect.Y,
                (int)dirtyRect.Width,
                (int)dirtyRect.Height);

            var args = new PaintEventArgs(g, clip);

            OnPaint(args);

            if(Paint != null)
            {
                Paint(this, args);
            }

        }

        #if __MAC__
        public override bool AcceptsFirstResponder ()
        {
            return true;
        }

        public override void KeyDown (NSEvent theEvent)
        {
            currentView++;
            currentView %= paintViewActions.Length;
            this.NeedsDisplay = true;
        }
        #endif

        #if __IOS__
        public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
        {
            currentView++;
            currentView %= paintViewActions.Length;
            //Console.WriteLine("Current View: {0}", currentView);
            MarkDirty();
            //this.NeedsDisplay = true;
            SetNeedsDisplay ();
        }
        #endif

        void SavePaintView(Action<Graphics> paintView)
        {
            var imageName = System.IO.Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Desktop), paintView.Method.Name + ".png");
            Bitmap bitmap = new Bitmap(606, 354, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(bitmap);
            g.Clear(BackColor);
            paintView.Invoke(g);
            bitmap.Save(imageName, System.Drawing.Imaging.ImageFormat.Png);
        }

        #if __MAC__
        // Here we make sure we are flipped so our subview PlotPanel size and location
        // are calculated correctly.  If not the positions are calculated on the 0,0 in 
        // the lower left corner instead of upper left.
        public override bool IsFlipped {
            get {
                //return base.IsFlipped;
                return true;
            }
        }
        #endif
    }


}

public delegate void PaintEventHandler(object sender, PaintEventArgs e);


public class PaintEventArgs : EventArgs, IDisposable
{
    private readonly Rectangle clipRect;
    private Graphics graphics;

    public PaintEventArgs(Graphics graphics, Rectangle clipRect)
    {
        if (graphics == null)
        {
            throw new ArgumentNullException("graphics");
        }
        this.graphics = graphics;
        this.clipRect = clipRect;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if ((disposing && (this.graphics != null)))
        {
            this.graphics.Dispose();
        }
    }

    ~PaintEventArgs()
    {
        this.Dispose(false);
    }

    public Rectangle ClipRectangle
    {
        get
        {
            return this.clipRect;
        }
    }

    public Graphics Graphics
    {
        get
        {
            return this.graphics;
        }
    }

}
