using UIKit;

namespace MTClippingTests {
	public partial class MTClippingTestsViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTClippingTestsViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTClippingTestsViewController_iPhone" : "MTClippingTestsViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

