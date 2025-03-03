namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate functions of achievement request
    /// </summary>
    public static class Achievement
    {
        /// <summary>
        /// Add achievement count progress
        /// </summary>
        /// <param name="achievementName">Achievement name</param>
        /// <param name="count">Add count</param>
        /// <returns></returns>
        public static YVRRequest AchievementAddCount(string achievementName, long count)
        {
            return YVRPlatform.isInitialized ? new YVRRequest(YVRPlatform.YVRAchievementUpdateAddCount(achievementName, count)) : null;
        }

        /// <summary>
        /// Add achievement fields progress
        /// </summary>
        /// <param name="achievementName">Achievement name</param>
        /// <param name="fields">Fields string</param>
        /// <returns></returns>
        public static YVRRequest AchievementAddFields(string achievementName, string fields)
        {
            return YVRPlatform.isInitialized ? new YVRRequest(YVRPlatform.YVRAchievementUpdateAddFields(achievementName, fields)) : null;
        }

        /// <summary>
        /// Unlock this achievement
        /// </summary>
        /// <param name="achievementName">Achievement name</param>
        /// <returns></returns>
        public static YVRRequest UnlockAchievement(string achievementName)
        {
            return YVRPlatform.isInitialized ? new YVRRequest(YVRPlatform.YVRAchievementUpdateUnlockAchievement(achievementName)) : null;
        }

        /// <summary>
        /// Get definitions of all achievements
        /// </summary>
        /// <returns></returns>
        public static YVRRequest<AchievementDefinitionList> GetAllDefinitions()
        {
            return YVRPlatform.isInitialized ? new YVRRequest<AchievementDefinitionList>(YVRPlatform.YVRAchievementDefinitionGetAllDefinitions()) : null;
        }

        /// <summary>
        /// Get definitions of designated achievements
        /// </summary>
        /// <param name="names">Designated achievement names</param>
        /// <returns></returns>
        public static YVRRequest<AchievementDefinitionList> GetDefinitionByName(string[] names)
        {
            return YVRPlatform.isInitialized ? new YVRRequest<AchievementDefinitionList>(YVRPlatform.YVRAchievementDefinitionGetDefinitionByName(names)) : null;
        }

        /// <summary>
        ///  Get progress of all achievements
        /// </summary>
        /// <returns></returns>
        public static YVRRequest<AchievementProgressList> GetAllProgress()
        {
            return YVRPlatform.isInitialized ? new YVRRequest<AchievementProgressList>(YVRPlatform.YVRAchievementProgressGetAllProgress()) : null;
        }

        /// <summary>
        /// Get progress of designated achievements
        /// </summary>
        /// <param name="names">Designated achievement names</param>
        /// <returns></returns>
        public static YVRRequest<AchievementProgressList> GetProgressByName(string[] names)
        {
            return YVRPlatform.isInitialized ? new YVRRequest<AchievementProgressList>(YVRPlatform.YVRAchievementProgressGetProgressByName(names)) : null;
        }
    }
}