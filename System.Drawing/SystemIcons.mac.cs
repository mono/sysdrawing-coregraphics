namespace System.Drawing
{
	public sealed partial class SystemIcons
	{
		static SystemIcons ()
		{
			WinLogo = FromResource ("Mono.ico");
			Shield = FromResource ("Shield.ico");
			Information = new Icon ("/System/Library/CoreServices/CoreTypes.bundle/Contents/Resources/ToolbarInfo.icns") { undisposable = true };
			Error = new Icon ("/System/Library/CoreServices/CoreTypes.bundle/Contents/Resources/AlertStopIcon.icns") { undisposable = true };
			Warning = new Icon ("/System/Library/CoreServices/CoreTypes.bundle/Contents/Resources/AlertCautionIcon.icns") { undisposable = true };
			Question = new Icon ("/System/Library/CoreServices/CoreTypes.bundle/Contents/Resources/AlertNoteIcon.icns") { undisposable = true };
		}
	}
}
