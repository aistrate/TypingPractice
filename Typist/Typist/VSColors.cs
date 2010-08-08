using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Typist
{
    /// <summary>
    /// The standard Visual Studio editor colors.
    /// </summary>
    public static class VsColors
    {
        public static Color StringLiteral { get { return Color.FromArgb(163, 21, 21); } }               // dark brown
        public static Color Comment { get { return Color.FromArgb(0, 128, 0); } }                       // same as Color.Green
        public static Color Keyword { get { return Color.FromArgb(0, 0, 255); } }                       // same as Color.Blue
        public static Color UserTypes { get { return Color.FromArgb(43, 145, 175); } }                  // light blue

        public static Color SelectedTextBackColor { get { return Color.FromArgb(178, 180, 191); } }     // same as KnownColor.Highlight
        public static Color InactiveSelectedText { get { return Color.FromArgb(162, 161, 161); } }      // same as KnownColor.InactiveCaptionText

        public static Color BookmarkBackColor { get { return Color.FromArgb(191, 210, 249); } }         // pale blue

        // Debug Mode
        public static Color BreakpointBackColor { get { return Color.FromArgb(150, 58, 70); } }         // dark brown
        public static Color CallReturnBackColor { get { return Color.FromArgb(180, 228, 180); } }       // light green
        public static Color CurrentStatementBackColor { get { return Color.FromArgb(255, 238, 98); } }  // bright yellow
    }
}
