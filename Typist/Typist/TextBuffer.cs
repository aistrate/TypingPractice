using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typist
{
    public class TextBuffer
    {
        public TextBuffer(string text)
            : this()
        {
            text = text ?? string.Empty;

            buffer = text.ToCharArray();
            Length = text.Length;
        }

        public TextBuffer(TextBuffer original)
            : this()
        {
            if (original == null)
                throw new ArgumentNullException("original.", "Original TextBuffer cannot be null.");

            Original = original;

            buffer = new char[Math.Max(2000, 2 * original.Length)];
            Length = 0;
        }

        private TextBuffer()
        {
            ErrorsUncorrected = new List<int>();
        }

        private readonly char[] buffer;

        public TextBuffer Original { get; private set; }

        public int Length { get; private set; }

        public int TotalKeys { get; private set; }

        public int ErrorsCommitted { get; private set; }

        public List<int> ErrorsUncorrected { get; private set; }


        public string Text { get { return new string(buffer, 0, Length); } }

        public string Substring(int startIndex)
        {
            return new string(buffer, startIndex, Length - startIndex);
        }

        public string Substring(int startIndex, int length)
        {
            return new string(buffer, startIndex, Math.Min(length, Length - startIndex));
        }

        public char this[int index] { get { return buffer[index]; } }

        public int LastIndex { get { return Length - 1; } }


        public TextBuffer RemoveLast()
        {
            if (Length > 0 && !IsLastSameAsOriginal)
                ErrorsUncorrected.RemoveAt(ErrorsUncorrected.Count - 1);

            Length = Math.Max(0, Length - 1);

            return this;
        }

        public TextBuffer Append(char ch)
        {
            buffer[Length++] = ch;

            if (!IsLastSameAsOriginal)
            {
                ErrorsCommitted++;
                ErrorsUncorrected.Add(LastIndex);

                if (ch == '\n')
                    buffer[LastIndex] = '\xB6';
            }

            return this;
        }

        public bool IsLastSameAsOriginal
        {
            get { return 0 < Length && Length <= Original.Length && this[LastIndex] == Original[LastIndex]; }
        }

        public TextBuffer ProcessKey(char ch)
        {
            if (ch == '\b')
                return RemoveLast();

            TotalKeys++;

            if (ch == '\r')
                return Append('\n');
            else
                return Append(ch);
        }

        public int TotalErrors
        {
            get { return TotalKeys - Length + ErrorsUncorrected.Count; }
        }

        public decimal Accuracy
        {
            get { return TotalKeys > 0 ? (decimal)(Length - ErrorsUncorrected.Count) / (decimal)TotalKeys : 1m; }
        }

        public int WordCount
        {
            get { return Length / 5; }
        }

        private const string whitespaceChars = " \n\r\t";
    }
}
