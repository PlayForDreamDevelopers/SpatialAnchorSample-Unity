using System;
using System.Collections.Generic;

namespace YVR.Utilities
{
    [Serializable]
    public struct LoggerConfig
    {
        public LoggerType loggerType;
        public PrefixType prefixType;
        public LogPriority priority;
        public List<Prefix2PriorityNode> prefix2PriorityMap;
    }
}