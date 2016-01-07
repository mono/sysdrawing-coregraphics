using UIKit;

namespace MTBitmapTests {
	public partial class MTBitmapTestsViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTBitmapTestsViewController ()
			: base(UserInterfaceIdiomIsPhone ? "MTBitmapTestsViewController_iPhone" : "MTBitmapTestsViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return !UserInterfaceIdiomIsPhone || (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

