using UIKit;

namespace MTFontTest {
	public partial class MTFontTestViewController : UIViewController {
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MTFontTestViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MTFontTestViewController_iPhone" : "MTFontTestViewController_iPad", null)
		{
		}
	}
}

