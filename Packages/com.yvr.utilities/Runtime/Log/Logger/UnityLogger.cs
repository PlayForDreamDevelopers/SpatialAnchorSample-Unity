namespace YVR.Utilities
{
    public class UnityLogger : LoggerBase
    {
        public UnityLogger(LoggerBase wrappedLogger = null, LogPrefixBase logPrefix = null,
                           LoggerControllerBase controller = null) : base(wrappedLogger, logPrefix, controller) { }

        /// <inheritdoc />
        protected override void DebugHandle(string msg) { UnityEngine.Debug.Log(msg); }

        /// <inheritdoc />
        protected override void InfoHandle(string msg) { UnityEngine.Debug.Log(msg); }

        /// <inheritdoc />
        protected override void WarnHandle(string msg) { UnityEngine.Debug.LogWarning(msg); }

        /// <inheritdoc />
        protected override void ErrorHandle(string msg) { UnityEngine.Debug.LogError(msg); }
    }
}