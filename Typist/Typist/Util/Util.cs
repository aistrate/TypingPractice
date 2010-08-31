using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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
            where T : struct
        {
            return list.Contains(elem);
        }

        public static PropertyInfo[] AllStaticProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
        }

        public static T[] GetValues<T>(this PropertyInfo[] propertyInfos)
        {
            return propertyInfos.Where(p => p.PropertyType == typeof(T) || p.PropertyType.IsSubclassOf(typeof(T)))
                                .Select(p => p.GetValue(null, null))
                                .OfType<T>()
                                .ToArray();
        }

        public static A RaiseIfEventArgsChanged<A>(Action<A> raiseEvent, A newEventArgs, A oldEventArgs)
            where A : EventArgs
        {
            if (!newEventArgs.Equals(oldEventArgs))
                raiseEvent(newEventArgs);

            return newEventArgs;
        }
    }
}
