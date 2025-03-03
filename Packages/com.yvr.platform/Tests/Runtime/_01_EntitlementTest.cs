using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class _01_EntitlementTest
{
    [UnityTest, Order(0)]
    public IEnumerator _00_Entitlement_InitializeWithAppID_ReturnsCorrectState()
    {
        PlatformTester.EntitlementCheck();

        yield return new WaitForSecondsRealtime(2f);
        
        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetEntitlementCallback));
    }
}
