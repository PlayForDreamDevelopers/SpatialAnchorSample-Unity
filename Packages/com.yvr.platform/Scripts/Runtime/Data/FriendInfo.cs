using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of friend info
    /// </summary>
    public class FriendInfo
    {
        /// <summary>
        /// Friend's account ID
        /// </summary>
        public readonly int accountID;

        /// <summary>
        /// Friend's nickname
        /// </summary>
        public readonly string nickname;

        /// <summary>
        /// Friend's avatar url
        /// </summary>
        public readonly string avatar;

        /// <summary>
        /// Friend's gender, 1:man, 2:women, 3:unknown
        /// </summary>
        public readonly int gender;

        /// <summary>
        /// Friend's age
        /// </summary>
        public readonly int age;

        /// <summary>
        /// Friend's online state, 1:online, 2:offline
        /// </summary>
        public readonly int onlineState;

        public FriendInfo(AndroidJavaObject obj)
        {
            accountID = YVRPlatform.YVRFriendsGetActIdOfUser(obj);
            nickname = YVRPlatform.YVRFriendsGetNickOfUser(obj);
            avatar = YVRPlatform.YVRFriendsGetIconOfUser(obj);
            gender = YVRPlatform.YVRFriendsGetSexOfUser(obj);
            age = YVRPlatform.YVRFriendsGetAgeOfUser(obj);
            onlineState = YVRPlatform.YVRFriendsGetOnlineOfUser(obj);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"accountID:[{accountID}],\n\r");
            str.Append($"nickname:[{nickname ?? "null"}],\n\r");
            str.Append($"avatar:[{avatar ?? "null"}],\n\r");
            str.Append($"gender:[{gender}],\n\r");
            str.Append($"age:[{age}],\n\r");
            str.Append($"onlineState:[{onlineState}],\n\r");

            return str.ToString();
        }
    }
}