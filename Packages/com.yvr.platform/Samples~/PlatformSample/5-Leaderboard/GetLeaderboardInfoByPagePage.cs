using System;
using UnityEngine;
using YVR.Platform;

public class GetLeaderboardInfoByPagePage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetLeaderboardInfoByPage();
    }

    private void GetLeaderboardInfoByPage()
    {
        LeaderboardByPage leaderboardByPage = new LeaderboardByPage();
        leaderboardByPage.current = long.Parse(GetInputValueByType(InputOptionType.CurrentPage));
        leaderboardByPage.size = long.Parse(GetInputValueByType(InputOptionType.PageSize));
        leaderboardByPage.leaderboardApiName = GetInputValueByType(InputOptionType.LeaderboardName);
        leaderboardByPage.pageType = Enum.Parse<LeaderboardPageType>(GetInputValueByType(InputOptionType.PageType));

        Leaderboard.GetLeaderboardInfoByPage(leaderboardByPage).OnComplete(GetLeaderboardInfoCallback);
    }

    private void GetLeaderboardInfoCallback(YVRMessage<LeaderboardInfo> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetLeaderboardInfoByPagePage.GetLeaderboardInfoCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetLeaderboardInfoByPagePage.GetLeaderboardInfoCallback: leaderboard content -> {msg}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}
