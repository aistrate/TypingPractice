using System.Linq;
using System.Text.RegularExpressions;

namespace Typist.TextBuffers
{
    public class ReadOnlyTypingBuffer : TypingBuffer
    {
        public ReadOnlyTypingBuffer(string text, bool expandNewlines, bool removeEndOfLineSpaces, bool removeMultipleWhitespace)
        {
            string allowedWhitespace = " \n";

            text = text ?? string.Empty;

            text = text.Replace("\r\n", "\n")
                       .Replace("\t", "    ")
                       .Where(c => !char.IsWhiteSpace(c) || allowedWhitespace.IndexOf(c) >= 0)
                       .AsString()
                       .TrimEnd(allowedWhitespace.ToCharArray());

            if (removeEndOfLineSpaces)
                text = Regex.Replace(text, @" +\n", "\n");

            if (removeMultipleWhitespace)
            {
                text = Regex.Replace(text, @" {2,}", " ");
                text = Regex.Replace(text, @"(^|(?<=.\n))\n{2,}", "\n");
            }

            Buffer = text.ToCharArray();
            Length = text.Length;

            ExpandNewlines = expandNewlines;
        }

        public virtual bool ExpandNewlines { get; set; }

        public TypingBuffer Expanded
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

        protected TypingBuffer ActualExpanded
        {
            get
            {
                if (actualExpanded == null)
                    actualExpanded = new TypingBuffer(this.ToString().Replace("\n", string.Format("{0}\n", pilcrow)));

                return actualExpanded;
            }
        }
        private TypingBuffer actualExpanded;

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
