namespace YVR.Utilities
{
    /// <summary>
    /// Base class for all logger controllers
    /// </summary>
    public abstract class LoggerControllerBase
    {
        private LoggerControllerBase m_WrappedController = null;

        /// <summary>
        /// Constructor for LoggerController
        /// </summary>
        /// <param name="wrappedController"> Wrapped logger controller </param>
        protected LoggerControllerBase(LoggerControllerBase wrappedController = null)
        {
            m_WrappedController = wrappedController;
        }

        /// <summary>
        /// To determinate whether the log is valid to be output
        /// </summary>
        /// <param name="context">The context where output the log</param>
        /// <param name="log">The log message</param>
        /// <param name="priority">Log Priority</param>
        /// <param name="prefix">The Log prefix</param>
        /// <returns>True if this log message should be output</returns>
        public bool IsLogValid(object context, string log, LogPriority priority, string prefix)
        {
            return (m_WrappedController == null || m_WrappedController.IsLogValid(context, log, priority, prefix))
                   && IsLogValidImpl(context, log, priority, prefix);
        }

        protected abstract bool IsLogValidImpl(object context, string log, LogPriority priority, string prefix);
    }
}