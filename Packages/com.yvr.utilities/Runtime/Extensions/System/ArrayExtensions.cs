using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YVR.Utilities
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Converts the elements of the specified array to a string, separated by commas.
        /// </summary>
        /// <param name="source">The array whose elements to concatenate into a string.</param>
        /// <returns>A string that contains the elements of the array, separated by commas.</returns>
        /// <remarks>
        /// This extension method is intended for use with arrays of value types or reference types that have a meaningful string representation.
        /// </remarks>
        public static string ToElementsString(this Array source)
        {
            string s = "";
            source.IndexForEach(index => s += $"{source.GetValue(index)}, ");
            return string.IsNullOrEmpty(s) ? "" : s.Substring(0, s.Length - 2);
        }

        /// <summary>
        /// Casts the elements of an Array to the specified type.
        /// </summary>
        /// <typeparam name="T">Target Type</typeparam>
        /// <param name="source"> The type to cast the element of <paramref name="source"/> to. </param>
        /// <returns>An IEnumerable that contains each element of the source sequence cast to target type. </returns>
        [Obsolete("Use Enumerable.Cast<T>(source), An official Linq implementation")]
        public static IEnumerable<T> Cast<T>(this Array source) => Enumerable.Cast<T>(source);

        /// <summary>
        /// Casts the elements of an Array to the specified type.
        /// </summary>
        /// <param name="source"> The Array whose elements to filter. </param>
        /// <param name="targetType"> The type to cast the element of <paramref name="source"/> to. </param>
        /// <returns>An IEnumerable that contains each element of the source sequence cast to target type. </returns>
        public static IEnumerable Cast(this Array source, Type targetType)
        {
            MethodInfo castMethod = typeof(Enumerable).GetMethod("Cast");
            MethodInfo castGenericMethod = castMethod.MakeGenericMethod(targetType);

            return (IEnumerable) castGenericMethod.Invoke(source, new object[] {source});
        }

        /// <summary>
        /// Performs the specified action on each element of the Array.
        /// </summary>
        /// <typeparam name="T"> Type of the element</typeparam>
        /// <param name="source"> Tye Array whose element will be handled by <paramref name="action"/> </param>
        /// <param name="action"> The specified action on each element of <paramref name="source"/> </param>
        public static void ForEach<T>(this T[] source, Action<T> action)
        {
            if (source == null || source.Length == 0) return;
            foreach (T item in Enumerable.Cast<T>(source))
            {
                action?.Invoke(item);
            }
        }
        
        public static void ForEach<T>(this Array source, Action<T> action)
        {
            (source as T[]).ForEach(action);
        }

        /// <summary>
        /// Perform the specified action on each element of the Array
        /// </summary>
        /// <param name="source"> Tye Array whose element will be handled by <paramref name="action"/> </param>
        /// <param name="action"> The specified action on each element of <paramref name="source"/>, with element's indices as the parameters </param>
        public static void IndexForEach(this Array source, Action<int[]> action)
        {
            if (source.Length == 0) return;

            int dimensions = source.Rank;
            int[] eachDimensionSize
                = Enumerable.Range(0, dimensions).Select((_, index) => source.GetLength(index)).ToArray();
            int[] indices = new int[dimensions];

            int ptrIndex = dimensions - 1;

            action(indices);

            while (ptrIndex >= 0)
            {
                if (indices[ptrIndex] + 1 < eachDimensionSize[ptrIndex])
                {
                    indices[ptrIndex]++;
                    action(indices);
                    ptrIndex = dimensions - 1;
                }
                else
                {
                    ptrIndex--;
                    for (int i = dimensions - 1; i > ptrIndex; i--)
                        indices[i] = 0;
                }
            }
        }
    }
}