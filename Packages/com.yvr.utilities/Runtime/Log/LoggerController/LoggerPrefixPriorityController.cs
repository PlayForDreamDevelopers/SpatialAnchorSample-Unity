using System.Collections.Generic;
using System.Linq;

namespace YVR.Utilities
{
    public class LoggerPrefixPriorityController : LoggerPriorityController
    {
        private Dictionary<string, LogPriority> m_Prefix2PriorityDic = null;

        public LoggerPrefixPriorityController(LoggerControllerBase wrappedController = null) : base(wrappedController)
        {
            m_Prefix2PriorityDic = new Dictionary<string, LogPriority>();
        }

        protected override bool IsLogValidImpl(object context, string log, LogPriority priority, string prefix)
        {
            string prefixKey = m_Prefix2PriorityDic.Keys.FirstOrDefault(prefix.Contains);
            if (prefixKey == null) return base.IsLogValidImpl(context, log, priority, prefix);

            return m_Prefix2PriorityDic[prefixKey] <= priority;
        }

        public void AddPrefix2PriorityMap(string prefix, LogPriority priority)
        {
            m_Prefix2PriorityDic.SafeAdd(prefix, priority, true);
        }

        public void RemovePrefixPriorityMap(string prefix) { m_Prefix2PriorityDic.Remove(prefix); }

        public void ClearPrefixPriority() { m_Prefix2PriorityDic.Clear(); }
    }
}