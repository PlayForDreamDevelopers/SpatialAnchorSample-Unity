namespace YVR.Utilities
{
    public class CustomLogPrefix : LogPrefixBase
    {
        public string prefix { get; set; } = null;

        public CustomLogPrefix(string prefix) { this.prefix = prefix; }

        protected override string GetPrefix(object context, string log, LogPriority priority) => prefix;
    }
}