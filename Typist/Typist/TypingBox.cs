using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Typist.Appearance;

namespace Typist
{
    public class TypingBox : PictureBox
    {
        #region Properties

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TextBuffer ImportedText { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TextBuffer TypedText { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Font TypingFont { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Theme Theme { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(2)]
        public int BarCursorLineWidth { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0)]
        public int MarginLeft { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0)]
        public int MarginRight { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0)]
        public int MarginTop { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0)]
        public int MarginBottom { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(true)]
        public bool CursorAsVerticalBar { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue('_')]
        public char CharCursorChar { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0)]
        public float BarCursorVOffset { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0)]
        public float CharCursorVOffset { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0)]
        public float ErrorBackgroundVOffset { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(false)]
        public bool VisibleNewlines { get; set; }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event CancelEventHandler DrawingCursor;

        #endregion


        #region General

        public TypingBox()
        {
            BarCursorLineWidth = 2;
            CursorAsVerticalBar = true;
            CharCursorChar = '_';
        }

        protected virtual void OnDrawingCursor(CancelEventArgs e)
        {
            if (DrawingCursor != null)
                DrawingCursor(this, e);
        }

        protected override void OnResize(EventArgs e)
        {
            this.Invalidate();
            base.OnResize(e);
        }

        #endregion


        #region Paint event handler

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (!DesignMode)
            {
                GraphicsContext gc  = getGraphicsContext(pe.Graphics);

                drawImportedText(gc);
                drawShadowText(gc);
                drawErrorChars(gc);
                drawCursor(gc);
                drawTopMargin(gc);
            }
        }

        private GraphicsContext getGraphicsContext(Graphics g)
        {
            RectangleF typingArea = new RectangleF()
            {
                X = MarginLeft,
                Y = MarginTop,
                Width = g.ClipBounds.Width - (MarginLeft + MarginRight),
                Height = g.ClipBounds.Height - (MarginTop + MarginBottom),
            };

            RectangleF[] sampleCharAreas = getCharAreas32("m\nm", new[] { 0, 2 }, g, typingArea);

            typingArea.Width -= sampleCharAreas[0].Width;

            float rowHeight = sampleCharAreas[1].IsEmpty ? sampleCharAreas[0].Height : (sampleCharAreas[1].Y - sampleCharAreas[0].Y);

            float vOffset = rowHeight * calculateRowOffset(g, typingArea, rowHeight);

            return new GraphicsContext()
            {
                Graphics = g,
                TypingArea = typingArea,
                DocumentArea = new RectangleF(typingArea.X, typingArea.Y + vOffset,
                                              typingArea.Width, typingArea.Height - vOffset),
            };
        }

        private int calculateRowOffset(Graphics g, RectangleF typingArea, float rowHeight)
        {
            if (ImportedText.Length == 0 || rowHeight == 0)
                return 0;

            RectangleF unlimitedArea = new RectangleF()
            {
                X = typingArea.X,
                Y = typingArea.Y,
                Width = typingArea.Width,
                Height = 10000000f,
            };

            GraphicsContext unlimitedContext = new GraphicsContext()
            {
                Graphics = g,
                DocumentArea = unlimitedArea,
            };

            RectangleF[] charAreas = getCharAreas(ImportedText.ToString(),
                                                  new int[]
                                                  {
                                                      ImportedText.LastIndex,
                                                      TypedText.LastIndex + 1,
                                                  },
                                                  unlimitedContext);

            int lastDocumentRow = getRow(charAreas[0].Y, unlimitedArea.Y, rowHeight);

            int cursorRow = TypedText.LastIndex < ImportedText.LastIndex ?
                                getRow(charAreas[1].Y, unlimitedArea.Y, rowHeight) :
                                lastDocumentRow;

            int visibleRows = Math.Max(1, (int)Math.Floor((double)typingArea.Height / rowHeight));

            if (lastDocumentRow < visibleRows ||
                cursorRow <= visibleRows / 2)
                return 0;
            else if (lastDocumentRow - cursorRow >= visibleRows / 2)
                return -cursorRow + visibleRows / 2;
            else
                return visibleRows - 1 - lastDocumentRow;
        }

        private int getRow(float charY, float typingAreaY, float rowHeight)
        {
            return (int)Math.Round((double)(charY - typingAreaY) / rowHeight, 0);
        }

        private void drawImportedText(GraphicsContext gc)
        {
            drawText(ImportedText.ToString(), gc, Theme.ImportedTextColor);
        }

        private void drawShadowText(GraphicsContext gc)
        {
            string shadowText = ImportedText.Substring(0, TypedText.Length);

            if (TypedText.Length > 0 && !char.IsWhiteSpace(ImportedText[TypedText.LastIndex]))
            {
                int lineJumpIndex = findLineJump(ImportedText.ToString(), TypedText.LastIndex, gc);
                if (lineJumpIndex < TypedText.LastIndex)
                    shadowText = insertSpacesAfterJump(ImportedText.ToString(), shadowText,
                                                       lineJumpIndex, TypedText.LastIndex,
                                                       gc);
            }

            drawText(shadowText, gc, Theme.TypedTextColor);
        }

        private int findLineJump(string text, int lastIndex, GraphicsContext gc)
        {
            RectangleF importedCharArea, shadowCharArea;
            int lineJumpIndex = lastIndex;

            while (lineJumpIndex >= 0)
            {
                importedCharArea = getCharArea(text, lineJumpIndex, gc);

                shadowCharArea = getCharArea(text.Substring(0, lineJumpIndex + 1), lineJumpIndex, gc);

                if (importedCharArea.IsEmpty || shadowCharArea.IsEmpty)
                    return lastIndex;

                if (shadowCharArea.Y == importedCharArea.Y)
                    return lineJumpIndex;

                lineJumpIndex--;
            }

            return lastIndex;
        }

        private string insertSpacesAfterJump(string text, string shadowText, int lineJumpIndex, int lastIndex, GraphicsContext gc)
        {
            RectangleF importedCharArea = getCharArea(text, lastIndex, gc);

            RectangleF shadowCharArea;
            int spaces = 0;

            do
            {
                shadowText = shadowText.Insert(lineJumpIndex + 1, " ");
                spaces++;

                shadowCharArea = getCharArea(shadowText, lastIndex + spaces, gc);
            }
            while (shadowCharArea.Y < importedCharArea.Y);

            return shadowText;
        }

        private void drawErrorChars(GraphicsContext gc)
        {
            RectangleF[] errorCharAreas = getCharAreas(ImportedText.ToString(),
                                                       TypedText.ErrorsUncorrected.ToArray(), gc);

            for (int i = 0; i < TypedText.ErrorsUncorrected.Count; i++)
            {
                gc.Graphics.FillRectangle(Theme.ErrorBackColor, fracOffsetCharArea(errorCharAreas[i], 0, ErrorBackgroundVOffset));

                drawChar(TypedText[TypedText.ErrorsUncorrected[i]],
                         gc, Theme.ErrorForeColor,
                         errorCharAreas[i]);
            }
        }

        private void drawCursor(GraphicsContext gc)
        {
            CancelEventArgs e = new CancelEventArgs(false);

            OnDrawingCursor(e);

            if (!e.Cancel)
            {
                RectangleF cursorArea = getCharArea(ImportedText.ToString(),
                                                    TypedText.LastIndex + 1, gc);

                cursorArea = fracOffsetCharArea(cursorArea, 0,
                                                CursorAsVerticalBar ? BarCursorVOffset : CharCursorVOffset);

                if (CursorAsVerticalBar)
                    gc.Graphics.FillRectangle(Theme.CursorColor, new RectangleF()
                    {
                        X = cursorArea.X - (0.125f * cursorArea.Width) - (0.5f * BarCursorLineWidth),
                        Y = cursorArea.Y,
                        Width = BarCursorLineWidth,
                        Height = cursorArea.Height,
                    });
                else
                    drawChar(CharCursorChar, gc, Theme.CursorColor, cursorArea);
            }
        }

        private RectangleF fracOffsetCharArea(RectangleF charArea, float fracWidth, float fracHeight)
        {
            return offsetCharArea(charArea, fracWidth * charArea.Width, fracHeight * charArea.Height);
        }

        private RectangleF offsetCharArea(RectangleF charArea, float x, float y)
        {
            return new RectangleF()
            {
                X = charArea.X + x,
                Y = charArea.Y + y,
                Width = charArea.Width,
                Height = charArea.Height,
            };
        }

        private char pilcrow = '\xB6';

        private void drawChar(char ch, GraphicsContext gc, Brush brush, RectangleF charArea)
        {
            if (ch == '\n')
                ch = pilcrow;

            gc.Graphics.DrawString(ch.ToString(), TypingFont, brush, charArea, SingleCharStringFormat);
        }

        private void drawText(string text, GraphicsContext gc, Brush brush)
        {
            if (VisibleNewlines)
                text = text.Replace("\n", string.Format("{0}\n", pilcrow));

            gc.Graphics.DrawString(text, TypingFont, brush, gc.DocumentArea, TextStringFormat);
        }

        private RectangleF getCharArea(string text, int charIndex, GraphicsContext gc)
        {
            return getCharAreas(text, new[] { charIndex }, gc).First();
        }

        private RectangleF[] getCharAreas(string text, int[] charIndexes, GraphicsContext gc)
        {
            charIndexes = charIndexes.Where(i => i < text.Length)
                                     .ToArray();

            RectangleF[] charAreas = charIndexes.Split(32)
                                                .SelectMany(grp => getCharAreas32(text, grp.ToArray(), gc))
                                                .ToArray();

            for (int i = 0; i < charIndexes.Length; i++)
                if (charAreas[i].IsEmpty)
                    charAreas[i] = getCharAreas32(text.Insert(charIndexes[i], "-"),
                                                  new[] { charIndexes[i] },
                                                  gc)[0];

            return charAreas;
        }

        private RectangleF[] getCharAreas32(string text, int[] charIndexes, GraphicsContext gc)
        {
            return getCharAreas32(text, charIndexes, gc.Graphics, gc.DocumentArea);
        }

        private RectangleF[] getCharAreas32(string text, int[] charIndexes, Graphics g, RectangleF documentArea)
        {
            CharacterRange[] ranges = charIndexes.Select(i => new CharacterRange(i, 1)).ToArray();

            StringFormat stringFormat = new StringFormat(TextStringFormat);
            stringFormat.SetMeasurableCharacterRanges(ranges);

            Region[] regions = g.MeasureCharacterRanges(text, TypingFont, documentArea, stringFormat);

            return regions.Select(r => r.GetBounds(g))
                          .ToArray();
        }

        private void drawTopMargin(GraphicsContext gc)
        {
            gc.Graphics.FillRectangle(Brushes.White, new RectangleF()
            {
                X = 0,
                Y = 0,
                Width = gc.Graphics.ClipBounds.Width,
                Height = MarginTop + 1,
            });
        }

        private void drawDebugMessage(string message, GraphicsContext gc)
        {
            drawDebugMessage(message, gc, true);
        }

        private void drawDebugMessage(string message, GraphicsContext gc, bool atBottom)
        {
            float y = atBottom ? (gc.TypingArea.Y + gc.TypingArea.Height - 16) : gc.TypingArea.Y;

            RectangleF position = new RectangleF(gc.TypingArea.X, y,
                                                 gc.TypingArea.Width, 16);

            using (Font font = new Font("Courier New", 10))
                gc.Graphics.DrawString(message, font, Brushes.Red, position, TextStringFormat);
        }

        protected StringFormat TextStringFormat
        {
            get
            {
                if (textStringFormat == null)
                    textStringFormat = new StringFormat(StringFormatFlags.LineLimit)
                    {
                        Trimming = StringTrimming.Word,
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Near,
                    };

                return textStringFormat;
            }
        }
        private StringFormat textStringFormat;

        protected StringFormat SingleCharStringFormat
        {
            get
            {
                if (singleCharStringFormat == null)
                    singleCharStringFormat = new StringFormat(StringFormatFlags.NoWrap)
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Far,
                    };

                return singleCharStringFormat;
            }
        }
        private StringFormat singleCharStringFormat;

        private class GraphicsContext
        {
            public Graphics Graphics;
            public RectangleF TypingArea;
            public RectangleF DocumentArea;
        }

        #endregion
    }
}
