using System.Collections.Generic;

public static class CommonDefine
{
    public const string appIDCache = "PrefKey_appIDCache";
    public static string[] typeDefine = {"Simple", "Count", "Bitfield"};

    public static List<InputOptionType> inputFieldTypesMap = new List<InputOptionType>()
    {
        InputOptionType.AppID,
        InputOptionType.FriendAccountID,
        InputOptionType.ProductSKU,
        InputOptionType.AmountOfPayment,
        InputOptionType.AchievementName,
        InputOptionType.AddCount,
        InputOptionType.AddField,
        InputOptionType.CurrentPage,
        InputOptionType.PageSize,
        InputOptionType.LeaderboardName,
        InputOptionType.CurrentStart,
        InputOptionType.Score,
        InputOptionType.ExtraData,
        InputOptionType.BeginTimeFormat,
        InputOptionType.EndTimeFormat,
    };

    public static List<InputOptionType> toggleTypesMap = new List<InputOptionType>()
    {
        InputOptionType.QuitAppWhenEntitlementCheckFail,
    };

    public static List<InputOptionType> dropdownTypesMap = new List<InputOptionType>()
    {
        InputOptionType.PageType,
        InputOptionType.DataDirection,
        InputOptionType.UpdatePolicy,
    };
}

public enum InputOptionType
{
    AppID = 0,
    QuitAppWhenEntitlementCheckFail = 1,
    FriendAccountID = 2,
    ProductSKU = 3,
    AmountOfPayment = 4,
    AchievementName = 5,
    AddCount = 6,
    AddField = 7,
    CurrentPage = 8,
    PageSize = 9,
    LeaderboardName = 10,
    PageType = 11,
    CurrentStart = 12,
    DataDirection = 13,
    Score = 14,
    ExtraData = 15,
    UpdatePolicy = 16,
    BeginTimeFormat = 17,
    EndTimeFormat = 19,
}

public enum OperationType
{
    Initialize = 0,
    GetViewerEntitled = 1,
    GetLoggedInUser = 2,
    GetFriends = 3,
    GetFriendInformation = 4,
    GetProductsBySKU = 5,
    LaunchCheckoutFlow = 6,
    GetViewerPurchases = 7,
    ConsumePurchase = 8,
    GetAllDefinitions = 9,
    GetAllProgress = 10,
    GetDefinitionByName = 11,
    GetProgressByName = 12,
    UnlockAchievement = 13,
    AchievementAddCount = 14,
    AchievementAddFields = 15,
    GetLeaderboardInfoByPage = 16,
    GetLeaderboardInfoByRank = 17,
    LeaderboardWriteItem = 18,
    GetSportUserInfo = 19,
    GetSportSummary = 20,
    GetSportDailySummary = 21,
}