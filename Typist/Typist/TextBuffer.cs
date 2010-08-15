using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Typist
{
    public class TextBuffer
    {
        public TextBuffer(string text, bool countWhitespaceAsWordChars, bool removeMultipleWhitespace)
            : this(countWhitespaceAsWordChars)
        {
            string allowedWhitespace = " \n";

            text = text ?? string.Empty;

            text = text.Replace("\r\n", "\n")
                       .Replace("\t", "    ")
                       .Where(c => !char.IsWhiteSpace(c) || allowedWhitespace.IndexOf(c) >= 0)
                       .AsString()
                       .TrimEnd(allowedWhitespace.ToCharArray());

            if (removeMultipleWhitespace)
            {
                text = Regex.Replace(text, @" +", " ");
                text = Regex.Replace(text, @" +\n", "\n");
                text = Regex.Replace(text, @"\n{3,}", "\n\n");
            }

            buffer = text.ToCharArray();
            Length = text.Length;

            CountErrorsAsWordChars = true;
        }

        public TextBuffer(TextBuffer original, bool countWhitespaceAsWordChars, bool countErrorsAsWordChars)
            : this(countWhitespaceAsWordChars)
        {
            if (original == null)
                throw new ArgumentNullException("original.", "Original TextBuffer cannot be null.");

            Original = original;

            buffer = new char[Math.Max(2000, 2 * original.Length)];
            Length = 0;

            CountErrorsAsWordChars = countErrorsAsWordChars;
        }

        private TextBuffer(bool countWhitespaceAsWordChars)
        {
            CountWhitespaceAsWordChars = countWhitespaceAsWordChars;

            ErrorsUncorrected = new List<int>();
        }

        private readonly char[] buffer;

        public TextBuffer Original { get; private set; }

        public int Length { get; private set; }

        public int TotalKeys { get; private set; }

        public int ErrorsCommitted { get; private set; }

        public List<int> ErrorsUncorrected { get; private set; }

        public bool CountWhitespaceAsWordChars { get; set; }
        public bool CountErrorsAsWordChars { get; set; }


        public char this[int index] { get { return buffer[index]; } }

        public int LastIndex { get { return Length - 1; } }


        public string Substring(int startIndex)
        {
            return new string(buffer, startIndex, Length - startIndex);
        }

        public string Substring(int startIndex, int length)
        {
            return new string(buffer, startIndex, Math.Min(length, Length - startIndex));
        }

        public override string ToString()
        {
            return new string(buffer, 0, Length);
        }


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
                Func<int, char, bool> isWordChar = (i, c) =>
                    (CountWhitespaceAsWordChars || !char.IsWhiteSpace(c)) &&
                    (CountErrorsAsWordChars || IsSameAsOriginal(i));

                return Count(isWordChar) / 5;
            }
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

        public event EventHandler Error;

        protected virtual void OnError(EventArgs e)
        {
            if (Error != null)
                Error(this, e);
        }
    }
}
