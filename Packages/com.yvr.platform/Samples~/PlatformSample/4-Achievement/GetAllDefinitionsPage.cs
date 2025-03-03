using UnityEngine;
using YVR.Platform;

public class GetAllDefinitionsPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetAllDefinitions();
    }

    private void GetAllDefinitions()
    {
        Achievement.GetAllDefinitions().OnComplete(GetAllDefinitionsCallback);
    }

    private void GetAllDefinitionsCallback(YVRMessage<AchievementDefinitionList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetAllDefinitionsPage.GetAllDefinitionsCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetAllDefinitionsPage.GetAllDefinitionsCallback: definitions -> {msg}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}
