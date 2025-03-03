using System.ComponentModel;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of leaderboard entry
    /// </summary>
    public class LeaderboardEntry
    {
        /// <summary>
        /// Leaderboard name
        /// </summary>
        public string leaderboardApiName;

        /// <summary>
        /// LeaderboardEntry score
        /// </summary>
        public double score;

        /// <summary>
        /// LeaderboardEntry extra data
        /// </summary>
        public sbyte[] extraData;

        /// <summary>
        /// LeaderboardEntry extra data length
        /// </summary>
        public long extraDataLength;

        /// <summary>
        /// LeaderboardEntry update policy
        /// </summary>
        public LeaderboardUpdatePolicy forceUpdate;
    }

    public enum LeaderboardUpdatePolicy
    {
        [Description("alwaysUpdate")]
        AlwaysUpdate = 0,
        [Description("bestScoreUpdate")]
        BestScoreUpdate = 1
    }
}