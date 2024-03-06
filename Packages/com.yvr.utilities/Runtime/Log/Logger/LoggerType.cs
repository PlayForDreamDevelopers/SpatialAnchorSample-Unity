using System;
using System.Diagnostics.CodeAnalysis;

namespace YVR.Utilities
{
    [Serializable]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum LoggerType
    {
        None = 0,
        YLog = 1 << 0,
        Unity = 1 << 1
    }
}