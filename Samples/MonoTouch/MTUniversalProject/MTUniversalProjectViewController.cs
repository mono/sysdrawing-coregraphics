using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MTUniversalProject
{
	public partial class MTUniversalProjectViewController : UIViewController
	{

		CGRect portrait;
		CGRect landscape;

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTUniversalProjectViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTUniversalProjectViewController_iPhone" : "MTUniversalProjectViewController_iPad", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			portrait = new CGRect (this.View.Bounds.Location, this.View.Bounds.Size);
			landscape = new CGRect(this.View.Bounds.Location, new CGSize(this.View.Bounds.Width * 2, this.View.Bounds.Height * 2));
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UserInterfaceIdiomIsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}

		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{

			CheckOrientation ();

			Console.WriteLine("Rotate From {0} to {1} with frame {2}",fromInterfaceOrientation, InterfaceOrientation, this.View.Frame);
			base.DidRotate (fromInterfaceOrientation);
		}

		void CheckOrientation()
		{
			switch (InterfaceOrientation) 
			{
			case UIInterfaceOrientation.LandscapeLeft:
			case UIInterfaceOrientation.LandscapeRight:
				//View.Subviews [0].Bounds = landscape;
				break;
			default:
				//View.Subviews [0].Bounds = portrait;
				break;
			}

			View.Subviews [0].SetNeedsDisplay ();

		}
	}
}

