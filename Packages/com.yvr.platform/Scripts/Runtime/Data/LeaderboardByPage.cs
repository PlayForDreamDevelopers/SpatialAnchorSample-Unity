using System.ComponentModel;

namespace YVR.Platform
{
    /// <summary>
    /// Leaderboard by page request data
    /// </summary>
    public class LeaderboardByPage
    {
        /// <summary>
        /// Current page
        /// </summary>
        public long current;

        /// <summary>
        /// Page size
        /// </summary>
        public long size;

        /// <summary>
        /// Leaderboard name
        /// </summary>
        public string leaderboardApiName;

        /// <summary>
        /// Page type, all data: none, friends data: friends
        /// </summary>
        public LeaderboardPageType pageType;

    }

    public enum LeaderboardDataDirection
    {
        [Description("none")]
        None,
        [Description("forward")]
        Forward,
        [Description("backward")]
        Backward
    }

    public enum LeaderboardPageType
    {
        [Description("none")]
        None,
        [Description("friends")]
        Friends
    }

    public enum LeaderboardSortType
    {
        [Description("none")]
        None,
        [Description("aroundOnView")]
        AroundOnView
    }
}