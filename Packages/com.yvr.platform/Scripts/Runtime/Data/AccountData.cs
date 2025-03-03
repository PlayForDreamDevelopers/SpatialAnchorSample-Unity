using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of account data
    /// </summary>
    public class AccountData
    {
        /// <summary>
        /// User's account ID
        /// </summary>
        public readonly int accountID;

        /// <summary>
        /// User's nickname
        /// </summary>
        public readonly string nickname;

        /// <summary>
        /// User's avatar url
        /// </summary>
        public readonly string avatar;

        /// <summary>
        /// User's gender, 1:man, 2:women, 3:unknown
        /// </summary>
        public readonly int gender;

        public AccountData(AndroidJavaObject obj)
        {
            accountID = YVRPlatform.YVRAccountGetAccountID(obj);
            nickname = YVRPlatform.YVRAccountGetUserName(obj);
            avatar = YVRPlatform.YVRAccountGetUserIcon(obj);
            gender = YVRPlatform.YVRAccountGetUserSex(obj);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"accountID:[{accountID}],\n\r");
            str.Append($"nickname:[{nickname ?? "null"}],\n\r");
            str.Append($"avatar:[{avatar ?? "null"}],\n\r");
            str.Append($"gender:[{gender}],\n\r");

            return str.ToString();
        }
    }
}