using System;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// The extensions class of platform sdk
    /// </summary>
    public static class PlatformExtension
    {
        /// <summary>
        /// Turns string array to android java object
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static AndroidJavaObject JavaArrayFromCs(this string[] values)
        {
            AndroidJavaClass arrayClass = new AndroidJavaClass("java.lang.reflect.Array");

            AndroidJavaObject arrayObject
                = arrayClass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass("java.lang.String"),
                                                           values.Length);

            for (int i = 0; i < values.Length; ++i)
            {
                arrayClass.CallStatic("set", arrayObject, i, new AndroidJavaObject("java.lang.String", values[i]));
            }

            return arrayObject;
        }

        public static long ToUnixTimeMillisecond(this DateTime dateTime)
        {
            TimeSpan span = DateTime.UtcNow - DateTime.Now;
            long timeStamp = (long) ((dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds +
                                     span.TotalMilliseconds);
            Debug.Log(timeStamp);
            return timeStamp;
        }
    }
}