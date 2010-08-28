using System;

namespace Typist.TextBuffers
{
    public class TextBuffer
    {
        public TextBuffer(string text)
        {
            text = text ?? string.Empty;

            Buffer = text.ToCharArray();
            Length = text.Length;
        }

        protected TextBuffer() { }

        protected char[] Buffer;

        public int Length { get; protected set; }

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

        public int CountSpaces(int index)
        {
            int spaces = 0;

            for (int i = index; i < Length && this[i] == ' '; i++)
                spaces++;

            return spaces;
        }
    }
}
