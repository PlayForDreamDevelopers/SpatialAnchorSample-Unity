using System;
using System.Text;
using UnityEngine;
using YVR.Platform;

public class LeaderboardWriteItemPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        LeaderboardWriteItem();
    }

    private void LeaderboardWriteItem()
    {
        LeaderboardEntry rankInfo = new LeaderboardEntry();
        rankInfo.leaderboardApiName = GetInputValueByType(InputOptionType.LeaderboardName);
        rankInfo.score =  double.Parse(GetInputValueByType(InputOptionType.Score));
        rankInfo.extraData = string.IsNullOrEmpty(GetInputValueByType(InputOptionType.ExtraData))
            ? new sbyte[] { }
            : Array.ConvertAll(Encoding.Default.GetBytes(GetInputValueByType(InputOptionType.ExtraData)), q => Convert.ToSByte(q));
        rankInfo.extraDataLength = rankInfo.extraData.Length;
        rankInfo.forceUpdate
            = Enum.Parse<LeaderboardUpdatePolicy>(GetInputValueByType(InputOptionType.UpdatePolicy));

        Leaderboard.LeaderboardWriteItem(rankInfo).OnComplete(LeaderboardWriteItemCallback);
    }

    private void LeaderboardWriteItemCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"LeaderboardWriteItemPage.LeaderboardWriteItemCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"LeaderboardWriteItemPage.LeaderboardWriteItemCallback: write success -> {msg}");
            m_TextResult.text = "Write success.\nPlease refresh leaderboard page";
        }
    }
}
