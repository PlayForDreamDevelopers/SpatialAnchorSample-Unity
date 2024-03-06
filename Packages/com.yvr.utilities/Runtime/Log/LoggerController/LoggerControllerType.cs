using System;

namespace YVR.Utilities
{
    [Serializable]
    public enum LoggerControllerType
    {
        None = 0,
        Priority = 1 << 0,
        PrefixPriority = 1 << 1,
    }
}