using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class _06_IAPTest
{
    private string m_IAPUniqueID = "IAP01";
    private float m_IAPPrice = 0f;
    
    [UnityTest, Order(0)]
    public IEnumerator _00_IAP_GetIAPProducts_LogCallbackName()
    {
        PlatformTester.GetIAPProducts(new []{m_IAPUniqueID});

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetIAPProductsCallback));
    }
    
    [UnityTest, Order(1)]
    public IEnumerator _01_IAP_InitiatePurchase_LogCallbackName()
    {
        PlatformTester.InitiatePurchase(m_IAPUniqueID, m_IAPPrice);

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.InitiatePurchaseCallback));
    }
    
    [UnityTest, Order(2)]
    public IEnumerator _02_IAP_GetPurchasedProducts_LogCallbackName()
    {
        PlatformTester.GetPurchasedProducts();

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetPurchasedProductsCallback));
    }
    
    [UnityTest, Order(3)]
    public IEnumerator _03_IAP_ConsumePurchasedProduct_LogCallbackName()
    {
        PlatformTester.ConsumePurchasedProduct(m_IAPUniqueID);

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.ConsumePurchasedProductCallback));
    }
}
