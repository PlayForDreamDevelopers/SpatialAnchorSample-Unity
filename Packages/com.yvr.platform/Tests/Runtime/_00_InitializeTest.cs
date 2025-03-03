using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using YVR.Platform;

public class _00_InitializeTest
{
    private long m_AppID = 4118951786;
    
    [UnityTest, Order(0)]
    public IEnumerator _00_Init_InitializeWithAppID_ReturnsCorrectState()
    {
        YVRPlatform.Initialize(m_AppID);

        yield return null;
        
        Assert.That(YVRPlatform.isInitialized);
    }
}
