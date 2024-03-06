using System;
using UnityEngine;

namespace YVR.Utilities
{
    /// <summary>
    /// Base class for all loggers
    /// </summary>
    public abstract class LoggerBase
    {
        private LoggerBase m_WrappedLogger = null;
        private LogPrefixBase m_LogPrefix = null;
        private LoggerControllerBase m_LoggerController = null;

        /// <summary>
        /// Constructor for YLogLogger
        /// </summary>
        /// <param name="wrappedLogger"> Wrapped other wrapped logger</param>
        /// <param name="logPrefix">Wrapped log prefix</param>
        /// <param name="controller">Controller for logger</param>
        protected LoggerBase(LoggerBase wrappedLogger = null, LogPrefixBase logPrefix = null,
                             LoggerControllerBase controller = null)
        {
            m_WrappedLogger = wrappedLogger;
            m_LogPrefix = logPrefix;

            m_LoggerController = controller;
        }

        /// <summary>
        /// Sets the log controller for the current logger
        /// </summary>
        /// <param name="controller">The <see cref="LoggerControllerBase"/> instance to use for control logger</param>
        public void SetController(LoggerControllerBase controller) { m_LoggerController = controller; }

        /// <summary>
        /// Sets the log prefix for the current logger.
        /// </summary>
        /// <param name="prefix">The <see cref="LogPrefixBase"/> instance to use for the log prefix.</param>
        public void SetPrefix(LogPrefixBase prefix) { m_LogPrefix = prefix; }

        /// <summary>
        /// Handle for output log in Debug priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        protected abstract void DebugHandle(string msg);

        /// <summary>
        /// Handle for output log in Info priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        protected abstract void InfoHandle(string msg);

        /// <summary>
        /// Handle for output log in Warn priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        protected abstract void WarnHandle(string msg);

        /// <summary>
        /// Handle for output log in Error priority
        /// </summary>
        /// <param name="msg"> The log message </param>
        protected abstract void ErrorHandle(string msg);

        /// <summary>
        /// Output log in Debug priority
        /// </summary>
        /// <param name="context"> The context where output the log </param>
        /// <param name="message"> The log message </param>
#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        public virtual void Debug(object context, string message)
        {
            Output(context, message, LogPriority.Debug, DebugFormattedMsg);

    #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
            void DebugFormattedMsg(string formattedMsg)
            {
                m_WrappedLogger?.DebugHandle(formattedMsg);
                DebugHandle(formattedMsg);
            }
        }

        /// <summary>
        /// Output log in Info priority
        /// </summary>
        /// <param name="context"> The context where output the log </param>
        /// <param name="message"> The log message </param>
#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        public virtual void Info(object context, string message)
        {
            Output(context, message, LogPriority.Info, InfoFormattedMsg);

    #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
            void InfoFormattedMsg(string formattedMsg)
            {
                m_WrappedLogger?.InfoHandle(formattedMsg);
                InfoHandle(formattedMsg);
            }
        }

        /// <summary>
        /// Output log in Warn priority
        /// </summary>
        /// <param name="context"> The context where output the log </param>
        /// <param name="message"> The log message </param>
#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        public virtual void Warn(object context, string message)
        {
            Output(context, message, LogPriority.Warn, WarnFormattedMsg);

    #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
            void WarnFormattedMsg(string formattedMsg)
            {
                m_WrappedLogger?.WarnHandle(formattedMsg);
                WarnHandle(formattedMsg);
            }
        }

        /// <summary>
        /// Output log in Error priority
        /// </summary>
        /// <param name="context"> The context where output the log </param>
        /// <param name="message"> The log message </param>
#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        public virtual void Error(object context, string message)
        {
            Output(context, message, LogPriority.Error, ErrorFormattedMsg);

    #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
            void ErrorFormattedMsg(string formattedMsg)
            {
                m_WrappedLogger?.ErrorHandle(formattedMsg);
                ErrorHandle(formattedMsg);
            }
        }

#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        protected virtual void Output(object context, string msg, LogPriority priority, Action<string> onFormattedMsg)
        {
            string prefix = m_LogPrefix == null ? "" : m_LogPrefix.GetCombinedPrefix(context, msg, priority);

            if (m_LoggerController != null && !m_LoggerController.IsLogValid(context, msg, priority, prefix))
            {
                OnInvalidLog(context, msg, priority, prefix);
                return;
            }

            string formattedMsg = MsgWithPrefix(msg, prefix);
            onFormattedMsg?.Invoke(formattedMsg);
        }

        protected virtual void OnInvalidLogImpl(object context, string msg, LogPriority priority, string prefix) { }

        protected static string MsgWithPrefix(string msg, string prefix)
        {
            return string.IsNullOrEmpty(prefix) ? msg : $"[{prefix}] {msg}";
        }

        private void OnInvalidLog(object context, string log, LogPriority priority, string prefix)
        {
            m_WrappedLogger?.OnInvalidLog(context, log, priority, prefix);
            OnInvalidLogImpl(context, log, priority, prefix);
        }
    }
}