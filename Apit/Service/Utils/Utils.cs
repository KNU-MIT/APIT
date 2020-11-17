using System;
using System.Collections.Generic;
using System.Linq;

namespace Apit.Utils
{
    public static class Utils
    {
        public static bool PairAny<T1, T2>(this T1[] collection1, T2[] collection2, Func<T1, T2, bool> predicate)
        {
            bool doIt = true;
            for (int i = 0; i < collection1.Length; i++)
                if (predicate(collection1[i], collection2[i]))
                    doIt = false;
            return doIt;
        }

        public static IEnumerable<IEnumerable<T>> SplitIntoSubArrays<T>(IEnumerable<T> array, uint subLength)
        {
            uint counter = subLength;
            return array.GroupBy(_ => counter++ / subLength);
        }
    }
}