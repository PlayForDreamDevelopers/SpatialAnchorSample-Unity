using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.TestTools;

namespace YVR.Utilities
{
    [ExcludeFromDocs, ExcludeFromCoverage]
    public class YLogAndroidAdapter : IYLogAdapter
    {
        #region DllImport

        [DllImport("yvrutilities")]
        private static extern void initializeRamLog(string tag, int ramLogSize);

        [DllImport("yvrutilities")]
        private static extern void saveLog();

        [DllImport("yvrutilities")]
        private static extern void debug(string logMsg);

        [DllImport("yvrutilities")]
        private static extern void ramDebug(string logMsg);

        [DllImport("yvrutilities")]
        private static extern void info(string logMsg);

        [DllImport("yvrutilities")]
        private static extern void ramInfo(string logMsg);

        [DllImport("yvrutilities")]
        private static extern void warn(string logMsg);

        [DllImport("yvrutilities")]
        private static extern void ramWarn(string logMsg);

        [DllImport("yvrutilities")]
        private static extern void error(string logMsg);

        [DllImport("yvrutilities")]
        private static extern void ramError(string logMsg);

        [DllImport("yvrutilities")]
        private static extern void setDebugOutputHandler(Action<string> outputViaUnityHandler);

        [DllImport("yvrutilities")]
        private static extern void setInfoOutputHandler(Action<string> outputViaUnityHandler);

        [DllImport("yvrutilities")]
        private static extern void setWarnOutputHandler(Action<string> outputViaUnityHandler);

        [DllImport("yvrutilities")]
        private static extern void setErrorOutputHandler(Action<string> outputViaUnityHandler);

        #endregion

        public void ConfigureYLog(string tag, int ramLogSize = 3) { initializeRamLog(tag, ramLogSize); }

        public void SaveLog() { saveLog(); }

        public void DebugHandle(string msg) { debug(msg); }

        public void RamDebugHandle(string msg) { ramDebug(msg); }

        public void InfoHandle(string msg) { info(msg); }

        public void RamInfoHandle(string msg) { ramInfo(msg); }

        public void WarnHandle(string msg) { warn(msg); }

        public void RamWarnHandle(string msg) { ramWarn(msg); }

        public void ErrorHandle(string msg) { error(msg); }

        public void RamErrorHandle(string msg) { ramError(msg); }

        /// <summary>
        /// Basically used for debugging native log
        /// </summary>
        /// <param name="viaUnity"> if true, native log will be output via Unity Debug, otherwise directly via android logcat </param>
        public void SetLogcatOutputHandler(bool viaUnity)
        {
            setDebugOutputHandler(viaUnity ? (Action<string>) DebugViaUnity : null);
            setInfoOutputHandler(viaUnity ? (Action<string>) InfoViaUnity : null);
            setWarnOutputHandler(viaUnity ? (Action<string>) WarnViaUnity : null);
            setErrorOutputHandler(viaUnity ? (Action<string>) ErrorViaUnity : null);
        }


        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void DebugViaUnity(string msg) { Debug.Log(msg); }

        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void InfoViaUnity(string msg) { Debug.Log(msg); }

        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void WarnViaUnity(string msg) { Debug.LogWarning(msg); }

        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void ErrorViaUnity(string msg) { Debug.LogError(msg); }
    }
}