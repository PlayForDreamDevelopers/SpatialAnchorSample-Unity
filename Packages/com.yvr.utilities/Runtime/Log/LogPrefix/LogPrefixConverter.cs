namespace YVR.Utilities
{
    public static class LogPrefixConverter
    {
        public static LogPrefixBase ToLogPrefix(this PrefixType prefixType)
        {
            LogPrefixBase prefix = null;
            if (prefixType.HasFlag(PrefixType.Priority)) prefix = new PriorityLogPrefix();
            if (prefixType.HasFlag(PrefixType.Context)) prefix = new ContextLogPrefix(prefix);
            return prefix;
        }
    }
}