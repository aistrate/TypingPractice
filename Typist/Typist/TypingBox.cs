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
        public Theme Theme { get; set; }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(true)]
        [Description("Determines whether the cursor will be a vertical bar (true), or a character (false).")]
        public bool CursorAsVerticalBar { get; set; }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue('_')]
        [Description("The character that will represent the cursor, " +
                     "if in character cursor mode (as opposed to vertical bar cursor mode).")]
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

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Description("If true, newline characters will be made visible as paragraph signs (pilcrows).")]
        public bool VisibleNewlines { get; set; }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Description("Determines whether the cursor is visible when typing is paused, " +
                     "or only when the user is actually typing (practice mode).")]
        public bool ShowCursorWhenPaused { get; set; }

        #endregion


        #region General

        public TypingBox()
        {
            CursorAsVerticalBar = true;
            CharCursorChar = '_';
        }

        public event CancelEventHandler DrawingCursor;

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

            RectangleF firstCharArea = getCharAreas32("_", new[] { 0 }, g, typingArea).First();

            typingArea.Width -= firstCharArea.Width;

            return new GraphicsContext()
            {
                Graphics = g,
                TypingArea = typingArea,
                FirstCharArea = firstCharArea,
            };
        }

        private void drawImportedText(GraphicsContext gc)
        {
            drawText(ImportedText.ToString(), gc, Theme.ImportedTextColor);
        }

        private void drawShadowText(GraphicsContext gc)
        {
            string shadowText = ImportedText.Substring(0, TypedText.Length);

            int lineJumpIndex = findLineJump(ImportedText.ToString(), TypedText.LastIndex, gc);
            if (lineJumpIndex < TypedText.LastIndex)
                shadowText = insertSpacesAfterJump(ImportedText.ToString(), shadowText,
                                                   lineJumpIndex, TypedText.LastIndex,
                                                   gc);

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
                        X = cursorArea.X - (0.125f * cursorArea.Width) - (0.5f * Theme.BarCursorLineWidth),
                        Y = cursorArea.Y,
                        Width = Theme.BarCursorLineWidth,
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

            gc.Graphics.DrawString(ch.ToString(), Theme.Font, brush, charArea, SingleCharStringFormat);
        }

        private void drawText(string text, GraphicsContext gc, Brush brush)
        {
            if (VisibleNewlines)
                text = text.Replace("\n", string.Format("{0}\n", pilcrow));

            gc.Graphics.DrawString(text, Theme.Font, brush, gc.TypingArea, TextStringFormat);
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
            return getCharAreas32(text, charIndexes, gc.Graphics, gc.TypingArea);
        }

        private RectangleF[] getCharAreas32(string text, int[] charIndexes, Graphics g, RectangleF typingArea)
        {
            CharacterRange[] ranges = charIndexes.Select(i => new CharacterRange(i, 1)).ToArray();

            StringFormat stringFormat = new StringFormat(TextStringFormat);
            stringFormat.SetMeasurableCharacterRanges(ranges);

            Region[] regions = g.MeasureCharacterRanges(text, Theme.Font, typingArea, stringFormat);

            return regions.Select(r => r.GetBounds(g))
                          .ToArray();
        }

        private void drawDebugMessage(string message, GraphicsContext gc)
        {
            RectangleF position = new RectangleF(gc.TypingArea.X, gc.TypingArea.Y + gc.TypingArea.Height - 16, 
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
            public RectangleF FirstCharArea;
        }

        #endregion
    }
}
