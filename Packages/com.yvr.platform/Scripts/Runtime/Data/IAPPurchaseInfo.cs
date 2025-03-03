using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of IAP purchase info
    /// </summary>
    public class IAPPurchaseInfo
    {
        public readonly IAPPurchasedProduct product;

        public IAPPurchaseInfo(AndroidJavaObject obj)
        {
            AndroidJavaObject productData = YVRPlatform.YVRIAPGetPurchaseInfo(obj);

            if (productData != null)
                product = new IAPPurchasedProduct(productData);
        }

        public override string ToString() { return product.ToString(); }
    }
}