using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Base class of yvrmessage class
    /// </summary>
    public class YVRMessage
    {
        /// <summary>
        /// Message type
        /// </summary>
        public string type { protected set; get; }

        /// <summary>
        /// Request ID of this message
        /// </summary>
        public int requestID { protected set; get; }

        /// <summary>
        /// Error data
        /// </summary>
        public YVRError error;

        /// <summary>
        /// Whether this message is error
        /// </summary>
        public bool isError;

        /// <summary>
        /// Execute callback function when handle message
        /// </summary>
        /// <param name="message"></param>
        public delegate void Callback(YVRMessage message);

        internal YVRMessage(AndroidJavaObject obj)
        {
            requestID = YVRPlatform.YVRMessageGetRequestID(obj);
            type = YVRPlatform.YVRMessageGetRequestType(obj);
            isError = YVRPlatform.YVRMessageIsError(obj);

            if (isError)
            {
                error = new YVRError(
                                     YVRPlatform.YVRErrorGetErrorCode(obj),
                                     YVRPlatform.YVRErrorGetErrorMessage(obj));

                Debug.LogError(
                               $"[YVRPlatform] Message is Error. \nErrorCode : {error.errorCode} \nErrorMessage : {error.errorMsg}");
            }
        }

        internal static YVRMessage PopMessage()
        {
            AndroidJavaObject obj = YVRPlatform.YVRPopMessage();

            if (obj == null)
                return null;

            YVRMessage msg = ParseMessageObj(obj);

            YVRPlatform.YVRMessageFreeRequest(msg.requestID);

            return msg;
        }

        internal static YVRMessage ParseMessageObj(AndroidJavaObject obj)
        {
            if (obj == null)
                return null;

            string msgType = YVRPlatform.YVRMessageGetRequestType(obj);
            YVRMessage msg;

            Debug.Log($"message type: {msgType}");

            switch (msgType)
            {
                case "reqUserInfo":
                    msg = new YVRMessageAccount(obj);
                    break;
                case "addAchieve":
                    msg = new YVRMessage(obj);
                    break;
                case "queryAchieveDefinition":
                    msg = new YVRMessageAchievementDefinitionList(obj);
                    break;
                case "queryAchieveProgress":
                    msg = new YVRMessageAchievementProgressList(obj);
                    break;
                case "reqIsUserEntitled":
                    msg = new YVRMessageEntitlement(obj);
                    break;
                case "getFriends":
                    msg = new YVRMessageFriendsList(obj);
                    break;
                case "getUserInfo":
                    msg = new YVRMessageFriendInfo(obj);
                    break;
                case "reqLeaderboard":
                    msg = new YVRMessageLeaderboardData(obj);
                    break;
                case "getProductsBySKU":
                    msg = new YVRMessageIAPProducts(obj);
                    break;
                case "launchCheckoutFlow":
                    msg = new YVRMessageIAPPurchaseInfo(obj);
                    break;
                case "getViewerPurchases":
                    msg = new YVRMessageIAPPurchasedProducts(obj);
                    break;
                case "addItem":
                    msg = new YVRMessage(obj);
                    break;
                case "getSportUserInfo":
                    msg = new YVRMessageSportUserInfo(obj);
                    break;
                case "getSummary":
                    msg = new YVRMessageSportSummary(obj);
                    break;
                case "getDailySummary":
                    msg = new YVRMessageSportDailySummary(obj);
                    break;
                default:
                    msg = new YVRMessage(obj);
                    break;
            }

            return msg;
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"Type:[{type ?? "null"}],\n\r");
            str.Append($"RequestID:[{requestID}],\n\r");
            str.Append($"Error:[{error}],\n\r");

            return str.ToString();
        }
    }

    /// <summary>
    /// Generics class of yvrmessage class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class YVRMessage<T> : YVRMessage
    {
        /// <summary>
        /// Execute callback function when handle message
        /// </summary>
        /// <param name="msg"></param>
        public new delegate void Callback(YVRMessage<T> msg);

        /// <summary>
        /// Final data of message
        /// </summary>
        public T data { protected set; get; }

        internal YVRMessage(AndroidJavaObject obj) : base(obj) { }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"Type:[{type ?? "null"}],\n\r");
            str.Append($"RequestID:[{requestID}],\n\r");
            str.Append($"Error:[{error}],\n\r");
            str.Append($"data:[{data?.ToString()}],\n\r");

            return str.ToString();
        }
    }

    /// <summary>
    /// Generics class of yvrmessage class with the data type : AccountData
    /// </summary>
    public class YVRMessageAccount : YVRMessage<AccountData>
    {
        internal YVRMessageAccount(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new AccountData(obj);
        }
    }

    /// <summary>
    /// Generics class of yvrmessage class with the data type : AchievementDefinitionList
    /// </summary>
    public class YVRMessageAchievementDefinitionList : YVRMessage<AchievementDefinitionList>
    {
        internal YVRMessageAchievementDefinitionList(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new AchievementDefinitionList(obj);
        }
    }

    /// <summary>
    /// Generics class of yvrmessage class with the data type : AchievementProgressList
    /// </summary>
    public class YVRMessageAchievementProgressList : YVRMessage<AchievementProgressList>
    {
        internal YVRMessageAchievementProgressList(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new AchievementProgressList(obj);
        }
    }

    /// <summary>
    /// Generics class of yvrmessage class with the data type : Entitlement
    /// </summary>
    public class YVRMessageEntitlement : YVRMessage<Entitlement>
    {
        internal YVRMessageEntitlement(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new Entitlement(obj);
        }
    }

    /// <summary>
    /// Generics class of yvrmessage class with the data type : FriendsList
    /// </summary>
    public class YVRMessageFriendsList : YVRMessage<FriendsList>
    {
        internal YVRMessageFriendsList(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new FriendsList(obj);
        }
    }

    /// <summary>
    /// Generics class of yvrmessage class with the data type : FriendInfo
    /// </summary>
    public class YVRMessageFriendInfo : YVRMessage<FriendInfo>
    {
        internal YVRMessageFriendInfo(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new FriendInfo(obj);
        }
    }

    public class YVRMessageLeaderboardData : YVRMessage<LeaderboardInfo>
    {
        internal YVRMessageLeaderboardData(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new LeaderboardInfo(obj);
        }
    }

    public class YVRMessageIAPProducts : YVRMessage<IAPProductList>
    {
        internal YVRMessageIAPProducts(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new IAPProductList(obj);
        }
    }

    public class YVRMessageIAPPurchaseInfo : YVRMessage<IAPPurchaseInfo>
    {
        internal YVRMessageIAPPurchaseInfo(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new IAPPurchaseInfo(obj);
        }
    }

    public class YVRMessageIAPPurchasedProducts : YVRMessage<IAPPurchasedProductList>
    {
        internal YVRMessageIAPPurchasedProducts(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new IAPPurchasedProductList(obj);
        }
    }

    public class YVRMessageSportUserInfo : YVRMessage<SportUserInfo>
    {
        internal YVRMessageSportUserInfo(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new SportUserInfo(obj);
        }
    }

    public class YVRMessageSportSummary : YVRMessage<SportSummary>
    {
        internal YVRMessageSportSummary(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new SportSummary(obj);
        }
    }

    public class YVRMessageSportDailySummary : YVRMessage<SportDailySummaryList>
    {
        internal YVRMessageSportDailySummary(AndroidJavaObject obj) : base(obj)
        {
            if (!isError) data = new SportDailySummaryList(obj);
        }
    }
}