using UnityEngine;
using YVR.Platform;

public class GetSportUserInfoPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetUserInfo();
    }

    private void GetUserInfo()
    {
        Sport.GetUserInfo().OnComplete(GetUserInfoCallback);
    }

    private void GetUserInfoCallback(YVRMessage<SportUserInfo> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetUserInfoPage.GetUserInfoCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetUserInfoPage.GetUserInfoCallback: user info -> {msg.data}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}
