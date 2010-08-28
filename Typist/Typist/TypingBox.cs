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
        #region Public properties

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

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DefaultValue(true)]
        public bool WordWrap
        {
            get { return wordWrap; }
            set
            {
                wordWrap = value;
                textStringFormat = null;
            }
        }
        private bool wordWrap = true;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event CancelEventHandler DrawingCursor;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event StatusChangedEventHandler StatusChanged;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event VisibleRegionChangedEventHandler HorizontalVisibleRegionChanged;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event VisibleRegionChangedEventHandler VerticalVisibleRegionChanged;

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

        protected virtual void OnHorizontalVisibleRegionChanged(VisibleRegionChangedEventArgs e)
        {
            if (HorizontalVisibleRegionChanged != null)
                HorizontalVisibleRegionChanged(this, e);
        }

        protected virtual void OnVerticalVisibleRegionChanged(VisibleRegionChangedEventArgs e)
        {
            if (VerticalVisibleRegionChanged != null)
                VerticalVisibleRegionChanged(this, e);
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
                drawClipMargins(gc);

                drawCursor(gc);
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

            SizeF cellSize = getCellSize(g, controlBounds);

            RectangleF typingArea = getTypingArea(g, controlBounds, cellSize),
                       documentArea = getDocumentArea(g, typingArea, cellSize);

            return new GraphicsContext()
            {
                Graphics = g,
                ControlBounds = controlBounds,
                CellSize = cellSize,
                TypingArea = typingArea,
                DocumentArea = documentArea,
            };
        }

        private SizeF getCellSize(Graphics g, RectangleF controlBounds)
        {
            RectangleF[] sampleCharAreas = getCharAreas32("mm\nm", new[] { 0, 1, 3 },
                                                          g, getUnlimitedHeightArea(controlBounds));

            return new SizeF(sampleCharAreas[0].Width,
                             sampleCharAreas[2].Y - sampleCharAreas[0].Y);
        }

        private RectangleF getTypingArea(Graphics g, RectangleF controlBounds, SizeF cellSize)
        {
            int charMarginLeftRight = (int)Math.Round(cellSize.Width / 4, 0),
                charMarginTopBottom = (int)Math.Round(cellSize.Height / 8, 0),
                roundedColWidth = (int)Math.Round(cellSize.Width, 0);

            float left = TextMargin.Left + charMarginLeftRight,
                  top = TextMargin.Top + charMarginTopBottom,
                  right = controlBounds.Width - TextMargin.Right - charMarginLeftRight - roundedColWidth,
                  bottom = controlBounds.Height - TextMargin.Bottom - charMarginTopBottom;

            return new RectangleF()
            {
                X = left,
                Y = top,
                Width = Math.Max(right - left, roundedColWidth - 1),
                Height = Math.Max(bottom - top, (float)Math.Ceiling(cellSize.Height) + 3),
            };
        }

        private RectangleF getDocumentArea(Graphics g, RectangleF typingArea, SizeF cellSize)
        {
            RectangleF unlimitedHeightArea = getUnlimitedHeightArea(typingArea);

            RectangleF cursorArea = getCursorCharArea(g, unlimitedHeightArea);
            Point cursorColumnRow = getColumnRow(cursorArea.Location, unlimitedHeightArea.Location, cellSize);

            int currentLineColumns = ImportedText.LineLength(TypedText.Length);

            Size documentColumnsRows = new Size(ImportedText.LongestLineLength,
                                                getDocumentRows(g, unlimitedHeightArea, cellSize));

            Size visibleColumnsRows = getVisibleColumnsRows(typingArea.Size, cellSize);

            float hOffset = cellSize.Width * calcColumnOffset(cursorColumnRow.X, visibleColumnsRows.Width, documentColumnsRows.Width,
                                                              currentLineColumns),
                  vOffset = cellSize.Height * calcRowOffset(cursorColumnRow.Y, visibleColumnsRows.Height, documentColumnsRows.Height);

            return new RectangleF(typingArea.X + hOffset,
                                  typingArea.Y + vOffset,
                                  typingArea.Width,
                                  typingArea.Height - vOffset);
        }

        private int calcColumnOffset(int cursorColumn, int visibleColumns, int documentColumns, int currentLineColumns)
        {
            if (WordWrap)
                return 0;

            int columnOffset = calculateOffset(cursorColumn, visibleColumns, currentLineColumns);

            OnHorizontalVisibleRegionChanged(getVisibleRegionChangedEventArgs(columnOffset, visibleColumns, documentColumns));

            return columnOffset;
        }

        private int calcRowOffset(int cursorRow, int visibleRows, int documentRows)
        {
            int rowOffset = calculateOffset(cursorRow, visibleRows, documentRows);

            OnVerticalVisibleRegionChanged(getVisibleRegionChangedEventArgs(rowOffset, visibleRows, documentRows));

            return rowOffset;
        }

        private int calculateOffset(int cursorLocation, int visibleSize, int documentSize)
        {
            if (documentSize <= visibleSize ||
                cursorLocation <= visibleSize / 2)
                return 0;
            else if (documentSize - 1 - cursorLocation >= visibleSize / 2)
                return -cursorLocation + visibleSize / 2;
            else
                return visibleSize - documentSize;
        }

        private VisibleRegionChangedEventArgs getVisibleRegionChangedEventArgs(int offset, int visibleSize, int documentSize)
        {
            int firstVisibleIndex = -offset,
                lastVisibleIndex = Math.Min(firstVisibleIndex + visibleSize - 1, documentSize - 1),
                totalLength = documentSize;

            return new VisibleRegionChangedEventArgs(firstVisibleIndex, lastVisibleIndex, totalLength);
        }

        private RectangleF getCursorCharArea(Graphics g, RectangleF unlimitedHeightArea)
        {
            return getCharArea(ImportedText.Expanded.ToString() + " ",
                               Math.Min(TypedText.ExpandedLength, ImportedText.Expanded.Length),
                               g, unlimitedHeightArea);
        }

        private int getDocumentRows(Graphics g, RectangleF unlimitedHeightArea, SizeF cellSize)
        {
            if (ImportedText.Expanded.Length > 0)
            {
                RectangleF lastDocumentCharArea = getCharArea(ImportedText.Expanded.ToString(),
                                                              ImportedText.Expanded.Length - 1,
                                                              g, unlimitedHeightArea);

                return getColumnRow(lastDocumentCharArea.Location, unlimitedHeightArea.Location, cellSize).Y + 1;
            }
            else
                return 0;
        }

        private Size getVisibleColumnsRows(SizeF typingAreaSize, SizeF cellSize)
        {
            return new Size(Math.Max(1, (int)Math.Floor((double)typingAreaSize.Width / cellSize.Width)),
                            Math.Max(1, (int)Math.Floor((double)typingAreaSize.Height / cellSize.Height)));
        }

        private Point getColumnRow(PointF charLocation, PointF origin, SizeF cellSize)
        {
            return new Point((int)Math.Round((double)(charLocation.X - origin.X) / cellSize.Width, 0),
                             (int)Math.Round((double)(charLocation.Y - origin.Y) / cellSize.Height, 0));
        }

        private RectangleF getUnlimitedHeightArea(RectangleF area)
        {
            return new RectangleF()
            {
                X = area.X,
                Y = area.Y,
                Width = area.Width,
                Height = float.MaxValue,
            };
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
                    int barCursorWidth = Math.Max(1, 1 + (int)Math.Round(gc.CellSize.Width * BarCursorRelativeWidth, 0));

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

        private void drawClipMargins(GraphicsContext gc)
        {
            gc.Graphics.FillRectangle(Brushes.White, new RectangleF()
            {
                X = 0,
                Y = 0,
                Width = gc.ControlBounds.Width,
                Height = gc.TypingArea.Y + 1,
            });

            gc.Graphics.FillRectangle(Brushes.White, new RectangleF()
            {
                X = 0,
                Y = 0,
                Width = gc.TypingArea.X,
                Height = gc.ControlBounds.Height,
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
                {
                    StringFormatFlags noWrap = WordWrap ? 0 : StringFormatFlags.NoWrap;

                    textStringFormat = new StringFormat(StringFormat.GenericTypographic)
                    {
                        FormatFlags = StringFormat.GenericTypographic.FormatFlags |
                                      StringFormatFlags.MeasureTrailingSpaces |
                                      StringFormatFlags.NoFontFallback |
                                      noWrap,
                    };
                }

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
            public SizeF CellSize;
            public RectangleF TypingArea;
            public RectangleF DocumentArea;
        }

        #endregion
    }
}
