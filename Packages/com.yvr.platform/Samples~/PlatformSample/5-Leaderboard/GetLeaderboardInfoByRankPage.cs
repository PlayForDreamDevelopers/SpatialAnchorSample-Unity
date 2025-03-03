using System;
using UnityEngine;
using YVR.Platform;

public class GetLeaderboardInfoByRankPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetLeaderboardInfoByRank();
    }

    private void GetLeaderboardInfoByRank()
    {
        LeaderboardByRank leaderboardByRank = new LeaderboardByRank();
        leaderboardByRank.currentStart = long.Parse(GetInputValueByType(InputOptionType.CurrentStart));
        leaderboardByRank.dataDirection
            = Enum.Parse<LeaderboardDataDirection>(GetInputValueByType(InputOptionType.DataDirection));
        leaderboardByRank.size = long.Parse(GetInputValueByType(InputOptionType.PageSize));
        leaderboardByRank.leaderboardApiName = GetInputValueByType(InputOptionType.LeaderboardName);
        leaderboardByRank.pageType = Enum.Parse<LeaderboardPageType>(GetInputValueByType(InputOptionType.PageType));

        Leaderboard.GetLeaderboardInfoByRank(leaderboardByRank).OnComplete(GetLeaderboardInfoCallback);
    }

    private void GetLeaderboardInfoCallback(YVRMessage<LeaderboardInfo> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetLeaderboardInfoByRankPage.GetLeaderboardInfoCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetLeaderboardInfoByRankPage.GetLeaderboardInfoCallback: leaderboard content -> {msg}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}