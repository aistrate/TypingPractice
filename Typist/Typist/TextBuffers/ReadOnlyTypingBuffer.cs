using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Typist.TextBuffers
{
    public class ReadOnlyTypingBuffer : TypingBuffer
    {
        public ReadOnlyTypingBuffer(string text, bool expandNewlines, bool countWhitespaceAsWordChars,
                                    bool removeEndOfLineSpaces, bool removeMultipleWhitespace)
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
            CountWhitespaceAsWordChars = countWhitespaceAsWordChars;
        }

        public string[] Lines
        {
            get
            {
                if (lines == null)
                    lines = this.ToString().Split('\n');

                return lines;
            }
        }
        private string[] lines;

        public int LongestLineLength
        {
            get
            {
                if (longestLineLength == null)
                    longestLineLength = Lines.Select(l => l.Length)
                                             .Concat(new[] { 0 })
                                             .Max();

                return (int)longestLineLength;
            }
        }
        private int? longestLineLength;

        public int[] LineNumbers
        {
            get
            {
                if (lineNumbers == null)
                {
                    var list = new List<int>();

                    for (int i = 0; i < Lines.Length; i++)
                        list.AddRange(Enumerable.Repeat(i, Lines[i].Length + 1));

                    lineNumbers = list.ToArray();
                }

                return lineNumbers;
            }
        }
        private int[] lineNumbers;

        public int LineLength(int index)
        {
            if (Length > 0)
                return Lines[LineNumbers[index]].Length;
            else
                return 0;
        }

        public TextBuffer Expanded
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

        protected TextBuffer ActualExpanded
        {
            get
            {
                if (actualExpanded == null)
                    actualExpanded = new TextBuffer(this.ToString().Replace("\n", string.Format("{0}\n", pilcrow)));

                return actualExpanded;
            }
        }
        private TextBuffer actualExpanded;

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
