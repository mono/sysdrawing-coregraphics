MONOTOUCH_PREFIX=/Developer/MonoTouch
SMCS=$(MONOTOUCH_PREFIX)/usr/bin/smcs
MONO_SYSD=../mono/mcs/class/System.Drawing

MONO_SOURCES = \
	$(MONO_SYSD)/../../build/common/Locale.cs			\
	$(MONO_SYSD)/System.Drawing/FontStyle.cs			\
	$(MONO_SYSD)/System.Drawing/GraphicsUnit.cs			\
	$(MONO_SYSD)/System.Drawing/ImageConverter.cs			\
	$(MONO_SYSD)/System.Drawing/ImageFormatConverter.cs		\
	$(MONO_SYSD)/System.Drawing/StringAligment.cs			\
	$(MONO_SYSD)/System.Drawing/SystemColors.cs			\
	$(MONO_SYSD)/System.Drawing/ToolboxBitmapAttribute.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/Blend.cs			\
	$(MONO_SYSD)/System.Drawing.Drawing2D/ColorBlend.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/CombineMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/CompostingMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/FillMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/InterpolationMode.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/MatrixOrder.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/PathData.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/PathPointType.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/PixelOffsetMode.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/SmoothingMode.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/WrapMode.cs		\
	$(MONO_SYSD)/System.Drawing.Text/TextRenderingHint.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/BitmapData.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorAdjustType.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorChannelFlag.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorMatrix.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorMatrixFlag.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ColorMap.cs			\
	$(MONO_SYSD)/System.Drawing.Imaging/ImageFormat.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/ImageLockMode.cs		\
	$(MONO_SYSD)/System.Drawing.Imaging/PixelFormat.cs		\
	$(MONO_SYSD)/System.Drawing.Printing/PrintPageEventHandler.cs	\

MONOMAC_EXTRA_SOURCES = \
	$(MONO_SYSD)/System.Drawing/Point.cs		\
	$(MONO_SYSD)/System.Drawing/PointF.cs		\
	$(MONO_SYSD)/System.Drawing/Rectangle.cs	\
	$(MONO_SYSD)/System.Drawing/RectangleF.cs	\
	$(MONO_SYSD)/System.Drawing/Size.cs		\
	$(MONO_SYSD)/System.Drawing/SizeF.cs

SOURCES =	\
	./System.Drawing/Bitmap.cs			\
	./System.Drawing/Brush.cs			\
	./System.Drawing/Color.cs			\
	./System.Drawing/Font.cs			\
	./System.Drawing/FontFamily.cs			\
	./System.Drawing/Graphics.cs			\
	./System.Drawing/Graphics-DrawImage.cs		\
	./System.Drawing/Image.cs			\
	./System.Drawing/KnownColor.cs			\
	./System.Drawing/KnownColors.cs			\
	./System.Drawing/Pen.cs				\
	./System.Drawing/Region.cs			\
	./System.Drawing/SolidBrush.cs			\
	./System.Drawing/StringFormat.cs		\
	./System.Drawing.Drawing2D/GraphicsContainer.cs	\
	./System.Drawing.Drawing2D/GraphicsPath.cs	\
	./System.Drawing.Drawing2D/GraphicsState.cs	\
	./System.Drawing.Drawing2D/Matrix.cs		\
	./System.Drawing.Printing/PageSettings.cs	\
	./System.Drawing.Printing/PrintDocument.cs	\
	./System.Drawing.Printing/PrinterSettings.cs	\
	./System.Drawing.Printing/PrintPageEventArgs.cs	\
	./System.Drawing.Imaging/ImageAttributes.cs	\

all: monotouch

monotouch: monotouch/System.Drawing.dll

monotouch/System.Drawing.dll: $(SOURCES) $(MONO_SOURCES) Makefile
	mkdir -p monotouch
	$(SMCS) -target:library -out:monotouch/System.Drawing.dll -debug $(SOURCES) $(MONO_SOURCES) -r:monotouch.dll 

monomac: monomac/System.Drawing.dll

# circular dependency problem. MonoMac.dll depends on System.Drawing.dll which would now depend on MonoMac.dll
monomac/System.Drawing.dll: $(SOURCES) $(MONO_SOURCES) $(MONO_EXTRA_SOURCES) Makefile
	mkdir -p monomac
	mcs -target:library -out:monomac/System.Drawing.dll -define:MONOMAC -debug $(SOURCES) $(MONO_SOURCES) $(MONOMAC_EXTRA_SOURCES) -r:../monomac/src/MonoMac.dll

clean:
	rm monotouch/*.dll*
	rm monomac/*.dll*

install: all
	cp monotouch/System.Drawing.dll monotouch/System.Drawing.dll.mdb $(MONOTOUCH_PREFIX)/usr/lib/mono/2.1/
