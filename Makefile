MONOTOUCH_PREFIX=/Developer/MonoTouch
SMCS=$(MONOTOUCH_PREFIX)/usr/bin/smcs
MONO_SYSD=../mono/mcs/class/System.Drawing

MONO_SOURCES = \
	$(MONO_SYSD)/../../build/common/Locale.cs		\
	$(MONO_SYSD)/System.Drawing/ImageConverter.cs		\
	$(MONO_SYSD)/System.Drawing/ImageFormatConverter.cs	\
	$(MONO_SYSD)/System.Drawing/ToolboxBitmapAttribute.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/Blend.cs		\
	$(MONO_SYSD)/System.Drawing.Drawing2D/ColorBlend.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/CompostingMode.cs	\
	$(MONO_SYSD)/System.Drawing.Drawing2D/MatrixOrder.cs	\
	$(MONO_SYSD)/System.Drawing.Imaging/ImageFormat.cs	\
	$(MONO_SYSD)/System.Drawing.Imaging/PixelFormat.cs

MONOMAC_EXTRA_SOURCES = \
	$(MONO_SYSD)/System.Drawing/Point.cs		\
	$(MONO_SYSD)/System.Drawing/PointF.cs		\
	$(MONO_SYSD)/System.Drawing/Rectangle.cs	\
	$(MONO_SYSD)/System.Drawing/RectangleF.cs	\
	$(MONO_SYSD)/System.Drawing/Size.cs		\
	$(MONO_SYSD)/System.Drawing/SizeF.cs

SOURCES =	\
	./System.Drawing/Bitmap.cs		\
	./System.Drawing/Brush.cs		\
	./System.Drawing/Color.cs		\
	./System.Drawing/Graphics.cs		\
	./System.Drawing/Image.cs		\
	./System.Drawing/KnownColor.cs		\
	./System.Drawing/KnownColors.cs		\
	./System.Drawing/Pen.cs			\
	./System.Drawing/SolidBrush.cs		\
	./System.Drawing.Drawing2D/Matrix.cs

all: monotouch

monotouch: monotouch/System.Drawing.dll

monotouch/System.Drawing.dll: $(SOURCES) $(MONO_SOURCES)
	mkdir -p monotouch
	$(SMCS) -target:library -out:monotouch/System.Drawing.dll -debug $(SOURCES) $(MONO_SOURCES) -r:monotouch.dll 

monomac: monomac/System.Drawing.dll

# circular dependency problem. MonoMac.dll depends on System.Drawing.dll which would now depend on MonoMac.dll
monomac/System.Drawing.dll: $(SOURCES) $(MONO_SOURCES) $(MONO_EXTRA_SOURCES)
	mkdir -p monomac
	mcs -target:library -out:monomac/System.Drawing.dll -define:MONOMAC -debug $(SOURCES) $(MONO_SOURCES) $(MONOMAC_EXTRA_SOURCES) -r:../monomac/src/MonoMac.dll

clean:
	rm monotouch/*.dll*
	rm monomac/*.dll*

install: all
	cp monotouch/System.Drawing.dll monotouch/System.Drawing.dll.mdb $(MONOTOUCH_PREFIX)/usr/lib/mono/2.1/
