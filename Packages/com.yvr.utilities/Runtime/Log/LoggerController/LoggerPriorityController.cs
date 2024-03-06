namespace YVR.Utilities
{
    public class LoggerPriorityController : LoggerControllerBase
    {
        public LogPriority priority { get; set; } = LogPriority.Lowest;

        public LoggerPriorityController(LoggerControllerBase wrappedController = null) : base(wrappedController) { }

        protected override bool IsLogValidImpl(object context, string log, LogPriority priority, string prefix)
        {
            return priority >= this.priority;
        }
    }
}