using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Typist.Appearance;
using Typist.TextBuffers;

namespace Typist
{
    public class TypingBox : PictureBox
    {
        #region Properties

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ReadOnlyTypingBuffer ImportedText { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ReadWriteTypingBuffer TypedText { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Font TypingFont { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Theme Theme { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0f)]
        public float BarCursorRelativeWidth { get; set; }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Padding TextMargin { get; set; }

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
        [DefaultValue(0f)]
        public float BarCursorVOffset { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0f)]
        public float CharCursorVOffset { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(0f)]
        public float ErrorBackgroundVOffset { get; set; }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event CancelEventHandler DrawingCursor;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event StatusChangedEventHandler StatusChanged;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event VisibleRegionChangedEventHandler VisibleRegionChanged;

        #endregion


        #region General

        public TypingBox()
        {
            CursorAsVerticalBar = true;
            CharCursorChar = '_';
            TextMargin = new Padding(0);
        }

        protected override void OnResize(EventArgs e)
        {
            this.Invalidate();
            base.OnResize(e);
        }

        protected virtual void OnDrawingCursor(CancelEventArgs e)
        {
            if (DrawingCursor != null)
                DrawingCursor(this, e);
        }

        protected virtual void OnStatusChanged(StatusChangedEventArgs e)
        {
            if (StatusChanged != null)
                StatusChanged(this, e);
        }

        protected void ShowStatusMessage(string message)
        {
            OnStatusChanged(new StatusChangedEventArgs(message));
        }

        protected void ShowStatusMessage(string format, params object[] args)
        {
            ShowStatusMessage(string.Format(format, args));
        }

        protected virtual void OnVisibleRegionChanged(VisibleRegionChangedEventArgs e)
        {
            if (VisibleRegionChanged != null)
                VisibleRegionChanged(this, e);
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
            RectangleF controlBounds = new RectangleF()
            {
                X = 0,
                Y = 0,
                Width = this.Width,
                Height = this.Height,
            };

            RectangleF[] sampleCharAreas = getCharAreas32("mm\nm", new[] { 0, 1, 3 },
                                                          g, unlimitedHeightArea(controlBounds));

            float rowHeight = sampleCharAreas[2].Y - sampleCharAreas[0].Y;

            int charMarginLeftRight = (int)Math.Round(sampleCharAreas[0].Width / 4, 0),
                charMarginTopBottom = (int)Math.Round(sampleCharAreas[0].Height / 8, 0),
                averageCharWidth = (int)Math.Round(sampleCharAreas[1].Width, 0);

            float left = TextMargin.Left + charMarginLeftRight,
                  top = TextMargin.Top + charMarginTopBottom,
                  right = controlBounds.Width - TextMargin.Right - charMarginLeftRight - averageCharWidth,
                  bottom = controlBounds.Height - TextMargin.Bottom - charMarginTopBottom;

            RectangleF typingArea = new RectangleF()
            {
                X = left,
                Y = top,
                Width = Math.Max(right - left, averageCharWidth - 1),
                Height = Math.Max(bottom - top, (float)Math.Ceiling(rowHeight) + 3),
            };

            float vOffset = rowHeight * calculateRowOffset(g, typingArea, rowHeight);

            return new GraphicsContext()
            {
                Graphics = g,
                ControlBounds = controlBounds,
                FirstCharArea = sampleCharAreas[0],
                TypingArea = typingArea,
                DocumentArea = new RectangleF(typingArea.X, typingArea.Y + vOffset,
                                              typingArea.Width, typingArea.Height - vOffset),
            };
        }

        private int calculateRowOffset(Graphics g, RectangleF typingArea, float rowHeight)
        {
            if (ImportedText.Length == 0 || rowHeight == 0)
            {
                OnVisibleRegionChanged(new VisibleRegionChangedEventArgs(0, -1, 0));
                return 0;
            }

            RectangleF unlimitedArea = unlimitedHeightArea(typingArea);

            RectangleF lastDocumentCharArea = getCharArea(ImportedText.Expanded.ToString(),
                                                          ImportedText.Expanded.Length - 1,
                                                          g, unlimitedArea);

            int lastDocumentRow = getRow(lastDocumentCharArea.Y, unlimitedArea.Y, rowHeight);

            int cursorRow = lastDocumentRow;

            if (TypedText.LastIndex < ImportedText.LastIndex)
            {
                RectangleF cursorCharArea = getCharArea(ImportedText.Expanded.ToString(),
                                                        TypedText.ExpandedLength,
                                                        g, unlimitedArea);

                cursorRow = getRow(cursorCharArea.Y, unlimitedArea.Y, rowHeight);
            }

            int visibleRows = Math.Max(1, (int)Math.Floor((double)typingArea.Height / rowHeight));

            int rowOffset;

            if (lastDocumentRow < visibleRows ||
                cursorRow <= visibleRows / 2)
                rowOffset = 0;
            else if (lastDocumentRow - cursorRow >= visibleRows / 2)
                rowOffset = -cursorRow + visibleRows / 2;
            else
                rowOffset = visibleRows - 1 - lastDocumentRow;

            int firstVisibleRow = -rowOffset,
                lastVisibleRow = Math.Min(firstVisibleRow + visibleRows - 1, lastDocumentRow),
                totalRowCount = lastDocumentRow + 1;

            OnVisibleRegionChanged(new VisibleRegionChangedEventArgs(firstVisibleRow, lastVisibleRow, totalRowCount));

            return rowOffset;
        }

        private RectangleF unlimitedHeightArea(RectangleF area)
        {
            return new RectangleF()
            {
                X = area.X,
                Y = area.Y,
                Width = area.Width,
                Height = float.MaxValue,
            };
        }

        private int getRow(float charY, float typingAreaY, float rowHeight)
        {
            return (int)Math.Round((double)(charY - typingAreaY) / rowHeight, 0);
        }

        private void drawImportedText(GraphicsContext gc)
        {
            drawText(ImportedText.Expanded.ToString(), gc, Theme.ImportedTextColor);
        }

        private void drawShadowText(GraphicsContext gc)
        {
            string shadowText = ImportedText.Expanded.Substring(0, TypedText.ExpandedLength);

            if (TypedText.Length > 0)
            {
                string importedText = ImportedText.Expanded.ToString();
                int lastIndex = TypedText.ExpandedLength - 1;

                int lineJumpIndex = findLineJump(importedText, lastIndex, gc);
                if (lineJumpIndex < lastIndex)
                    shadowText = insertSpacesAfterJump(importedText, shadowText,
                                                       lineJumpIndex, lastIndex,
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
            RectangleF[] errorCharAreas = getCharAreas(ImportedText.Expanded.ToString(),
                                                       TypedText.ExpandedErrorsUncorrected,
                                                       gc);

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
                RectangleF cursorArea = getCharArea(ImportedText.Expanded.ToString(),
                                                    TypedText.ExpandedLength, gc);

                cursorArea = fracOffsetCharArea(cursorArea, 0,
                                                CursorAsVerticalBar ? BarCursorVOffset : CharCursorVOffset);

                if (CursorAsVerticalBar)
                {
                    int barCursorWidth = Math.Max(1, 1 + (int)Math.Round(gc.FirstCharArea.Width * BarCursorRelativeWidth, 0));

                    gc.Graphics.FillRectangle(Theme.CursorColor, new RectangleF()
                    {
                        X = cursorArea.X - (0.125f * cursorArea.Width) - (0.5f * barCursorWidth),
                        Y = cursorArea.Y,
                        Width = barCursorWidth,
                        Height = cursorArea.Height,
                    });
                }
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

        private const char pilcrow = '\xB6';

        private void drawChar(char ch, GraphicsContext gc, Brush brush, RectangleF charArea)
        {
            if (ch == '\n')
                ch = pilcrow;

            gc.Graphics.DrawString(ch.ToString(), TypingFont, brush, charArea, SingleCharStringFormat);
        }

        private void drawText(string text, GraphicsContext gc, Brush brush)
        {
            gc.Graphics.DrawString(text, TypingFont, brush, gc.DocumentArea, TextStringFormat);
        }

        private RectangleF getCharArea(string text, int charIndex, GraphicsContext gc)
        {
            return getCharArea(text, charIndex, gc.Graphics, gc.DocumentArea);
        }

        private RectangleF getCharArea(string text, int charIndex, Graphics g, RectangleF documentArea)
        {
            return getCharAreas(text, new[] { charIndex }, g, documentArea).First();
        }

        private RectangleF[] getCharAreas(string text, int[] charIndexes, GraphicsContext gc)
        {
            return getCharAreas(text, charIndexes, gc.Graphics, gc.DocumentArea);
        }

        private RectangleF[] getCharAreas(string text, int[] charIndexes, Graphics g, RectangleF documentArea)
        {
            charIndexes = charIndexes.Where(i => i < text.Length)
                                     .ToArray();

            RectangleF[] charAreas = charIndexes.Split(32)
                                                .SelectMany(grp => getCharAreas32(text, grp.ToArray(), g, documentArea))
                                                .ToArray();

            return charAreas;
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
                Width = gc.ControlBounds.Width,
                Height = gc.TypingArea.Y + 1,
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
                    textStringFormat = new StringFormat(StringFormat.GenericTypographic)
                    {
                        FormatFlags = StringFormat.GenericTypographic.FormatFlags |
                                      StringFormatFlags.MeasureTrailingSpaces,
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
            public RectangleF ControlBounds;
            public RectangleF FirstCharArea;
            public RectangleF TypingArea;
            public RectangleF DocumentArea;
        }

        #endregion
    }
}
