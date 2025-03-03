namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate functions of account request
    /// </summary>
    public static class Account
    {
        /// <summary>
        /// Get logged in user
        /// </summary>
        /// <returns></returns>
        public static YVRRequest<AccountData> GetLoggedInUser()
        {
            return YVRPlatform.isInitialized ? new YVRRequest<AccountData>(YVRPlatform.YVRAccountGetLoggedInUser()) : null;
        }
    }
}