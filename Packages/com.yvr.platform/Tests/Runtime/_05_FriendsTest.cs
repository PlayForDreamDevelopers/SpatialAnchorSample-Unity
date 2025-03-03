using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class _05_FriendsTest
{
    private int m_FriendID = 585156;

    [UnityTest, Order(0)]
    public IEnumerator _00_Friend_GetFriends_LogCallbackName()
    {
        PlatformTester.GetFriends();

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetFriendsCallback));
    }
    
    [UnityTest, Order(1)]
    public IEnumerator _01_Friend_GetFriendInformation_LogCallbackName()
    {
        PlatformTester.GetFriendInfo(m_FriendID);

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetFriendInfoCallback));
    }
}