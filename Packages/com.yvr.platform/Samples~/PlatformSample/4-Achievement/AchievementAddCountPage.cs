using UnityEngine;
using YVR.Platform;

public class AchievementAddCountPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        AchievementAddCount();
    }

    private void AchievementAddCount()
    {
        string paramName = GetInputValueByType(InputOptionType.AchievementName);
        if (int.TryParse(GetInputValueByType(InputOptionType.AddCount), out int count))
            Achievement.AchievementAddCount(paramName, count).OnComplete(AchievementAddCountCallback);
        else
            Debug.LogError("AchievementAddCountPage.AchievementAddCount: count is invalid");
    }

    private void AchievementAddCountCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"AchievementAddCountPage.AchievementAddCountCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"AchievementAddCountPage.AchievementAddCountCallback: add count success -> {msg}");
            m_TextResult.text = "Add count success.\nPlease refresh progress page";
        }
    }
}
