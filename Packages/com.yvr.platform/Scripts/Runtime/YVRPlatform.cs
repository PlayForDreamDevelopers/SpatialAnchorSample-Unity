using UnityEngine;
using YVR.Utilities;
#if YVR_CORE
using YVR.Core.XR;
#endif

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate all android API
    /// </summary>
    public class YVRPlatform
    {
        private static AndroidJavaClass s_PermissionJavaClass
            = new AndroidJavaClass("com.yvr.thirdsdk.YvrPermission");

        private static AndroidJavaClass s_RequestJavaClass = new AndroidJavaClass("com.yvr.thirdsdk.Request");

        private static AndroidJavaClass s_AccountManagerJavaClass
            = new AndroidJavaClass("com.yvr.thirdsdk.Account");

        private static AndroidJavaClass s_AchievementJavaClass
            = new AndroidJavaClass("com.yvr.thirdsdk.Achievement");

        private static AndroidJavaClass s_DeeplinkJavaClass
            = new AndroidJavaClass("com.yvr.thirdsdk.ContextUtil");

        private static AndroidJavaClass s_FriendsJavaClass
            = new AndroidJavaClass("com.yvr.thirdsdk.FriendsManager");

        private static AndroidJavaClass s_LeaderboardJavaClass
            = new AndroidJavaClass("com.yvr.thirdsdk.LeaderBoard");

        private static AndroidJavaClass s_SportJavaClass
            = new AndroidJavaClass("com.yvr.thirdsdk.Sport");

        private static AndroidJavaClass s_IAPJavaClass = new AndroidJavaClass("com.yvr.thirdsdk.IAP");
        private static AndroidJavaClass s_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        private static AndroidJavaObject s_CurrentActivity
            = s_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        /// <summary>
        /// Whether platform sdk is initialized
        /// </summary>
        public static bool isInitialized { private set; get; }

        /// <summary>
        /// Initialize platform sdk
        /// </summary>
        /// <param name="appId"></param>
        public static void Initialize(long appId = 0)
        {
#if YVR_CORE
            if (appId == 0) long.TryParse(YVRPlatformSetting.Instance.appID, out appId);
#endif
            Debug.Log($"App ID : {appId}");

            isInitialized = YVRInitializePlatformInit(appId);

            if (!isInitialized)
                throw new UnityException("[YVRPlatform] YVR Platform failed to initialize.");

            new GameObject("YVRCallbackRunner").AddComponent<YVRCallbackRunner>();

            PackageVersion.PrintPackagesVersion(typeof(YVRPlatform));
        }

        #region Entitlement

        internal static bool YVRInitializePlatformInit(long appId)
        {
            return s_RequestJavaClass.CallStatic<bool>("initYvrPlatformSdk", appId, null);
        }

        internal static int YVRPermissionGetViewerEntitled()
        {
            return s_PermissionJavaClass.CallStatic<int>("getViewerEntitled");
        }

        internal static bool YVRPermissionIsViewerEntitled(AndroidJavaObject obj)
        {
            return s_PermissionJavaClass.CallStatic<bool>("isViewerEntitled", obj);
        }

        #endregion

        #region Request

        internal static AndroidJavaObject YVRPopMessage()
        {
            return s_RequestJavaClass.CallStatic<AndroidJavaObject>("yvr_PopMessage");
        }

        internal static int YVRMessageGetRequestID(AndroidJavaObject obj)
        {
            return s_RequestJavaClass.CallStatic<int>("yvr_getRequestId", obj);
        }

        internal static string YVRMessageGetRequestType(AndroidJavaObject obj)
        {
            return s_RequestJavaClass.CallStatic<string>("yvr_getRequestType", obj);
        }

        internal static bool YVRMessageIsError(AndroidJavaObject obj)
        {
            return s_RequestJavaClass.CallStatic<bool>("yvr_isError", obj);
        }

        internal static void YVRMessageFreeRequest(int requestID)
        {
            s_RequestJavaClass.CallStatic("yvr_freeRequest", requestID);
        }

        #endregion

        #region Error

        internal static string YVRErrorGetErrorMessage(AndroidJavaObject obj)
        {
            return s_RequestJavaClass.CallStatic<string>("yvr_getErrorMsg", obj);
        }

        internal static int YVRErrorGetErrorCode(AndroidJavaObject obj)
        {
            return s_RequestJavaClass.CallStatic<int>("yvr_getErrorCode", obj);
        }

        #endregion

        #region Account

        internal static int YVRAccountGetLoggedInUser()
        {
            return s_AccountManagerJavaClass.CallStatic<int>("yvr_user_GetLoggedInUser");
        }

        internal static int YVRAccountGetAccountID(AndroidJavaObject obj)
        {
            return s_AccountManagerJavaClass.CallStatic<int>("getYvrAccountId", obj);
        }

        internal static string YVRAccountGetUserName(AndroidJavaObject obj)
        {
            return s_AccountManagerJavaClass.CallStatic<string>("getYvrUserName", obj);
        }

        internal static string YVRAccountGetUserIcon(AndroidJavaObject obj)
        {
            return s_AccountManagerJavaClass.CallStatic<string>("getYvrUserIcon", obj);
        }

        internal static int YVRAccountGetUserSex(AndroidJavaObject obj)
        {
            return s_AccountManagerJavaClass.CallStatic<int>("getYvrUserSex", obj);
        }

        #endregion

        #region Achievement

        internal static int YVRAchievementUpdateAddCount(string achievementName, long count)
        {
            return s_AchievementJavaClass.CallStatic<int>("addCount", achievementName, count);
        }

        internal static int YVRAchievementUpdateAddFields(string achievementName, string fields)
        {
            return s_AchievementJavaClass.CallStatic<int>("addFields", achievementName, fields);
        }

        internal static int YVRAchievementUpdateUnlockAchievement(string achievementName)
        {
            return s_AchievementJavaClass.CallStatic<int>("unlockAchievement", achievementName);
        }

        internal static int YVRAchievementDefinitionGetAllDefinitions()
        {
            return s_AchievementJavaClass.CallStatic<int>("getAllDefinitions");
        }

        internal static int YVRAchievementDefinitionGetDefinitionByName(string[] names)
        {
            return s_AchievementJavaClass.CallStatic<int>("getDefinitionByNames", names.JavaArrayFromCs());
        }

        internal static int YVRAchievementDefinitionGetSizeOfAllDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getSizeOfAllDefinitions", obj);
        }

        internal static AndroidJavaObject
            YVRAchievementDefinitionGetElementOfDefinitions(AndroidJavaObject obj, int index)
        {
            return s_AchievementJavaClass.CallStatic<AndroidJavaObject>("getElementOfDefinitions", obj, index);
        }

        internal static int YVRAchievementDefinitionGetIdFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getIdFromElementOfDefinitions", obj);
        }

        internal static string YVRAchievementDefinitionGetApiNameFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<string>("getApiNameFromElementOfDefinitions", obj);
        }

        internal static int YVRAchievementDefinitionGetTypeFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getAchievementTypeFromElementOfDefinitions", obj);
        }

        internal static int YVRAchievementDefinitionGetPolicyFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getPolicyFromElementOfDefinitions", obj);
        }

        internal static int YVRAchievementDefinitionGetTargetFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getTargetFromElementOfDefinitions", obj);
        }

        internal static int YVRAchievementDefinitionGetBitfieldLengthFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getBitfieldLengthFromElementOfDefinitions", obj);
        }

        internal static bool YVRAchievementDefinitionGetIsAchievedFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<bool>("getIsAchievedFromElementOfDefinitions", obj);
        }

        internal static string YVRAchievementDefinitionGetTitleFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<string>("getTitleFromElementOfDefinitions", obj);
        }

        internal static string YVRAchievementDefinitionGetDescriptionFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<string>("getDescriptionFromElementOfDefinitions", obj);
        }

        internal static string
            YVRAchievementDefinitionGetUnlockedDescriptionFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<string>("getUnlockedDescriptionFromElementOfDefinitions", obj);
        }

        internal static bool YVRAchievementDefinitionGetIsSecretFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<bool>("getIsSecretFromElementOfDefinitions", obj);
        }

        internal static string YVRAchievementDefinitionGetLockedImageFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<string>("getLockedImageFromElementOfDefinitions", obj);
        }

        internal static string YVRAchievementDefinitionGetUnlockedImageFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<string>("getUnlockedImageFromElementOfDefinitions", obj);
        }

        internal static long YVRAchievementDefinitionGetCreatedTimeFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<long>("getCreatedTimeFromElementOfDefinitions", obj);
        }

        internal static long YVRAchievementDefinitionGetUpdateTimeFromElementOfDefinitions(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<long>("getUpdateTimeFromElementOfDefinitions", obj);
        }

        internal static int YVRAchievementProgressGetAllProgress()
        {
            return s_AchievementJavaClass.CallStatic<int>("GetAllProgress");
        }

        internal static int YVRAchievementProgressGetProgressByName(string[] names)
        {
            return s_AchievementJavaClass.CallStatic<int>("GetProgressByName", names.JavaArrayFromCs());
        }

        internal static int YVRAchievementProgressGetSizeOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getSizeOfAchievementProgress", obj);
        }

        internal static AndroidJavaObject
            YVRAchievementProgressGetElementOfAchievementProgress(AndroidJavaObject obj, int index)
        {
            return s_AchievementJavaClass.CallStatic<AndroidJavaObject>("getElementOfAchievementProgress", obj, index);
        }

        internal static int YVRAchievementProgressGetIdFromElementOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getIdFromElementOfAchievementProgress", obj);
        }

        internal static AndroidJavaObject
            YVRAchievementProgressGetDefinitionFromElementOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<AndroidJavaObject>("getDefinitionFromElementOfAchievementProgress",
                                                                        obj);
        }

        internal static string YVRAchievementProgressGetNameFromDefinitionOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<string>("getNameFromDefinitionOfAchievementProgress", obj);
        }

        internal static int YVRAchievementProgressGetTargetFromDefinitionOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getTargetFromDefinitionOfAchievementProgress", obj);
        }

        internal static int
            YVRAchievementProgressGetCountProgressFromElementOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<int>("getCountProgressFromElementOfAchievementProgress", obj);
        }

        internal static string
            YVRAchievementProgressGetBitfieldProgressFromElementOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<string>("getBitfieldProgressFromElementOfAchievementProgress",
                                                             obj);
        }

        internal static bool YVRAchievementProgressIsUnlockedFromElementOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<bool>("isUnlockedFromElementOfAchievementProgress", obj);
        }

        internal static long YVRAchievementProgressGetUnlockTimeFromElementOfAchievementProgress(AndroidJavaObject obj)
        {
            return s_AchievementJavaClass.CallStatic<long>("getUnlockTimeFromElementOfAchievementProgress", obj);
        }

        #endregion

        #region DeepLink

        internal static bool YVRDeeplinkIsDeeplinkLaunch()
        {
            return s_DeeplinkJavaClass.CallStatic<bool>("isDeeplinkLaunch");
        }

        internal static string YVRDeeplinkGetDeeplinkRoomId()
        {
            return s_DeeplinkJavaClass.CallStatic<string>("getDeeplinkRoomId");
        }

        internal static string YVRDeeplinkGetDeeplinkApiName()
        {
            return s_DeeplinkJavaClass.CallStatic<string>("getDeeplinkApiName");
        }

        #endregion

        #region Friends

        internal static int YVRFriendsGetYvrFriends() { return s_FriendsJavaClass.CallStatic<int>("getYvrFriends"); }

        internal static int YVRFriendsGetSizeOfFriends(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getFriendsSize", obj);
        }

        internal static AndroidJavaObject YVRFriendsGetElementOfFriends(AndroidJavaObject obj, int index)
        {
            return s_FriendsJavaClass.CallStatic<AndroidJavaObject>("getItemOfFriendsList", obj, index);
        }

        internal static int YVRFriendsGetActIdOfFriendItem(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getActIdOfFriendItem", obj);
        }

        internal static string YVRFriendsGetNickOfFriendItem(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<string>("getNickOfFriendItem", obj);
        }

        internal static int YVRFriendsGetAgeOfFriendItem(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getAgeOfFriendItem", obj);
        }

        internal static int YVRFriendsGetSexOfFriendItem(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getSexOfFriendItem", obj);
        }

        internal static string YVRFriendsGetIconOfFriendItem(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<string>("getIconOfFriendItem", obj);
        }

        internal static int YVR_Friends_GetOnlineOfFriendItem(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getOnlineOfFriendItem", obj);
        }

        internal static AndroidJavaObject YVRFriendsGetUsingAppOfFriendItem(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<AndroidJavaObject>("getUsingAppOfFriendItem", obj);
        }

        internal static string YVRFriendsGetScoverOfUsingApp(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<string>("getScoverOfUsingApp", obj);
        }

        internal static int YVRFriendsGetTypeOfUsingApp(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getTypeOfUsingApp", obj);
        }

        internal static string YVRFriendsGetPkgOfUsingApp(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<string>("getPkgOfUsingApp", obj);
        }

        internal static string YVRFriendsGetNameOfUsingApp(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<string>("getNameOfUsingApp", obj);
        }

        internal static int YVRFriendsGetYvrFriendInfo(int accountID)
        {
            return s_FriendsJavaClass.CallStatic<int>("getYvrFriendInfo", accountID);
        }

        internal static int YVRFriendsGetActIdOfUser(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getActIdOfUser", obj);
        }

        internal static string YVRFriendsGetNickOfUser(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<string>("getNickOfUser", obj);
        }

        internal static int YVRFriendsGetAgeOfUser(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getAgeOfUser", obj);
        }

        internal static int YVRFriendsGetSexOfUser(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getSexOfUser", obj);
        }

        internal static string YVRFriendsGetIconOfUser(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<string>("getIconOfUser", obj);
        }

        internal static int YVRFriendsGetOnlineOfUser(AndroidJavaObject obj)
        {
            return s_FriendsJavaClass.CallStatic<int>("getOnlineOfUser", obj);
        }

        #endregion

        #region LeaderBoard

        internal static int YVRLeaderboardAddItem(string leaderboardApiName, double score, sbyte[] extraData,
                                                  long extraDataLength, int forceUpdate)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("addItem", leaderboardApiName, score, extraData,
                                                          extraDataLength, forceUpdate);
        }

        internal static int YVRLeaderboardGetList(long current, long currentStart, string dataDirection,
                                                  long size, string leaderboardApiName, string pageType,
                                                  string sortType)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("reqLeaderboardList", current, currentStart, dataDirection,
                                                          size, leaderboardApiName, pageType, sortType);
        }

        internal static int YVRLeaderboardGetCurrentPageIndex(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getReqLDBCurrPageIndex", obj);
        }

        internal static int YVRLeaderboardGetPageSize(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getReqLDBPageSize", obj);
        }

        internal static int YVRLeaderboardGetTotalPages(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getReqLDBTotalPages", obj);
        }

        internal static int YVRLeaderboardGetTotalCount(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getReqLDBTotalCount", obj);
        }

        internal static AndroidJavaObject YVRLeaderboardGetRankList(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<AndroidJavaObject>("getReqLDBLists", obj);
        }

        internal static int YVRLeaderboardGetRankListSize(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getSizeOfLDBLists", obj);
        }

        internal static AndroidJavaObject YVRLeaderboardGetRankItem(AndroidJavaObject obj, int index)
        {
            return s_LeaderboardJavaClass.CallStatic<AndroidJavaObject>("getItemOfLDBLists", obj, index);
        }

        internal static long YVRLeaderboardGetAppIdByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<long>("getLeaderboardApiIdOfCurrLDB", obj);
        }

        internal static string YVRLeaderboardGetLdbNameByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<string>("getLeaderboardApiNameOfCurrLDB", obj);
        }

        internal static long YVRLeaderboardGetUserIDByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<long>("getLeaderboardUserIdOfCurrLDB", obj);
        }

        internal static string YVRLeaderboardGetUserNameByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<string>("getLeaderboardUserNameOfCurrLDB", obj);
        }

        internal static string YVRLeaderboardGetUserIconByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<string>("getLeaderboardUserIconOfCurrLDB", obj);
        }

        internal static int YVRLeaderboardGetUserGenderByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getLeaderboardUserSexOfCurrLDB", obj);
        }

        internal static double YVRLeaderboardGetScoreByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<double>("getLeaderboardScoreOfCurrLDB", obj);
        }

        internal static sbyte[] YVRLeaderboardGetExtraDataByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<sbyte[]>("getLeaderboardExtraOfCurrLDB", obj);
        }

        internal static long YVRLeaderboardGetExtraLengthByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<long>("getLeaderboardExtraLengthOfCurrLDB", obj);
        }

        internal static long YVRLeaderboardGetSortByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<long>("getLeaderboardSortOfCurrLDB", obj);
        }

        internal static string YVRLeaderboardGetAddTimeByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<string>("getLeaderboardAddTimeOfCurrLDB", obj);
        }

        internal static string YVRLeaderboardGetUpdateTimeByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<string>("getLeaderboardUpdateTimeOfCurrLDB", obj);
        }

        internal static int YVRLeaderboardGetUpdatePolicyByRank(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getLeaderboardForceUpdateOfCurrLDB", obj);
        }

        internal static string YVRLeaderboardGetUserWearingAvatarOfCurrLDB(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<string>("getLeaderboardUserWearingAvatarOfCurrLDB", obj);
        }

        internal static string YVRLeaderboardGetUserWearingBorderOfCurrLDB(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<string>("getLeaderboardUserWearingBorderOfCurrLDB", obj);
        }

        internal static int YVRLeaderboardGetUserWearingAvatarCgyOfCurrLDB(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getLeaderboardUserAvatarCgyOfCurrLDB", obj);
        }

        internal static int YVRLeaderboardGetUserWearingBorderCgyOfCurrLDB(AndroidJavaObject obj)
        {
            return s_LeaderboardJavaClass.CallStatic<int>("getLeaderboardUserBorderCgyOfCurrLDB", obj);
        }

        #endregion

        #region IAP

        internal static int YVRIAPGetProducts(string[] uniqueIds)
        {
            return s_IAPJavaClass.CallStatic<int>("getProductsBySKU", uniqueIds?.JavaArrayFromCs());
        }

        internal static int YVRIAPGetSizeOfProducts(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<int>("getProductSize", obj);
        }

        internal static AndroidJavaObject YVRIAPGetElementOfProduct(AndroidJavaObject obj, int index)
        {
            return s_IAPJavaClass.CallStatic<AndroidJavaObject>("getProductByIndex", obj, index);
        }

        internal static long YVRIAPGetAppIdOfProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<long>("getAppIdOfProduct", obj);
        }

        internal static string YVRIAPGetUniqueIdOfProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<string>("getSkuOfProduct", obj);
        }

        internal static string YVRIAPGetNameOfProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<string>("getNameOfProduct", obj);
        }

        internal static int YVRIAPGetTypeOfProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<int>("getTypeOfProduct", obj);
        }

        internal static float YVRIAPGetPriceOfProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<float>("getPriceOfProduct", obj);
        }

        internal static string YVRIAPGetIconOfProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<string>("getScoverOfProduct", obj);
        }

        internal static string YVRIAPGetDescriptionOfProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<string>("getBriefOfProduct", obj);
        }

        internal static int YVRIAPInitiatePurchase(string uniqueId, float price)
        {
            return s_IAPJavaClass.CallStatic<int>("launchCheckoutFlow", uniqueId, price);
        }

        internal static AndroidJavaObject YVRIAPGetPurchaseInfo(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<AndroidJavaObject>("getPurchaseByMessage", obj);
        }

        internal static string YVRIAPGetIconOfPurchasedProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<string>("getScoverOfPurchase", obj);
        }

        internal static string YVRIAPGetOrderIdOfPurchasedProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<string>("getTradeNoOfPurchase", obj);
        }

        internal static int YVRIAPGetTypeOfPurchasedProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<int>("getTypeOfPurchase", obj);
        }

        internal static string YVRIAPGetUniqueIdOfPurchasedProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<string>("getSkuOfPurchase", obj);
        }

        internal static string YVRIAPGetNameOfPurchasedProduct(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<string>("getNameOfPurchase", obj);
        }

        internal static int YVRIAPGetPurchasedProducts()
        {
            return s_IAPJavaClass.CallStatic<int>("getViewerPurchases");
        }

        internal static int YVRIAPGetSizeOfPurchasedProducts(AndroidJavaObject obj)
        {
            return s_IAPJavaClass.CallStatic<int>("getPurchaseSize", obj);
        }

        internal static AndroidJavaObject YVRIAPGetElementOfPurchasedProduct(AndroidJavaObject obj, int index)
        {
            return s_IAPJavaClass.CallStatic<AndroidJavaObject>("getPurchaseByIndex", obj, index);
        }

        internal static int YVRIAPConsumePurchasedProduct(string uniqueId)
        {
            return s_IAPJavaClass.CallStatic<int>("consumePurchase", uniqueId);
        }

        #endregion

        #region Sport

        internal static int YVRSportGetUserInfo() { return s_SportJavaClass.CallStatic<int>("getUserInfo"); }

        internal static int YVRSportGetGender(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getGender", obj);
        }

        internal static int YVRSportGetBirthday(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getBirthDay", obj);
        }

        internal static int YVRSportGetStature(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getStature", obj);
        }

        internal static int YVRSportGetWeight(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getWeight", obj);
        }

        internal static int YVRSportGetPlanCalories(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getPlanCalorie", obj);
        }

        internal static int YVRSportGetPlanDurationInSeconds(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getPlanDurationInSeconds", obj);
        }

        internal static int YVRSportGetDaysPerWeek(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getDaysPerWeek", obj);
        }

        internal static int YVRSportGetSummary(long beginTime, long endTime)
        {
            return s_SportJavaClass.CallStatic<int>("getSummary", beginTime, endTime);
        }

        internal static long YVRSportGetSummaryDurationInSeconds(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<long>("getSummaryDurationInSeconds", obj);
        }

        internal static double YVRSportGetSummaryCalorie(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<double>("getSummaryCalorie", obj);
        }

        internal static int YVRSportGetDailySummary(long beginTime, long endTime)
        {
            return s_SportJavaClass.CallStatic<int>("getDailySummary", beginTime, endTime);
        }

        internal static int YVRSportGetDailySummarySize(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getDailySummarySize", obj);
        }

        internal static AndroidJavaObject YVRSportGetDailySummaryByIndex(AndroidJavaObject obj, int index)
        {
            return s_SportJavaClass.CallStatic<AndroidJavaObject>("getDailySummaryByIndex", obj, index);
        }

        internal static string YVRSportGetDailySummaryDate(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<string>("getDailySummaryDate", obj);
        }

        internal static int YVRSportGetDailySummaryPlanDurationInSeconds(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<int>("getDailySummaryPlanDurationInSeconds", obj);
        }

        internal static long YVRSportGetDailySummaryDurationInSeconds(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<long>("getDailySummaryDurationInSeconds", obj);
        }

        internal static double YVRSportGetDailySummaryCalorie(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<double>("getDailySummaryCalorie", obj);
        }

        internal static double YVRSportGetDailySummaryPlanCalorie(AndroidJavaObject obj)
        {
            return s_SportJavaClass.CallStatic<double>("getDailySummaryPlanCalorie", obj);
        }

        #endregion
    }
}