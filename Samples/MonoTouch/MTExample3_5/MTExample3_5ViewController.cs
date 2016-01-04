using UIKit;

namespace MTExample3_5 {
	public partial class MTExample3_5ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample3_5ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample3_5ViewController_iPhone" : "MTExample3_5ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown;
		}
	}
}

