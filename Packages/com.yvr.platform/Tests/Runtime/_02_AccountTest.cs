using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class _02_AccountTest
{
    [UnityTest, Order(0)]
    public IEnumerator _00_Account_GetAccountInfoLoggedIn_LogCallbackName()
    {
        PlatformTester.LoggedInAccountCheck();

        yield return new WaitForSecondsRealtime(2f);
        
        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetLoggedInAccountCallback));
    }
}
