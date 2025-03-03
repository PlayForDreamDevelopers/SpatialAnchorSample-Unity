using System;
using UnityEngine;
using YVR.Platform;

public class GetSportDailySummaryPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetSportDailySummary();
    }

    private void GetSportDailySummary()
    {
        DateTime beginTime = DateTime.Parse(GetInputValueByType(InputOptionType.BeginTimeFormat));
        DateTime endTime = DateTime.Parse(GetInputValueByType(InputOptionType.EndTimeFormat));

        Sport.GetDailySummary(beginTime, endTime).OnComplete(GetSportDailySummaryCallback);
    }

    private void GetSportDailySummaryCallback(YVRMessage<SportDailySummaryList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetSportDailySummaryPage.GetSportDailySummaryCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetSportDailySummaryPage.GetSportDailySummaryCallback: summary -> {msg.data}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}
