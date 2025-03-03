using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YVR.Platform;

public class PlatformTester
{
    private static string s_AchievementNameCache = "";
    public static int achievementProgress = 0;

    #region Entitlement

    public static void EntitlementCheck() { PlatformCore.GetViewerEntitled().OnComplete(GetEntitlementCallback); }

    public static void GetEntitlementCallback(YVRMessage<Entitlement> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetEntitlementCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetEntitlementCallback));
    }

    #endregion

    #region Account

    public static void LoggedInAccountCheck() { Account.GetLoggedInUser().OnComplete(GetLoggedInAccountCallback); }

    public static void GetLoggedInAccountCallback(YVRMessage<AccountData> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetLoggedInAccountCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetLoggedInAccountCallback));
    }

    #endregion

    #region Achievement

    public static void GetAllAchievementDefinitions()
    {
        Achievement.GetAllDefinitions().OnComplete(GetAllAchievementDefinitionsCallback);
    }

    public static void GetAllAchievementDefinitionsCallback(YVRMessage<AchievementDefinitionList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetAllAchievementDefinitionsCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetAllAchievementDefinitionsCallback));
    }

    public static void GetAchievementDefinitionByName(string param)
    {
        Achievement.GetDefinitionByName(new[] {param}).OnComplete(GetAchievementDefinitionByNameCallback);
    }

    public static void GetAchievementDefinitionByNameCallback(YVRMessage<AchievementDefinitionList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetAchievementDefinitionByNameCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetAchievementDefinitionByNameCallback));
    }

    public static void GetAllAchievementProgress()
    {
        Achievement.GetAllProgress().OnComplete(GetAllAchievementProgressCallback);
    }

    public static void GetAllAchievementProgressCallback(YVRMessage<AchievementProgressList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetAllAchievementProgressCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetAllAchievementProgressCallback));
    }

    public static void GetAchievementProgressByName(string param)
    {
        s_AchievementNameCache = param;
        Achievement.GetProgressByName(new[] {param}).OnComplete(GetAchievementProgressByNameCallback);
    }

    public static void GetAchievementProgressByNameCallback(YVRMessage<AchievementProgressList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetAchievementProgressByNameCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetAchievementProgressByNameCallback));

        achievementProgress = msg.data.Count switch
        {
            0 => 0,
            _ => msg.data[0].countProgress
        };
    }

    public static void AchievementAddCount(string name, int addNum)
    {
        Achievement.AchievementAddCount(name, addNum).OnComplete(AchievementAddCountCallback);
    }

    public static void AchievementAddCountCallback(YVRMessage msg) { Debug.Log(nameof(AchievementAddCountCallback)); }

    public static void AchievementAddFields(string name, string field)
    {
        Achievement.AchievementAddFields(name, field).OnComplete(AchievementAddFieldsCallback);
    }

    public static void AchievementAddFieldsCallback(YVRMessage msg) { Debug.Log(nameof(AchievementAddFieldsCallback)); }

    public static void UnlockAchievement(string name)
    {
        Achievement.UnlockAchievement(name).OnComplete(UnlockAchievementCallback);
    }

    public static void UnlockAchievementCallback(YVRMessage msg) { Debug.Log(nameof(UnlockAchievementCallback)); }

    #endregion

    #region Friends

    public static void GetFriends() { Friends.GetFriends().OnComplete(GetFriendsCallback); }

    public static void GetFriendsCallback(YVRMessage<FriendsList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetFriendsCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetFriendsCallback));
    }

    public static void GetFriendInfo(int accountID)
    {
        Friends.GetFriendInformation(accountID).OnComplete(GetFriendInfoCallback);
    }

    public static void GetFriendInfoCallback(YVRMessage<FriendInfo> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetFriendInfoCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetFriendInfoCallback));
    }

    #endregion

    #region IAP

    public static void GetIAPProducts(string[] uniqueIds)
    {
        IAP.GetProductsBySKU(uniqueIds).OnComplete(GetIAPProductsCallback);
    }

    public static void GetIAPProductsCallback(YVRMessage<IAPProductList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetIAPProductsCallback: msg is error!");
            return;
        }

        Debug.Log(msg.data.ToString());

        Debug.Log(nameof(GetIAPProductsCallback));
    }

    public static void InitiatePurchase(string uniqueId, float price)
    {
        IAP.LaunchCheckoutFlow(uniqueId, price).OnComplete(InitiatePurchaseCallback);
    }

    public static void InitiatePurchaseCallback(YVRMessage<IAPPurchaseInfo> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("InitiatePurchaseCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(InitiatePurchaseCallback));
    }

    public static void GetPurchasedProducts() { IAP.GetViewerPurchases().OnComplete(GetPurchasedProductsCallback); }

    public static void GetPurchasedProductsCallback(YVRMessage<IAPPurchasedProductList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetPurchasedProductsCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetPurchasedProductsCallback));
    }

    public static void ConsumePurchasedProduct(string uniqueId)
    {
        IAP.ConsumePurchase(uniqueId).OnComplete(ConsumePurchasedProductCallback);
    }

    public static void ConsumePurchasedProductCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError("ConsumePurchasedProductCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(ConsumePurchasedProductCallback));
    }

    #endregion

    #region Leaderboard

    public static void GetLeaderboardInfoByPage(LeaderboardByPage data)
    {
        Leaderboard.GetLeaderboardInfoByPage(data).OnComplete(GetLeaderboardInfoCallback);
    }

    public static void GetLeaderboardInfoByRank(LeaderboardByRank data)
    {
        Leaderboard.GetLeaderboardInfoByRank(data).OnComplete(GetLeaderboardInfoCallback);
    }

    public static void GetLeaderboardInfoCallback(YVRMessage<LeaderboardInfo> msg)
    {
        if (msg.isError)
        {
            Debug.LogError("GetLeaderboardInfoCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(GetLeaderboardInfoCallback));
    }
    
    public static void LeaderboardWriteItem(LeaderboardEntry data)
    {
        Leaderboard.LeaderboardWriteItem(data).OnComplete(LeaderboardWriteItemCallback);
    }
    
    public static void LeaderboardWriteItemCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError("LeaderboardWriteItemCallback: msg is error!");
            return;
        }

        Debug.Log(nameof(LeaderboardWriteItemCallback));
    }

    #endregion
}