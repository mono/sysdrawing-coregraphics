using UIKit;

namespace MTExample5_6 {
	public partial class MTExample5_6ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample5_6ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample5_6ViewController_iPhone" : "MTExample5_6ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown;
		}
	}
}

