using System;
using System.Collections.Generic;
using System.Linq;

namespace Typist.TextBuffers
{
    public class ReadWriteTypingBuffer : TypingBuffer
    {
        public ReadWriteTypingBuffer(ReadOnlyTypingBuffer original, bool countErrorsAsWordChars)
        {
            if (original == null)
                throw new ArgumentNullException("original.", "Original TextBuffer cannot be null.");

            Original = original;

            Buffer = new char[Math.Max(2000, 2 * original.Length)];
            Length = 0;

            CountErrorsAsWordChars = countErrorsAsWordChars;

            ErrorsUncorrected = new List<int>();
        }

        public ReadOnlyTypingBuffer Original { get; private set; }

        public override bool ExpandNewlines
        {
            get { return Original.ExpandNewlines; }
            set { Original.ExpandNewlines = value; }
        }

        public override bool CountWhitespaceAsWordChars
        {
            get { return Original.CountWhitespaceAsWordChars; }
            set { Original.CountWhitespaceAsWordChars = value; }
        }

        public bool CountErrorsAsWordChars { get; set; }

        public int ForwardKeys { get; private set; }

        public int BackspaceKeys { get; private set; }

        public int ErrorsCommitted { get; private set; }

        public List<int> ErrorsUncorrected { get; private set; }

        public override string[] Lines { get { return GetLines(); } }

        public override int LongestLineLength { get { return GetLongestLineLength(); } }

        public override int[] LineNumbers { get { return GetLineNumbers(); } }

        public ReadWriteTypingBuffer RemoveLast()
        {
            if (Length > 0)
            {
                if (this[LastIndex] == ' ' && IsLastSameAsOriginal)
                {
                    for (int i = LastIndex; i >= 0 && this[i] == ' ' && Original[i] == ' '; i--)
                        Length--;
                }
                else
                {
                    if (!IsLastSameAsOriginal)
                        ErrorsUncorrected.RemoveAt(ErrorsUncorrected.Count - 1);

                    Length--;
                }
            }

            return this;
        }

        public ReadWriteTypingBuffer Append(char ch)
        {
            Buffer[Length++] = ch;

            if (!IsLastSameAsOriginal)
            {
                ErrorsCommitted++;
                ErrorsUncorrected.Add(LastIndex);

                OnError(EventArgs.Empty);
            }

            return this;
        }

        public bool IsLastSameAsOriginal
        {
            get { return Length > 0 && IsSameAsOriginal(LastIndex); }
        }

        public bool IsSameAsOriginal(int index)
        {
            return index <= Original.LastIndex && this[index] == Original[index];
        }

        public ReadWriteTypingBuffer ProcessKey(char ch)
        {
            if (ch == '\b')
            {
                BackspaceKeys++;
                return RemoveLast();
            }

            if (Length >= Original.Length)
                return this;

            ForwardKeys++;

            if (ch == '\r')
                return Append('\n');
            else if (ch == '\t')
                return ExpandTab();
            else
                return Append(ch);
        }

        public ReadWriteTypingBuffer ExpandTab()
        {
            Append(' ');

            if (IsLastSameAsOriginal)
                for (int i = LastIndex + 1; i < Original.Length && Original[i] == ' '; i++)
                    Append(' ');

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
            get { return BackspaceKeys + ErrorsUncorrected.Count; }
        }

        public decimal Accuracy
        {
            get { return ForwardKeys > 0 ? 1m - (decimal)(BackspaceKeys + ErrorsUncorrected.Count) / (decimal)ForwardKeys : 1m; }
        }

        protected override bool IsWordChar(int index, char c)
        {
            return base.IsWordChar(index, c) &&
                   (CountErrorsAsWordChars || IsSameAsOriginal(index));
        }

        public event EventHandler Error;

        protected virtual void OnError(EventArgs e)
        {
            if (Error != null)
                Error(this, e);
        }

        public int ExpandedLastIndex
        {
            get { return Original.ExpandedIndex(this.LastIndex); }
        }

        public int ExpandedLength
        {
            get { return Original.ExpandedIndex(this.Length); }
        }

        public int[] ExpandedErrorsUncorrected
        {
            get { return ErrorsUncorrected.Select(i => Original.ExpandedIndex(i)).ToArray(); }
        }
    }
}
