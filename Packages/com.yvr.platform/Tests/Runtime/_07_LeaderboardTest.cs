using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YVR.Platform;

public class _07_LeaderboardTest
{
    private string m_LeaderboardName = "Leaderboard01";

    [UnityTest, Order(0)]
    public IEnumerator _00_Leaderboard_GetLeaderboardInfoByPage_LogCallbackName()
    {
        LeaderboardByPage leaderboardByPage = new LeaderboardByPage();
        leaderboardByPage.current = 1;
        leaderboardByPage.size = 10;
        leaderboardByPage.leaderboardApiName = m_LeaderboardName;
        leaderboardByPage.pageType = LeaderboardPageType.Friends;

        PlatformTester.GetLeaderboardInfoByPage(leaderboardByPage);

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetLeaderboardInfoCallback));
    }

    [UnityTest, Order(1)]
    public IEnumerator _01_Leaderboard_GetLeaderboardInfoByRank_LogCallbackName()
    {
        LeaderboardByRank leaderboardByRank = new LeaderboardByRank();
        leaderboardByRank.currentStart = 0;
        leaderboardByRank.dataDirection = LeaderboardDataDirection.Forward;
        leaderboardByRank.size = 3;
        leaderboardByRank.leaderboardApiName = m_LeaderboardName;
        leaderboardByRank.pageType = LeaderboardPageType.Friends;

        PlatformTester.GetLeaderboardInfoByRank(leaderboardByRank);

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetLeaderboardInfoCallback));
    }
    
    [UnityTest, Order(2)]
    public IEnumerator _02_Leaderboard_LeaderboardWriteItem_LogCallbackName()
    {
        LeaderboardEntry rankInfo = new LeaderboardEntry();
        rankInfo.leaderboardApiName = m_LeaderboardName;
        rankInfo.score = 100;
        rankInfo.extraData = new sbyte[] { };
        rankInfo.extraDataLength = rankInfo.extraData.Length;
        rankInfo.forceUpdate = LeaderboardUpdatePolicy.AlwaysUpdate;

        PlatformTester.LeaderboardWriteItem(rankInfo);

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.LeaderboardWriteItemCallback));
    }
}