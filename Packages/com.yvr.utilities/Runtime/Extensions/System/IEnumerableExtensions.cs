using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace YVR.Utilities
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> selfArray, Action<T> action)
        {
            if (action == null) throw new ArgumentException();
            foreach (T item in selfArray)
            {
                action(item);
            }

            return selfArray;
        }
    }
}