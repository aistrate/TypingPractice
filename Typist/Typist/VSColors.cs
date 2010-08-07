using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Typist
{
    public static class VSColors
    {
        public static Color String { get { return Color.FromArgb(163, 21, 21); } }        // dark brown
        public static Color Comment { get { return Color.FromArgb(0, 128, 0); } }         // same as Color.Green
        public static Color Keyword { get { return Color.FromArgb(0, 0, 255); } }         // same as Color.Blue
        public static Color UserTypes { get { return Color.FromArgb(43, 145, 175); } }    // light blue

        public static Color SelectedTextBackground { get { return Color.FromArgb(178, 180, 191); } } // same as KnownColor.Highlight
        public static Color InactiveSelectedText { get { return Color.FromArgb(162, 161, 161); } }   // same as KnownColor.InactiveCaptionText

        public static Color BookmarkBackground { get { return Color.FromArgb(191, 210, 249); } }        // pale blue

        // Debug Mode
        public static Color BreakpointBackground { get { return Color.FromArgb(150, 58, 70); } }        // dark brown
        public static Color CallReturnBackground { get { return Color.FromArgb(180, 228, 180); } }      // light green
        public static Color CurrentStatementBackground { get { return Color.FromArgb(255, 238, 98); } } // bright yellow
    }
}
