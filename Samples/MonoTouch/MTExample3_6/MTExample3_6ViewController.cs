using UIKit;

namespace MTExample3_6 {
	public partial class MTExample3_6ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample3_6ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample3_6ViewController_iPhone" : "MTExample3_6ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown;
		}
	}
}

