using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using YVR.Platform;

public class _03_DeeplinkTest
{
    [UnityTest, Order(0)]
    public IEnumerator _03_Deeplink_InitializeWithAppID_ReturnsCorrectState()
    {
        bool isDeeplinkLaunched = Deeplink.IsDeeplinkLaunch();
        string roomID  = Deeplink.GetDeeplinkRoomId();
        string apiName = Deeplink.GetDeeplinkApiName();

        yield return null;
        
        Assert.That(!isDeeplinkLaunched);
        Assert.That(roomID == null);
        Assert.That(apiName == null);
    }
}
