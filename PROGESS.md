This file describes the progress in code integration from the mac-playground contribution

Directly from master, and adjusted, not partial:

* Bitmap.cs - imported directly from master
* PrinterSettings.cs 
* PageSettings.cs
* Extensions
* Color
* KnownColors
* KnownColor
* Font.cs
* SystemFonts.cs
	- Had to create an iOS version, but this one is not complete (SystemFonts.ios.cs)
* Graphics-DrawImage.cs 
* Graphics-DrawString.cs
* Graphics-DrawStringCache.cs
* StringFormat.cs
* Image.cs
* Region.cs
* Graphics.cs
* SystemIcons.cs
* Icon.cs

On the first patch, fa17b7bb8140752280ff846d8fa97c7936cc1881, next up
Graphics.cs

Challenges with Graphics.cs:

	mac-playground abandoned support for retina displays (screenScale in Graphics),
	so we need to proceed with caution.   The patch so far merely splits code in Mac/iOS
	but does not attempt to do much else as this will require the test harness to be setup

	MIght be possible to compare not against master, but against the version before,
	which was 772ab833835d9b221c915536be993a8d503d05d3, and does not include the 
	changes to the retina code
