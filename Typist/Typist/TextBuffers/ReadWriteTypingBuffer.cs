using System;
using System.Collections.Generic;
using System.Linq;

namespace Typist.TextBuffers
{
    public class ReadWriteTypingBuffer : TypingBuffer
    {
        public ReadWriteTypingBuffer(ReadOnlyTypingBuffer original)
        {
            if (original == null)
                throw new ArgumentNullException("original.", "Original TextBuffer cannot be null.");

            Original = original;

            Buffer = new char[original.Length + 1];
            Length = 0;

            RecordedKeys = new KeyBuffer(Buffer.Length);

            ErrorsUncorrected = new List<int>();
        }

        public ReadOnlyTypingBuffer Original { get; private set; }

        public KeyBuffer RecordedKeys { get; private set; }

        public int TotalForwardKeys { get; private set; }

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

                RecordedKeys.RemoveFromPosition(Length);
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

        public ReadWriteTypingBuffer ProcessKey(char keyChar)
        {
            if (keyChar == '\b')
            {
                BackspaceKeys++;
                RemoveLast();
            }
            else if (Length < Original.Length)
                addKey(keyChar);

            return this;
        }

        private void addKey(char keyChar)
        {
            char ch = keyChar;

            if (keyChar == '\r')
                ch = '\n';
            else if (keyChar == '\t')
                ch = ' ';

            Append(ch);

            TotalForwardKeys++;

            RecordedKeys.Add(keyChar, LastIndex, !IsLastSameAsOriginal);

            if (keyChar == '\t' && IsLastSameAsOriginal)
                for (int i = LastIndex + 1; i < Original.Length && Original[i] == ' '; i++)
                    Buffer[Length++] = ' ';
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
            get { return TotalForwardKeys > 0 ? 1m - (decimal)(BackspaceKeys + ErrorsUncorrected.Count) / (decimal)TotalForwardKeys : 1m; }
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
