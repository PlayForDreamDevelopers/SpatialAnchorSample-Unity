namespace YVR.Platform
{
    public static class IAP
    {
        /// <summary>
        ///  Get products by Sku
        /// </summary>
        /// <param name="skus">Unique ID of product</param>
        /// <returns></returns>
        public static YVRRequest<IAPProductList> GetProductsBySKU(string[] skus)
        {
            return YVRPlatform.isInitialized
                    ? new YVRRequest<IAPProductList>(YVRPlatform.YVRIAPGetProducts(skus))
                    : null;
        }

        /// <summary>
        /// Launch checkout flow
        /// </summary>
        /// <param name="sku">Unique ID of product</param>
        /// <param name="price">Amount of payment</param>
        /// <returns></returns>
        public static YVRRequest<IAPPurchaseInfo> LaunchCheckoutFlow(string sku, float price)
        {
            return YVRPlatform.isInitialized
                ? new YVRRequest<IAPPurchaseInfo>(YVRPlatform.YVRIAPInitiatePurchase(sku, price))
                : null;
        }

        /// <summary>
        /// Get viewer purchases
        /// </summary>
        /// <returns></returns>
        public static YVRRequest<IAPPurchasedProductList> GetViewerPurchases()
        {
            return YVRPlatform.isInitialized
                ? new YVRRequest<IAPPurchasedProductList>(YVRPlatform.YVRIAPGetPurchasedProducts())
                : null;
        }

        /// <summary>
        /// Consume purchase
        /// </summary>
        /// <param name="sku">Unique ID of product</param>
        /// <returns></returns>
        public static YVRRequest ConsumePurchase(string sku)
        {
            return YVRPlatform.isInitialized
                ? new YVRRequest(YVRPlatform.YVRIAPConsumePurchasedProduct(sku))
                : null;
        }
    }
}