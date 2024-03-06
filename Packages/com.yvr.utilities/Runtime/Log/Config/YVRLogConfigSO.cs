using System.Collections.Generic;
using UnityEngine;

namespace YVR.Utilities
{
    [CreateAssetMenu(fileName = "YVRLogConfig", menuName = "YVR/YVRLogConfig")]
    public class YVRLogConfigSO :ScriptableObject
    {
        public bool enable = true;
        public string tag = "Unity";
        public List<LoggerConfig> loggerConfigs;
        
        public void Apply()
        {
            YVRLog.enable = enable;
            YVRLog.ClearLoggers();
            YLogLogger.ConfigureYLog(tag);
            
            loggerConfigs?.ForEach(cfg =>
            {
                LoggerBase logger = cfg.loggerType.ToLogger();
                logger.SetPrefix(cfg.prefixType.ToLogPrefix());
                logger.SetController(cfg.ToLoggerController());
                YVRLog.RegisterLogger(logger);
            });
         }
    }
}