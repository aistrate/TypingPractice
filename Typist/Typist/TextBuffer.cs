using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typist
{
    public class TextBuffer
    {
        public TextBuffer(string text)
        {
            text = text ?? string.Empty;

            buffer = text.ToCharArray();
            Length = text.Length;
        }

        public TextBuffer(TextBuffer original)
        {
            if (original == null)
                throw new ArgumentNullException("original.", "Original TextBuffer cannot be null.");

            Original = original;

            buffer = new char[Math.Max(2000, 2 * original.Length)];
            Length = 0;
        }

        private readonly char[] buffer;

        public TextBuffer Original { get; private set; }

        public int Length { get; private set; }

        public int TotalKeys { get; private set; }

        public int ErrorsCommitted { get; private set; }
        public int ErrorsUncorrected { get; private set; }


        public string Text { get { return new string(buffer, 0, Length); } }

        public char this[int index] { get { return buffer[index]; } }

        public int LastIndex { get { return Length - 1; } }


        public TextBuffer RemoveLast()
        {
            if (Length > 0 && !IsLastSameAsOriginal)
                ErrorsUncorrected--;

            Length = Math.Max(0, Length - 1);

            return this;
        }

        public TextBuffer Append(char ch)
        {
            buffer[Length++] = ch;

            if (!IsLastSameAsOriginal)
            {
                ErrorsCommitted++;
                ErrorsUncorrected++;
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
            get { return TotalKeys - Length + ErrorsUncorrected; }
        }

        public decimal Accuracy
        {
            get { return (decimal)(Length - ErrorsUncorrected) / (decimal)TotalKeys; }
        }

        public int WordCount
        {
            get { return Length / 5; }
        }

        private const string whitespaceChars = " \n\r\t";
    }
}
