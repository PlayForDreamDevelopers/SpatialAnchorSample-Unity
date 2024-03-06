using System;

namespace YVR.Utilities
{
    [Serializable]
    public struct Prefix2PriorityNode
    {
        public string prefix;
        public LogPriority priority;

        public Prefix2PriorityNode(string prefix, LogPriority priority)
        {
            this.prefix = prefix;
            this.priority = priority;
        }
    }
}