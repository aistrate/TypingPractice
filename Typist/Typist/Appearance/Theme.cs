using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Typist.Appearance
{
    public class Theme
    {
        private Theme()
        {
            TypingFont = new Font(FontNames.FixedWidth.CourierNew, 10, FontStyle.Regular);

            ImportedTextColor = Brushes.Black;
            TypedTextColor = new SolidBrush(VsColors.UserTypes);
            ErrorBackColor = new SolidBrush(VsColors.SelectedTextBackColor);
            ErrorForeColor = new SolidBrush(VsColors.StringLiteral);
            CursorColor = Brushes.Crimson;

            BarCursorLineWidth = 2;
        }

        public Theme(Theme original)
        {
            TypingFont = (Font)original.TypingFont.Clone();

            ImportedTextColor = original.ImportedTextColor;
            TypedTextColor = original.TypedTextColor;
            ErrorBackColor = original.ErrorBackColor;
            ErrorForeColor = original.ErrorForeColor;
            CursorColor = original.CursorColor;

            BarCursorLineWidth = original.BarCursorLineWidth;
        }

        public Theme(Theme original, string fontName)
            : this(original)
        {
            FontName = fontName;
        }

        public Font TypingFont { get; set; }

        public Brush ImportedTextColor { get; set; }
        public Brush TypedTextColor { get; set; }
        public Brush ErrorBackColor { get; set; }
        public Brush ErrorForeColor { get; set; }
        public Brush CursorColor { get; set; }

        public int BarCursorLineWidth { get; set; }


        public string FontName
        {
            get { return TypingFont.FontFamily.Name; }
            set { TypingFont = new Font(value, TypingFont.Size, TypingFont.Style); }
        }

        public float FontSize
        {
            get { return TypingFont.Size; }
            set { TypingFont = new Font(TypingFont.FontFamily, value, TypingFont.Style); }
        }

        public FontStyle FontStyle
        {
            get { return TypingFont.Style; }
            set { TypingFont = new Font(TypingFont.FontFamily, TypingFont.Size, value); }
        }


        public static Theme Default = new Theme();

        public static Theme DefaultLarge = new Theme(Default)
        {
            FontSize = 16,
            FontStyle = FontStyle.Bold,
            BarCursorLineWidth = 3,
        };

        public static Theme Discreet = new Theme(Default)
        {
            CursorColor = Brushes.Black,
        };
    }
}
