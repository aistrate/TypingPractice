using System;
using System.Collections.Generic;
using System.Linq;

namespace Typist.TextBuffers
{
    public class TypingBuffer
    {
        public TypingBuffer(string text)
        {
            text = text ?? string.Empty;

            Buffer = text.ToCharArray();
            Length = text.Length;
        }

        protected TypingBuffer() { }

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


        public virtual string[] Lines
        {
            get { return lines ?? (lines = GetLines()); }
        }
        private string[] lines;

        public virtual int LongestLineLength
        {
            get { return (int)(longestLineLength ?? (longestLineLength = GetLongestLineLength())); }
        }
        private int? longestLineLength;

        public virtual int[] LineNumbers
        {
            get { return lineNumbers ?? (lineNumbers = GetLineNumbers()); }
        }
        private int[] lineNumbers;


        protected string[] GetLines()
        {
            return this.ToString().Split('\n');
        }

        protected int GetLongestLineLength()
        {
            return Lines.Select(l => l.Length)
                        .Concat(new[] { 0 })
                        .Max();
        }

        protected int[] GetLineNumbers()
        {
            var list = new List<int>();

            for (int i = 0; i < Lines.Length; i++)
                list.AddRange(Enumerable.Repeat(i, Lines[i].Length + 1));

            return list.ToArray();
        }

        public int LineLength(int index)
        {
            if (Length > 0)
                return Lines[LineNumbers[index]].Length;
            else
                return 0;
        }
    }
}
