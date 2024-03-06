namespace YVR.Utilities
{
    public static class SystemObjectExtensions
    {
        public static T As<T>(this object selfObj) where T : class { return selfObj as T; }
    }
}