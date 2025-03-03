using UnityEngine;
using YVR.Platform;

public class GetLoggedInUserPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetLoggedInUser();
    }

    private void GetLoggedInUser() { Account.GetLoggedInUser().OnComplete(GetLoggedInUserCallback); }

    private void GetLoggedInUserCallback(YVRMessage<AccountData> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetLoggedInUserPage.GetLoggedInUserCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetLoggedInUserPage.GetLoggedInUserCallback: accountData -> {msg.data}");
            if (m_TextResult != null) m_TextResult.text = msg.data.ToString();
        }
    }
}