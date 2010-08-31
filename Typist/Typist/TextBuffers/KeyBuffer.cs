using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typist.TextBuffers
{
    public class KeyBuffer
    {
        public KeyBuffer(int capacity)
        {
            buffer = new RecordedKey[capacity];
            Length = 0;
        }

        private RecordedKey[] buffer;

        public int Length { get; private set; }

        public RecordedKey this[int index] { get { return buffer[index]; } }

        public void Add(char key, int position, bool isError)
        {
            buffer[Length++] = new RecordedKey(key, position, isError);
        }

        public void RemoveFromPosition(int position)
        {
            for (int i = Length - 1; i >= 0 && this[i].Position >= position; i--)
                Length--;
        }

        public class RecordedKey
        {
            private RecordedKey() { }

            public RecordedKey(char key, int position, bool isError)
            {
                Key = key;
                Position = position;
                IsError = isError;
            }

            public readonly char Key;
            public readonly int Position;
            public readonly bool IsError;

            public bool IsWhitespace { get { return char.IsWhiteSpace(Key); } }

            public override string ToString()
            {
                return string.Format("{{{0}, {1}, {2}}}", Key.Show(), Position, IsError);
            }
        }

        public char[] Keys
        {
            get { return Values.Select(v => v.Key).ToArray(); }
        }

        public RecordedKey[] Values
        {
            get
            {
                RecordedKey[] keys = new RecordedKey[Length];
                Array.Copy(buffer, keys, Length);
                return keys;
            }
        }

        public int Count(Func<RecordedKey, bool> predicate)
        {
            int count = 0;

            for (int i = 0; i < Length; i++)
                if (predicate(this[i]))
                    count++;

            return count;
        }

        public int Count(bool countWhitespace, bool countErrors)
        {
            return Count(k => (countWhitespace || !k.IsWhitespace) &&
                              (countErrors || !k.IsError));
        }
    }
}
