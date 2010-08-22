using System.Linq;
using System.Text.RegularExpressions;

namespace Typist
{
    public class ReadOnlyTextBuffer : TextBuffer
    {
        public ReadOnlyTextBuffer(string text, bool removeMultipleWhitespace, bool expandNewlines, bool countWhitespaceAsWordChars)
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

            ExpandNewlines = expandNewlines;
            CountWhitespaceAsWordChars = countWhitespaceAsWordChars;
        }

        public ReadOnlyTextBuffer Expanded
        {
            get
            {
                if (ExpandNewlines)
                    return ActualExpanded;
                else
                    return this;
            }
        }

        public int ExpandedIndex(int index)
        {
            if (ExpandNewlines)
                return ActualExpandedIndexes[index];
            else
                return index;
        }

        private const char pilcrow = '\xB6';

        protected ReadOnlyTextBuffer ActualExpanded
        {
            get
            {
                if (actualExpanded == null)
                {
                    string actualExpandedText = this.ToString().Replace("\n", string.Format("{0}\n", pilcrow));

                    actualExpanded = new ReadOnlyTextBuffer(actualExpandedText, false, false, false);
                }

                return actualExpanded;
            }
        }
        private ReadOnlyTextBuffer actualExpanded;

        protected int[] ActualExpandedIndexes
        {
            get
            {
                if (actualExpandedIndexes == null)
                {
                    actualExpandedIndexes = new int[Length + 1];

                    for (int i = 0, k = 0; i <= Length; i++, k++)
                    {
                        actualExpandedIndexes[i] = k;

                        if (i < Length && Buffer[i] == '\n')
                            k++;
                    }
                }

                return actualExpandedIndexes;
            }
        }
        private int[] actualExpandedIndexes;
    }
}
