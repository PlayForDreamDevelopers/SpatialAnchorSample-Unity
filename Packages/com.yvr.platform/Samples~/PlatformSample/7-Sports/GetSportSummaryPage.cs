using System;
using UnityEngine;
using YVR.Platform;

public class GetSportSummaryPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetSportSummary();
    }

    private void GetSportSummary()
    {
        DateTime beginTime = DateTime.Parse(GetInputValueByType(InputOptionType.BeginTimeFormat));
        DateTime endTime = DateTime.Parse(GetInputValueByType(InputOptionType.EndTimeFormat));

        Sport.GetSummary(beginTime, endTime).OnComplete(GetSportSummaryCallback);
    }

    private void GetSportSummaryCallback(YVRMessage<SportSummary> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetSportSummaryPage.GetSportSummaryCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetSportSummaryPage.GetSportSummaryCallback: summary -> {msg.data}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}
