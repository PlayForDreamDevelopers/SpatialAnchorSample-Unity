using UnityEngine;
using YVR.Platform;

public class GetFriendInformationPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetFriendInformation();
    }

    private void GetFriendInformation()
    {
        if (int.TryParse(GetInputValueByType(InputOptionType.FriendAccountID), out int accountID))
            Friends.GetFriendInformation(accountID).OnComplete(GetFriendInformationCallback);
        else
            Debug.LogError("GetFriendInformationPage.GetFriendInformation: account Id is invalid");
    }

    private void GetFriendInformationCallback(YVRMessage<FriendInfo> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetFriendInformationPage.GetFriendInformationCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetFriendInformationPage.GetFriendInformationCallback: friend information -> {msg.data}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}