namespace Typist
{
    public abstract class TypingBuffer : TextBuffer
    {
        public virtual bool ExpandNewlines { get; set; }

        public virtual bool CountWhitespaceAsWordChars { get; set; }

        protected virtual bool IsWordChar(int index, char c)
        {
            return CountWhitespaceAsWordChars || !char.IsWhiteSpace(c);
        }

        public int WordCount
        {
            get { return Count(IsWordChar) / 5; }
        }
    }
}
