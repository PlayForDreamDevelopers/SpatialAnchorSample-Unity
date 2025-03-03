using UnityEngine;
using YVR.Platform;

public class GetAllProgressPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetAllProgress();
    }

    private void GetAllProgress() { Achievement.GetAllProgress().OnComplete(GetAllProgressCallback); }

    private void GetAllProgressCallback(YVRMessage<AchievementProgressList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetAllProgressPage.GetAllProgressCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetAllProgressPage.GetAllProgressCallback: progress -> {msg}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}