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

            this.original = original;

            buffer = new char[Math.Max(2000, 2 * original.Length)];
            Length = 0;
        }

        private readonly char[] buffer;
        private readonly TextBuffer original = null;

        public int Length { get; private set; }

        public string Text { get { return new string(buffer, 0, Length); } }


        public TextBuffer RemoveLast()
        {
            Length = Math.Max(0, Length - 1);
            return this;
        }

        public TextBuffer Append(char ch)
        {
            buffer[Length++] = ch;

            return this;
        }

        public TextBuffer Append(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                s.CopyTo(0, buffer, Length, s.Length);
                Length += s.Length;
            }

            return this;
        }

        public TextBuffer ProcessKey(char ch)
        {
            switch (ch)
            {
                case '\b':
                    return RemoveLast();
                case '\r':
                    return Append('\n');
                default:
                    return Append(ch);
            }
        }

        public int WordCount
        {
            get { return Length / 5; }
        }

        private const string whitespaceChars = " \n\r\t";
    }
}
