using System;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Leaderboard entry info
    /// </summary>
    public class LeaderboardEntryInfo
    {
        /// <summary>
        /// The leaderboard id
        /// </summary>
        public readonly long leaderboardApiId;

        /// <summary>
        /// User id
        /// </summary>
        public readonly long id;

        /// <summary>
        /// User name
        /// </summary>
        public readonly string name;

        /// <summary>
        /// User icon
        /// </summary>
        public readonly string icon;

        /// <summary>
        /// User gender
        /// </summary>
        public readonly int gender;

        /// <summary>
        /// User score
        /// </summary>
        public readonly double score;

        /// <summary>
        /// User extra data
        /// </summary>
        public readonly sbyte[] extraData;

        /// <summary>
        /// Extra data length
        /// </summary>
        public readonly long extraDataLength;

        /// <summary>
        /// LeaderboardEntryInfo sort
        /// </summary>
        public readonly long sort;

        /// <summary>
        /// LeaderboardEntryInfo add time
        /// </summary>
        public readonly string addTime;

        /// <summary>
        /// LeaderboardEntryInfo update time
        /// </summary>
        public readonly string updateTime;

        /// <summary>
        /// Redemption avatars used by users
        /// </summary>
        public readonly string wearingAvatar;

        /// <summary>
        /// Redemption avatar border used by users
        /// </summary>
        public readonly string wearingBorder;

        /// <summary>
        ///Avatar category 2: static avatar 3: dynamic avatar
        /// </summary>
        public readonly int avatarCgy;

        /// <summary>
        /// Border Category 1: Static Border 4: Dynamic Border
        /// </summary>
        public readonly int borderCgy;

        /// <summary>
        ///  Update policy
        /// </summary>
        public readonly LeaderboardUpdatePolicy updatePolicy;

        public LeaderboardEntryInfo(AndroidJavaObject obj)
        {
            leaderboardApiId = YVRPlatform.YVRLeaderboardGetAppIdByRank(obj);
            id = YVRPlatform.YVRLeaderboardGetUserIDByRank(obj);
            name = YVRPlatform.YVRLeaderboardGetUserNameByRank(obj);
            icon = YVRPlatform.YVRLeaderboardGetUserIconByRank(obj);
            gender = YVRPlatform.YVRLeaderboardGetUserGenderByRank(obj);
            score = YVRPlatform.YVRLeaderboardGetScoreByRank(obj);
            extraData = YVRPlatform.YVRLeaderboardGetExtraDataByRank(obj);
            extraDataLength = YVRPlatform.YVRLeaderboardGetExtraLengthByRank(obj);
            sort = YVRPlatform.YVRLeaderboardGetSortByRank(obj);
            addTime = YVRPlatform.YVRLeaderboardGetAddTimeByRank(obj);
            updateTime = YVRPlatform.YVRLeaderboardGetUpdateTimeByRank(obj);
            updatePolicy = (LeaderboardUpdatePolicy)YVRPlatform.YVRLeaderboardGetUpdatePolicyByRank(obj);
            wearingAvatar = YVRPlatform.YVRLeaderboardGetUserWearingAvatarOfCurrLDB(obj);
            wearingBorder = YVRPlatform.YVRLeaderboardGetUserWearingBorderOfCurrLDB(obj);
            avatarCgy = YVRPlatform.YVRLeaderboardGetUserWearingAvatarCgyOfCurrLDB(obj);
            borderCgy = YVRPlatform.YVRLeaderboardGetUserWearingBorderCgyOfCurrLDB(obj);

        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"leaderboardApiId:{leaderboardApiId}\n");
            str.Append($"id:{id}\n");
            str.Append($"name:{name}\n");
            str.Append($"icon:{icon}\n");
            str.Append($"score:{score}\n");
            str.Append(extraDataLength > 0 && extraData?.Length > 0
                ? $"extraData:{System.Text.Encoding.UTF8.GetString(Array.ConvertAll(extraData, (a) => (byte)a))}\n"
                : $"extraData: null\n");
            str.Append($"extraDataLength:{extraDataLength}\n");
            str.Append($"sort:{sort}\n");
            str.Append($"addTime:{addTime}\n");
            str.Append($"updateTime:{updateTime}\n");
            str.Append($"updatePolicy:{updatePolicy}\n");
            str.Append($"wearingAvatar:{wearingAvatar}\n");
            str.Append($"wearingBorder:{wearingBorder}\n");
            str.Append($"avatarCgy:{avatarCgy}\n");
            str.Append($"borderCgy:{borderCgy}\n");

            return str.ToString();
        }
    }
}