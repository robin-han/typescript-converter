using System;
using System.Collections.Generic;
using System.Text;

namespace java.lang.common.extensions
{
    static class ListExtension
    {
        public static bool nonEmpty<T>(this IList<T> list)
        {
            return list.Count > 0;
        }

        public static bool isEmpty<T>(this IList<T> list)
        {
            return list.Count == 0;
        }

        public static T last<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        public static int size<T>(this IList<T> list)
        {
            return list.Count;
        }
    }
}
