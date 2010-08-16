using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Typist.Appearance
{
    public class FontInfo
    {
        public FontInfo(string name, float size)
            : this(name, size, FontStyle.Regular) { }

        public FontInfo(string name, float size, GraphicsUnit unit)
            : this(name, size, FontStyle.Regular, unit) { }

        public FontInfo(string name, float size, FontStyle style)
            : this(name, size, style, GraphicsUnit.Point) { }

        public FontInfo(string name, float size, FontStyle style, GraphicsUnit unit)
        {
            Name = name;
            Size = size;
            Style = style;
            Unit = unit;
        }

        public string Name { get; private set; }
        public float Size { get; private set; }
        public FontStyle Style { get; private set; }
        public GraphicsUnit Unit { get; private set; }

        public bool IsAvailable
        {
            get
            {
                if (isAvailable == null)
                    isAvailable = Array.IndexOf(fontFamilyNames, Name.ToLower()) >= 0 &&
                                  new FontFamily(Name).IsStyleAvailable(Style);

                return (bool)isAvailable;
            }
        }
        private bool? isAvailable = null;

        private static string[] fontFamilyNames = FontFamily.Families
                                                            .Select(f => f.Name.ToLower())
                                                            .ToArray();

        public Font Font
        {
            get
            {
                if (font == null)
                {
                    if (IsAvailable)
                        font = new Font(Name, Size, Style, Unit);
                    else
                        font = new Font(FontFamily.GenericMonospace, Size, Style, Unit);
                }

                return font;
            }
        }
        private Font font;

        public string Description
        {
            get { return GetDescription(Font); }
        }

        public string OriginalDescription
        {
            get
            {
                if (IsAvailable)
                    return Description;
                else
                    return GetDescription(Name, Size, Style, Unit);
            }
        }

        public static string GetDescription(Font font)
        {
            return GetDescription(font.Name, font.Size, font.Style, font.Unit);
        }

        public static string GetDescription(string name, float size, FontStyle style, GraphicsUnit unit)
        {
            return string.Format("{0} ({1} {2}{3})",
                                 name,
                                 size, unit.ToString().ToLower(),
                                 style != FontStyle.Regular ? ", " + style.ToString().ToLower() : "");
        }

        public FontInfo ToSizeInPoints()
        {
            return new FontInfo(Font.Name, Font.SizeInPoints, Font.Style, GraphicsUnit.Point);
        }
    }
}
