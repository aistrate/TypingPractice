using System;

namespace Typist
{
    public abstract class TextBuffer
    {
        protected TextBuffer()
        {
        }

        protected char[] Buffer;

        public int Length { get; protected set; }

        public virtual bool VisibleNewlines { get; set; }

        public virtual bool CountWhitespaceAsWordChars { get; set; }


        public char this[int index] { get { return Buffer[index]; } }

        public int LastIndex { get { return Length - 1; } }


        public string Substring(int startIndex)
        {
            return new string(Buffer, startIndex, Length - startIndex);
        }

        public string Substring(int startIndex, int length)
        {
            return new string(Buffer, startIndex, Math.Min(length, Length - startIndex));
        }

        public override string ToString()
        {
            return new string(Buffer, 0, Length);
        }


        protected virtual bool IsWordChar(int index, char c)
        {
            return CountWhitespaceAsWordChars || !char.IsWhiteSpace(c);
        }

        public int WordCount
        {
            get { return Count(IsWordChar) / 5; }
        }

        public int Count(Func<char, bool> predicate)
        {
            return Count((i, c) => predicate(c));
        }

        public int Count(Func<int, char, bool> predicate)
        {
            int count = 0;

            for (int i = 0; i < Length; i++)
                if (predicate(i, this[i]))
                    count++;

            return count;
        }
    }
}
