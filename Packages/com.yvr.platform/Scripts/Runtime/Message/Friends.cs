namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate functions of friends request
    /// </summary>
    public static class Friends
    {
        /// <summary>
        /// Get friends list
        /// </summary>
        /// <returns></returns>
        public static YVRRequest<FriendsList> GetFriends()
        {
            if (!YVRPlatform.isInitialized)
                return null;

            return new YVRRequest<FriendsList>(YVRPlatform.YVRFriendsGetYvrFriends());
        }

        /// <summary>
        /// Get friends information
        /// </summary>
        /// <returns></returns>
        public static YVRRequest<FriendInfo> GetFriendInformation(int accountID)
        {
            if (!YVRPlatform.isInitialized)
                return null;

            return new YVRRequest<FriendInfo>(YVRPlatform.YVRFriendsGetYvrFriendInfo(accountID));
        }
    }
}