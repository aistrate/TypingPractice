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

            CountWhitespaceAsWordChars = true;
        }

        public TextBuffer(TextBuffer original, bool countWhitespaceAsWordChars)
            : this()
        {
            if (original == null)
                throw new ArgumentNullException("original.", "Original TextBuffer cannot be null.");

            Original = original;

            buffer = new char[Math.Max(2000, 2 * original.Length)];
            Length = 0;

            CountWhitespaceAsWordChars = countWhitespaceAsWordChars;
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

        public bool CountWhitespaceAsWordChars { get; set; }


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

            if (Length >= Original.Length)
                return this;

            TotalKeys++;

            if (ch == '\r')
                return Append('\n');
            else if (ch == '\t')
                return ExpandTab();
            else
                return Append(ch);
        }

        public TextBuffer ExpandTab()
        {
            Append(' ');

            if (IsLastSameAsOriginal)
                for (int i = LastIndex + 1; i < Original.Length && Original[i] == ' '; i++)
                {
                    TotalKeys++;
                    Append(' ');
                }

            return this;
        }

        public bool IsAtBeginningOfLine(int index)
        {
            int i;
            for (i = index; i >= 0 && this[i] == ' '; i--)
                ;

            return i < 0 || (this[i] == '\n');
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
            get
            {
                if (CountWhitespaceAsWordChars)
                    return Length / 5;
                else
                    return Count(c => whitespaceChars.IndexOf(c) < 0) / 5;
            }
        }

        public int Count(Func<char, bool> predicate)
        {
            return Array.FindAll(Active, c => predicate(c)).Length;
        }

        public char[] Active
        {
            get
            {
                char[] active = new char[Length];
                Array.Copy(buffer, 0, active, 0, Length);
                return active;
            }
        }

        private const string whitespaceChars = " \n\r\t";
    }
}
