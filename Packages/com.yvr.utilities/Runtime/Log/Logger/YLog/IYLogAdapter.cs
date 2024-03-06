namespace YVR.Utilities
{
    /// <summary>
    /// Interface that intend to encapsulate YLog operations on different platform
    /// </summary>
    public interface IYLogAdapter
    {
        /// <summary>
        /// Configure native YLog module
        /// </summary>
        /// <param name="tag"> Tag used by RamLog </param>
        /// <param name="ramLogSize">The maximum memory size(in mb) used by RamLog </param>
        void ConfigureYLog(string tag, int ramLogSize = 5);

        /// <summary>
        /// Save the log in memory to local IO
        /// </summary>
        void SaveLog();

        /// <summary>
        /// Basically used for debugging native log
        /// </summary>
        /// <param name="viaUnity"> if true, native log will be output via Unity Debug, otherwise directly via android logcat </param>
        void SetLogcatOutputHandler(bool viaUnity);

        /// <summary>
        /// Handle to output log using logcat in Debug priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        void DebugHandle(string msg);

        /// <summary>
        /// Handle to output log using ram in Debug priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        void RamDebugHandle(string msg);

        /// <summary>
        /// Handle to output log using logcat in Info priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        void InfoHandle(string msg);

        /// <summary>
        /// Handle to output log using ram in Info priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        void RamInfoHandle(string msg);

        /// <summary>
        /// Handle to output log using logcat in Warn priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        void WarnHandle(string msg);

        /// <summary>
        /// Handle to output log using ram in Warn priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        void RamWarnHandle(string msg);


        /// <summary>
        /// Handle to output log using logcat in Error priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        void ErrorHandle(string msg);

        /// <summary>
        /// Handle to output log using ram in Error priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        void RamErrorHandle(string msg);
    }
}