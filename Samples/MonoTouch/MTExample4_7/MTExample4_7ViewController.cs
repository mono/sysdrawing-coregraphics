using UIKit;

namespace MTExample4_7 {
	public partial class MTExample4_7ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample4_7ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample4_7ViewController_iPhone" : "MTExample4_7ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return UserInterfaceIdiomIsPhone ? (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown) : true;
		}
	}
}

