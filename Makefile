MONO_SYSD=../mono/mcs/class/System.Drawing

MONO_SOURCES = \
	$(MONO_SYSD)/System.Drawing.Drawing2D/Blend.cs			\
	$(MONO_SYSD)/System.Drawing.Drawing2D/ColorBlend.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/CombineMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/CompostingMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/CompostingQuality.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/CoordinateSpace.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/DashCap.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/DashStyle.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/FillMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/FlushIntention.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/InterpolationMode.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/LineCap.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/LineJoin.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/LinearGradientMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/MatrixOrder.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/PathData.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/PathPointType.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/PixelOffsetMode.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/SmoothingMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/WarpMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/WrapMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/GraphicsState.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/GraphicsContainer.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/BitmapData.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorAdjustType.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorChannelFlag.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorMap.cs			\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorMatrix.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorMatrixFlag.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ImageFlags.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ImageFormat.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ImageLockMode.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/PixelFormat.cs		\
	$(MONO_SYSD)/System.Drawing.Printing/PrintPageEventHandler.cs	\
	$(MONO_SYSD)/System.Drawing.Text/GenericFontFamilies.cs		\
	$(MONO_SYSD)/System.Drawing.Text/TextRenderingHint.cs		\
	$(MONO_SYSD)/System.Drawing/CharacterRange.cs			\
	$(MONO_SYSD)/System.Drawing/FontStyle.cs			\
	$(MONO_SYSD)/System.Drawing/GraphicsUnit.cs			\
	$(MONO_SYSD)/System.Drawing/IconConverter.cs			\
	$(MONO_SYSD)/System.Drawing/ImageConverter.cs			\
	$(MONO_SYSD)/System.Drawing/ImageFormatConverter.cs		\
	$(MONO_SYSD)/System.Drawing/RotateFlipType.cs			\
	$(MONO_SYSD)/System.Drawing/StringAligment.cs			\
	$(MONO_SYSD)/System.Drawing/StringFormatFlags.cs		\
	$(MONO_SYSD)/System.Drawing/StringTrimming.cs			\
	$(MONO_SYSD)/System.Drawing/SystemColors.cs			\
	$(MONO_SYSD)/System.Drawing/ToolboxBitmapAttribute.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/HatchStyle.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/FrameDimension.cs		\
	$(MONO_SYSD)/System.Drawing/Brushes.cs				\
	$(MONO_SYSD)/System.Drawing/Pens.cs				\
	$(MONO_SYSD)/System.Drawing/PointConverter.cs			\
	$(MONO_SYSD)/System.Drawing/RectangleConverter.cs		\
	$(MONO_SYSD)/System.Drawing/SizeConverter.cs			\
	$(MONO_SYSD)/System.Drawing/SizeFConverter.cs			\

SOURCES =	\
	./System.Drawing.Drawing2D/GraphicsPath.cs	\
	./System.Drawing.Drawing2D/CGGraphicsState.cs	\
	./System.Drawing.Drawing2D/Matrix.cs		\
	./System.Drawing.Imaging/ImageAttributes.cs	\
	./System.Drawing.Printing/PageSettings.cs	\
	./System.Drawing.Printing/PrintDocument.cs	\
	./System.Drawing.Printing/PrintPageEventArgs.cs	\
	./System.Drawing.Printing/PrinterSettings.cs	\
	./System.Drawing.Text/FontCollection.cs		\
	./System.Drawing/Bitmap.cs			\
	./System.Drawing/Brush.cs			\
	./System.Drawing/Color.cs			\
	./System.Drawing/Font.cs			\
	./System.Drawing/FontFamily.cs			\
	./System.Drawing/Graphics-DrawEllipticalArc.cs	\
	./System.Drawing/Graphics-DrawImage.cs		\
	./System.Drawing/Graphics.cs			\
	./System.Drawing/Icon.cs			\
	./System.Drawing/Image.cs			\
	./System.Drawing/KnownColor.cs			\
	./System.Drawing/KnownColors.cs			\
	./System.Drawing/Pen.cs				\
	./System.Drawing/Region.cs			\
	./System.Drawing/SolidBrush.cs			\
	./System.Drawing/StringFormat.cs		\
	./Utilities/ClipperLib/clipper.cs 		\
	./Utilities/ConversionHelpers.cs		\
	./System.Drawing.Drawing2D/GraphicsPath-AddString.cs \
	./System.Drawing.Drawing2D/GraphicsPathIterator.cs \
	./System.Drawing.Drawing2D/HatchBrush.cs \
	./System.Drawing.Drawing2D/LinearGradientBrush.cs \
	./System.Drawing.Drawing2D/PathGradientBrush.cs \
	./System.Drawing.Text/InstalledFontCollection.cs \
	./System.Drawing.Text/PrivateFontCollection-CoreText.cs \
	./System.Drawing.Text/PrivateFontCollection.cs \
	./System.Drawing.Text/FontCollection-CoreText.cs \
	./System.Drawing/Font-CoreText.cs \
	./System.Drawing/FontFamily-CoreText.cs \
	./System.Drawing/Graphics-DrawString.cs \
	./System.Drawing/TextureBrush.cs \
	./Utilities/GeomTransformUtils.cs \
	./Utilities/GeomUtilities.cs \
	./Utilities/Locale.cs \

all: ios mac
mac: mac-mobile mac-xm45

IOS_PREFIX=/Library/Frameworks/Xamarin.iOS.framework/Versions/Current

ios: bin/ios/System.Drawing.dll

bin/ios/System.Drawing.dll: $(SOURCES) $(MONO_SOURCES) Makefile
	mkdir -p bin/ios
	$(IOS_PREFIX)/bin/smcs -define:MONOTOUCH -unsafe -target:library -out:bin/ios/System.Drawing.dll -debug $(SOURCES) $(MONO_SOURCES) -r:$(IOS_PREFIX)/lib/mono/Xamarin.iOS/System.Core.dll -r:$(IOS_PREFIX)/lib/mono/Xamarin.iOS/Xamarin.iOS.dll


MAC_PREFIX=/Library/Frameworks/Xamarin.Mac.framework/Versions/Current
MAC_SOURCES=$(SOURCES) $(MONO_SOURCES)
mac-mobile: bin/mac/mobile/System.Drawing.dll
mac-xm45: bin/mac/xm45/System.Drawing.dll

bin/mac/mobile/System.Drawing.dll: $(SOURCES) $(MONO_SOURCES) $(MONO_EXTRA_SOURCES) Makefile
	mkdir -p bin/mac/mobile
	/Library/Frameworks/Mono.framework/Commands/mcs -unsafe -noconfig -define:MONOMAC -debug -out:bin/mac/mobile/System.Drawing.dll $(MAC_SOURCES)  -target:library -define:MONOMAC /nostdlib /reference:$(MAC_PREFIX)/lib/mono/Xamarin.Mac/System.dll /reference:$(MAC_PREFIX)/lib/mono/Xamarin.Mac/System.Core.dll /reference:$(MAC_PREFIX)/lib/mono/Xamarin.Mac/Xamarin.Mac.dll /reference:$(MAC_PREFIX)/lib/mono/Xamarin.Mac/mscorlib.dll

bin/mac/xm45/System.Drawing.dll: $(SOURCES) $(MONO_SOURCES) $(MONO_EXTRA_SOURCES) Makefile
	mkdir -p bin/mac/xm45
	/Library/Frameworks/Mono.framework/Commands/mcs -unsafe -noconfig -define:MONOMAC -define:XM45 -debug -out:bin/mac/xm45/System.Drawing.dll $(MAC_SOURCES)  -target:library -define:MONOMAC /nostdlib /reference:$(MAC_PREFIX)/lib/mono/4.5/System.dll /reference:$(MAC_PREFIX)/lib/mono/4.5/System.Core.dll /reference:$(MAC_PREFIX)/lib/mono/4.5/Xamarin.Mac.dll /reference:$(MAC_PREFIX)/lib/mono/4.5/mscorlib.dll /reference:$(MAC_PREFIX)/lib/mono/4.5/OpenTK.dll

clean:
	rm -rf bin/
