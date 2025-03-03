using UnityEngine;
using YVR.Platform;

public class UnlockAchievementPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        UnlockAchievement();
    }

    private void UnlockAchievement()
    {
        string paramName = GetInputValueByType(InputOptionType.AchievementName);

        Achievement.UnlockAchievement(paramName).OnComplete(UnlockAchievementCallback);
    }

    private void UnlockAchievementCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"UnlockAchievementPage.UnlockAchievementCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"UnlockAchievementPage.UnlockAchievementCallback: unlock success -> {msg}");
            m_TextResult.text = "Unlock success.\nPlease refresh progress page";
        }
    }
}
