using UnityEngine;

namespace YVR.Utilities
{
    /// <summary>
    /// Logger which encapsulate native YLog
    /// </summary>
    public class YLogLogger : LoggerBase
    {
        private static IYLogAdapter s_YLogAdapter = null;

        /// <summary>
        /// Priority threshold for RamLog
        /// </summary>
        public static LogPriority ramLogPriorityTHold = LogPriority.Lowest;

        /// <summary>
        /// Constructor for YLogLogger
        /// </summary>
        /// <param name="wrappedLogger"> Wrapped other wrapped logger </param>
        /// <param name="prefix">Wrapped log prefix </param>
        /// <returns></returns>
        public YLogLogger(LoggerBase wrappedLogger = null, LogPrefixBase prefix = null) : base(wrappedLogger, prefix)
        {
            s_YLogAdapter ??= Application.isEditor ? (IYLogAdapter) new YLogEditorAdapter() : new YLogAndroidAdapter();
        }

        /// <summary>
        /// Configure YLog
        /// </summary>
        /// <param name="tag"> Tag used by RamLog which will decide the folder ram log saved</param>
        /// <param name="ramLogSize">The maximum memory size(in mb) used by RamLog </param>
        public static void ConfigureYLog(string tag, int ramLogSize = 3)
        {
            s_YLogAdapter.ConfigureYLog(tag, ramLogSize);
        }

        /// <summary>
        /// Set the logcat output handler of YLogAdapter.
        /// </summary>
        /// <param name="viaUnity">If set to true, the output will use Unity.</param>
        /// <remarks> This function should only be used for testing</remarks>
        public static void OutputViaUnity(bool viaUnity) { s_YLogAdapter.SetLogcatOutputHandler(viaUnity); }

        /// <summary>
        /// Save the log in memory to local IO
        /// </summary>
        public static void SaveLog() { s_YLogAdapter.SaveLog(); }

        /// <inheritdoc />

#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        protected override void DebugHandle(string msg) { s_YLogAdapter.DebugHandle(msg); }

        /// <inheritdoc />
#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        protected override void InfoHandle(string msg) { s_YLogAdapter.InfoHandle(msg); }

        /// <inheritdoc />
#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        protected override void WarnHandle(string msg) { s_YLogAdapter.WarnHandle(msg); }

        /// <inheritdoc />
#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        protected override void ErrorHandle(string msg) { s_YLogAdapter.ErrorHandle(msg); }

        protected override void OnInvalidLogImpl(object context, string msg, LogPriority priority, string prefix)
        {
            base.OnInvalidLogImpl(context, msg, priority, prefix);

            // If ramLogPriorityTHold is lower than priority, then output log to ram
            if (ramLogPriorityTHold > priority) return;

            if (priority == LogPriority.Debug)
                s_YLogAdapter.RamDebugHandle(MsgWithPrefix(msg, prefix));
            else if (priority == LogPriority.Info)
                s_YLogAdapter.RamInfoHandle(MsgWithPrefix(msg, prefix));
            else if (priority == LogPriority.Warn)
                s_YLogAdapter.RamWarnHandle(MsgWithPrefix(msg, prefix));
            else if (priority == LogPriority.Error)
                s_YLogAdapter.RamErrorHandle(MsgWithPrefix(msg, prefix));
        }
    }
}