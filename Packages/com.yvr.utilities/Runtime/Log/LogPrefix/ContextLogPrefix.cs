using UnityEngine;

namespace YVR.Utilities
{
    /// <summary>
    /// Used for adding context information as log prefix
    /// </summary>
    public class ContextLogPrefix : LogPrefixBase
    {
        /// <summary>
        /// Constructor for ContextLogPrefix
        /// </summary>
        /// <param name="prefix"> wrapped log prefix </param>
        /// <returns></returns>
        public ContextLogPrefix(LogPrefixBase prefix = null) : base(prefix) { }

        /// <summary>
        /// Get context info prefix for log
        /// </summary>
        /// <param name="context"> The context where output the log </param>
        /// <param name="log"> The log message </param>
        /// <param name="priority"> Log Priority </param>
        /// <returns> The context info prefix in format 'Context: {contextInfo}' </returns>
        protected override string GetPrefix(object context, string log, LogPriority priority)
        {
            if (context == null) return "NoneContext";

            string contextInfo;

            switch (context)
            {
                case GameObject go:
                    contextInfo = $"{go.name}";
                    break;
                case MonoBehaviour mono:
                    contextInfo = $"{mono.gameObject.name}-{mono.GetType().FullName}";
                    break;
                case string s:
                    contextInfo = s;
                    break;
                default:
                    contextInfo = context.GetType().FullName;
                    break;
            }

            return $"Context: {contextInfo}";
        }
    }
}