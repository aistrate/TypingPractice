using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typist
{
    public static class Util
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int subsequenceLength)
        {
            for (IEnumerable<T> subseq = source.Take(subsequenceLength), rest = source.Skip(subsequenceLength);
                 subseq.Count() > 0;
                 subseq = rest.Take(subsequenceLength), rest = rest.Skip(subsequenceLength))
                yield return subseq;
        }

        public static bool IsOneOf<T>(this T elem, params T[] list)
        {
            return list.Contains(elem);
        }

        public static string AsString(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }
    }
}
