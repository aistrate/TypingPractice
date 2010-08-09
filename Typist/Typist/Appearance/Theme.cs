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
            Font = new Font(FontNames.FixedWidth.CourierNew, 10, FontStyle.Regular);

            ImportedTextColor = Brushes.Black;
            TypedTextColor = new SolidBrush(VsColors.UserTypes);
            ErrorBackColor = new SolidBrush(VsColors.SelectedTextBackColor);
            ErrorForeColor = new SolidBrush(VsColors.StringLiteral);
            CursorColor = Brushes.Crimson;

            BarCursorLineWidth = 2;

            BeepOnError = true;
        }

        public Theme(Theme original)
        {
            Font = (Font)original.Font.Clone();

            ImportedTextColor = original.ImportedTextColor;
            TypedTextColor = original.TypedTextColor;
            ErrorBackColor = original.ErrorBackColor;
            ErrorForeColor = original.ErrorForeColor;
            CursorColor = original.CursorColor;

            BarCursorLineWidth = original.BarCursorLineWidth;

            BeepOnError = original.BeepOnError;
        }

        public Theme(Theme original, string fontName)
            : this(original)
        {
            FontName = fontName;
        }

        public Font Font { get; set; }

        public Brush ImportedTextColor { get; set; }
        public Brush TypedTextColor { get; set; }
        public Brush ErrorBackColor { get; set; }
        public Brush ErrorForeColor { get; set; }
        public Brush CursorColor { get; set; }

        public int BarCursorLineWidth { get; set; }

        public bool BeepOnError { get; set; }


        public string FontName
        {
            get { return Font.FontFamily.Name; }
            set { Font = new Font(value, Font.Size, Font.Style); }
        }

        public float FontSize
        {
            get { return Font.Size; }
            set { Font = new Font(Font.FontFamily, value, Font.Style); }
        }

        public FontStyle FontStyle
        {
            get { return Font.Style; }
            set { Font = new Font(Font.FontFamily, Font.Size, value); }
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
            ErrorBackColor = Brushes.LightGray,
            CursorColor = Brushes.Black,
            BeepOnError = false,
        };
    }
}
