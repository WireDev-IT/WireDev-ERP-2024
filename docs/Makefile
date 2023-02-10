.PHONY: default
default:
	pdflatex --interaction=nonstopmode Vorlage.tex

.PHONY: clean
clean:
	rm \
		*.cb \
		*.cb2 \
		*.idx \
		*.mw \
		*.out \
		*.snm \
		*.toc \
		*.log \
		*.nav \
	|| true

.PHONY: oesch
oesch:
	wget http://mirrors.ctan.org/fonts/oesch/oe.def
	wget http://mirrors.ctan.org/fonts/oesch/oesch.mf
	wget http://mirrors.ctan.org/fonts/oesch/oesch.sty
	wget http://mirrors.ctan.org/fonts/oesch/oesch_lig.mf
	wget http://mirrors.ctan.org/fonts/oesch/oesch_m.mf
	wget http://mirrors.ctan.org/fonts/oesch/oeschb.mf
