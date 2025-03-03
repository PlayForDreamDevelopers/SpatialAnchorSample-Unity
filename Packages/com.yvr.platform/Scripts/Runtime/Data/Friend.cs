using System.Collections.Generic;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of friend
    /// </summary>
    public class Friend
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

        /// <summary>
        /// App url of which friend is using. This represents that friend is not using any app if is null or empty.
        /// </summary>
        public readonly string usingAppUrl;

        /// <summary>
        /// App type of which friend is using, 1:game, 2:application. This represents that friend is not using any app if zero.
        /// </summary>
        public readonly int usingAppType = 0;

        /// <summary>
        /// App name of which friend is using. This represents that friend is not using any app if is null or empty.
        /// </summary>
        public readonly string usingAppName;

        public Friend(AndroidJavaObject obj)
        {
            accountID = YVRPlatform.YVRFriendsGetActIdOfFriendItem(obj);
            nickname = YVRPlatform.YVRFriendsGetNickOfFriendItem(obj);
            avatar = YVRPlatform.YVRFriendsGetIconOfFriendItem(obj);
            gender = YVRPlatform.YVRFriendsGetSexOfFriendItem(obj);
            age = YVRPlatform.YVRFriendsGetAgeOfFriendItem(obj);
            onlineState = YVRPlatform.YVR_Friends_GetOnlineOfFriendItem(obj);
            AndroidJavaObject usingAppInfo = YVRPlatform.YVRFriendsGetUsingAppOfFriendItem(obj);

            if (usingAppInfo != null)
            {
                usingAppUrl = YVRPlatform.YVRFriendsGetScoverOfUsingApp(usingAppInfo);
                usingAppType = YVRPlatform.YVRFriendsGetTypeOfUsingApp(usingAppInfo);
                usingAppName = YVRPlatform.YVRFriendsGetNameOfUsingApp(usingAppInfo);
            }
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
            str.Append($"usingAppUrl:[{usingAppUrl ?? "null"}],\n\r");
            str.Append($"usingAppType:[{usingAppType}],\n\r");
            str.Append($"usingAppName:[{usingAppName ?? "null"}],\n\r");

            return str.ToString();
        }
    }

    /// <summary>
    /// The storage of friend data
    /// </summary>
    public class FriendsList : DeserializableList<Friend>
    {
        public FriendsList(AndroidJavaObject obj)
        {
            int count = YVRPlatform.YVRFriendsGetSizeOfFriends(obj);

            data = new List<Friend>(count);

            for (int i = 0; i < count; i++)
                data.Add(new Friend(YVRPlatform.YVRFriendsGetElementOfFriends(obj, i)));
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            foreach (var item in data)
                str.Append(item + "\n\r");

            return str.ToString();
        }
    }
}