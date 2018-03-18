This file describes the progress in code integration from the mac-playground contribution

Directly from master, and adjusted, not partial:

Challenges with Graphics.cs:

	mac-playground abandoned support for retina displays (screenScale in Graphics),
	so we need to proceed with caution.   The patch so far merely splits code in Mac/iOS
	but does not attempt to do much else as this will require the test harness to be setup

	MIght be possible to compare not against master, but against the version before,
	which was 772ab833835d9b221c915536be993a8d503d05d3, and does not include the 
	changes to the retina code

Pending Files:
System.Drawing.Imaging/ColorPalette.cs
System.Drawing.Imaging/Encoder.cs
System.Drawing.Imaging/EncoderParameter.cs
System.Drawing.Imaging/EncoderParameterValueType.cs
System.Drawing.Imaging/EncoderParameters.cs
System.Drawing.Imaging/ImageAttributes.cs
System.Drawing.Imaging/ImageCodecFlags.cs
System.Drawing.Imaging/ImageCodecInfo.cs
System.Drawing.Printing/PreviewPageInfo.cs
System.Drawing.Printing/PreviewPrintController.cs
System.Drawing.Printing/PrintDocument.cs
System.Drawing.Printing/PrintEventArgs.cs
System.Drawing.Printing/PrintPageEventArgs.cs
System.Drawing.Printing/PrinterSettings.cs
System.Drawing.Printing/StandardPrintController.cs
System.Drawing.Text/FontCollection-CoreText.cs
System.Drawing.Text/FontCollection.cs
System.Drawing.Text/InstalledFontCollection.cs
System.Drawing.Text/PrivateFontCollection-CoreText.cs
System.Drawing.Text/PrivateFontCollection.cs
System.Drawing/Font-CoreText.cs
System.Drawing/FontFamily-CoreText.cs
System.Drawing/FontFamily.cs
System.Drawing/GDIPlus.cs
System.Drawing/Graphics-DrawEllipticalArc.cs
System.Drawing/SolidBrush.cs
System.Drawing/SystemFonts.cs
System.Drawing/TextureBrush.cs
Utilities/ClipperLib/clipper.cs
sysdrawing-coregraphics-Mac/System.Drawing/GDIPlus.cs
