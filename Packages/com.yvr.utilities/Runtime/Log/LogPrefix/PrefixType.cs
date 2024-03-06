using System;
using System.Diagnostics.CodeAnalysis;

namespace YVR.Utilities
{
    [Flags, Serializable]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum PrefixType
    {
        None = 0,
        Priority = 1 << 0,
        Context = 1 << 1,
    }
}