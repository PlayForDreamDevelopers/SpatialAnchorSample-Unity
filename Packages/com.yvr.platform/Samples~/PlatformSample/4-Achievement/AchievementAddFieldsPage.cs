using UnityEngine;
using YVR.Platform;

public class AchievementAddFieldsPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        AchievementAddFields();
    }

    private void AchievementAddFields()
    {
        string paramName = GetInputValueByType(InputOptionType.AchievementName);
        string field = GetInputValueByType(InputOptionType.AddField);

        Achievement.AchievementAddFields(paramName, field).OnComplete(AchievementAddFieldsCallback);
    }

    private void AchievementAddFieldsCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"AchievementAddFieldsPage.AchievementAddFieldsCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"AchievementAddFieldsPage.AchievementAddFieldsCallback: add field success -> {msg}");
            m_TextResult.text = "Add field success.\nPlease refresh progress page";
        }
    }
}
