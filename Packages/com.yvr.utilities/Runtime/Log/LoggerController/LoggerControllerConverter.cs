namespace YVR.Utilities
{
    public static class LoggerControllerConverter
    {
        public static LoggerControllerBase ToLoggerController(this LoggerConfig cfg)
        {
            LoggerControllerBase controller = null;

            var type = LoggerControllerType.None;
            if (cfg.priority != LogPriority.Lowest)
                type = LoggerControllerType.Priority;
            if (cfg.prefix2PriorityMap != null && cfg.prefix2PriorityMap.Count != 0)
                type = LoggerControllerType.PrefixPriority;

            if (type == LoggerControllerType.Priority)
            {
                controller = new LoggerPriorityController();
                ((LoggerPriorityController) controller).priority = cfg.priority;
            }
            else if (type == LoggerControllerType.PrefixPriority)
            {
                controller = new LoggerPrefixPriorityController();
                var prefixPriorityController = (LoggerPrefixPriorityController) controller;
                prefixPriorityController.priority = cfg.priority;
                cfg.prefix2PriorityMap.ForEach(node =>
                {
                    prefixPriorityController.AddPrefix2PriorityMap(node.prefix, node.priority);
                });
            }

            return controller;
        }
    }
}