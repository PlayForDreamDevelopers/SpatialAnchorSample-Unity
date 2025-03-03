using UnityEngine;
using YVR.Platform;

public class GetDefinitionByNamePage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetDefinitionByName();
    }

    private void GetDefinitionByName()
    {
        string[] names = GetInputValueByType(InputOptionType.AchievementName).Split(';');

        Achievement.GetDefinitionByName(names).OnComplete(GetDefinitionByNameCallback);
    }

    private void GetDefinitionByNameCallback(YVRMessage<AchievementDefinitionList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetDefinitionByNamePage.GetDefinitionByNameCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetDefinitionByNamePage.GetDefinitionByNameCallback: definitions -> {msg}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}
