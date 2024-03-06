namespace YVR.Utilities
{
    public static class LoggerConverter
    {
        public static LoggerBase ToLogger(this LoggerType type)
        {
            LoggerBase logger = null;
            if (type == LoggerType.YLog) logger = new YLogLogger();
            if (type == LoggerType.Unity) logger = new UnityLogger();

            return logger ?? new YLogLogger();
        }
    }
}