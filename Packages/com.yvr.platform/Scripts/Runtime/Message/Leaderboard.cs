using YVR.Utilities;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate functions of LeaderBoard request
    /// </summary>
    public static class Leaderboard
    {
        /// <summary>
        /// Get leaderboard info by page
        /// </summary>
        /// <param name="leaderboardByPage">leaderboard request info</param>
        /// <returns></returns>
        public static YVRRequest<LeaderboardInfo> GetLeaderboardInfoByPage(LeaderboardByPage leaderboardByPage)
        {
            if (!YVRPlatform.isInitialized)
                return null;

            return new YVRRequest<LeaderboardInfo>(YVRPlatform.YVRLeaderboardGetList(leaderboardByPage.current,
                0, LeaderboardDataDirection.Backward.GetDescription(), leaderboardByPage.size, leaderboardByPage.leaderboardApiName,
                leaderboardByPage.pageType.GetDescription(), LeaderboardSortType.None.GetDescription()));
        }

        /// <summary>
        /// Get leaderboard info by leaderboardEntry
        /// </summary>
        /// <param name="leaderboardByRank"></param>
        /// <returns></returns>
        public static YVRRequest<LeaderboardInfo> GetLeaderboardInfoByRank(LeaderboardByRank leaderboardByRank)
        {
            if (!YVRPlatform.isInitialized)
                return null;

            return new YVRRequest<LeaderboardInfo>(YVRPlatform.YVRLeaderboardGetList(0, leaderboardByRank.currentStart,
                leaderboardByRank.dataDirection == LeaderboardDataDirection.None ? "" : leaderboardByRank.dataDirection.GetDescription(),
                leaderboardByRank.size, leaderboardByRank.leaderboardApiName,
                leaderboardByRank.pageType.GetDescription(), LeaderboardSortType.AroundOnView.GetDescription()));
        }

        /// <summary>
        /// Write a leaderboard entry
        /// </summary>
        /// <param name="leaderboardEntry">leaderboardEntry item info </param>
        /// <returns></returns>
        public static YVRRequest LeaderboardWriteItem(LeaderboardEntry leaderboardEntry)
        {
            if (!YVRPlatform.isInitialized)
                return null;

            leaderboardEntry.extraData ??= new sbyte[] { };

            return new YVRRequest(YVRPlatform.YVRLeaderboardAddItem(leaderboardEntry.leaderboardApiName, leaderboardEntry.score,
                leaderboardEntry.extraData, leaderboardEntry.extraDataLength, (int) leaderboardEntry.forceUpdate));
        }
    }
}