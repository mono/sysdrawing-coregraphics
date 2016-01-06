using UIKit;

namespace MTExample5_5 {
	public partial class MTExample5_5ViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTExample5_5ViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTExample5_5ViewController_iPhone" : "MTExample5_5ViewController_iPad", null)
		{
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return UserInterfaceIdiomIsPhone ? toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown : true;
		}
	}
}

