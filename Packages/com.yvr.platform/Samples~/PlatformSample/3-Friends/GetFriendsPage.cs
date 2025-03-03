using UnityEngine;
using YVR.Platform;

public class GetFriendsPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetFriends();
    }

    private void GetFriends() { Friends.GetFriends().OnComplete(GetFriendsCallback); }

    private void GetFriendsCallback(YVRMessage<FriendsList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetFriendsPage.GetFriendsCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetFriendsPage.GetFriendsCallback: friends -> {msg.data}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}