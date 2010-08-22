using System.Linq;
using System.Text.RegularExpressions;

namespace Typist
{
    public class ReadOnlyTextBuffer : TextBuffer
    {
        public ReadOnlyTextBuffer(string text, bool removeMultipleWhitespace, bool visibleNewlines, bool countWhitespaceAsWordChars)
        {
            string allowedWhitespace = " \n";

            text = text ?? string.Empty;

            text = text.Replace("\r\n", "\n")
                       .Replace("\t", "    ")
                       .Where(c => !char.IsWhiteSpace(c) || allowedWhitespace.IndexOf(c) >= 0)
                       .AsString()
                       .TrimEnd(allowedWhitespace.ToCharArray());

            if (removeMultipleWhitespace)
            {
                text = Regex.Replace(text, @" +", " ");
                text = Regex.Replace(text, @" +\n", "\n");
                text = Regex.Replace(text, @"(^|(?<=.\n))\n{2,}", "\n");
            }

            Buffer = text.ToCharArray();
            Length = text.Length;

            VisibleNewlines = visibleNewlines;
            CountWhitespaceAsWordChars = countWhitespaceAsWordChars;
        }
    }
}
