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
            ImportedTextColor = Brushes.Black;
            TypedTextColor = new SolidBrush(VsColors.UserTypes);
            ErrorBackColor = new SolidBrush(VsColors.SelectedTextBackColor);
            ErrorForeColor = Brushes.DarkSlateGray;
            CursorColor = Brushes.Crimson;
        }

        public Theme(Theme original)
        {
            ImportedTextColor = original.ImportedTextColor;
            TypedTextColor = original.TypedTextColor;
            ErrorBackColor = original.ErrorBackColor;
            ErrorForeColor = original.ErrorForeColor;
            CursorColor = original.CursorColor;
        }

        public Brush ImportedTextColor { get; set; }
        public Brush TypedTextColor { get; set; }
        public Brush ErrorBackColor { get; set; }
        public Brush ErrorForeColor { get; set; }
        public Brush CursorColor { get; set; }

        public static Theme Default = new Theme();
    }
}
