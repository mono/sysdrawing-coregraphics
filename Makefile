CSC=/Developer/MonoTouch/usr/bin/smcs
MONO_SYSD=../mono/mcs/class/System.Drawing

MONO_SOURCES = \
	$(MONO_SYSD)/System.Drawing.Drawing2D/CompostingMode.cs \
	$(MONO_SYSD)/System.Drawing.Drawing2D/MatrixOrder.cs

SOURCES =	\
	./System.Drawing/Brush.cs		\
	./System.Drawing/Color.cs		\
	./System.Drawing/Graphics.cs		\
	./System.Drawing/KnownColor.cs		\
	./System.Drawing/KnownColors.cs		\
	./System.Drawing/Pen.cs			\
	./System.Drawing/SolidBrush.cs		\
	./System.Drawing.Drawing2D/Matrix.cs

all: System.Drawing.dll

System.Drawing.dll: $(SOURCES) $(MONO_SOURCES) Makefile
	$(CSC) -target:library -out:System.Drawing.dll -debug $(SOURCES) $(MONO_SOURCES) -r:monotouch.dll 