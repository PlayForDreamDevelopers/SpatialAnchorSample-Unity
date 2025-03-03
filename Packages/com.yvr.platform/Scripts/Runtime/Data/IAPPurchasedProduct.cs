using System.Collections.Generic;
using UnityEngine;

namespace YVR.Platform
{
    public class IAPPurchasedProduct
    {
        /// <summary>
        /// ID of purchased product
        /// </summary>
        public readonly string id;

        /// <summary>
        /// sku of purchased product
        /// </summary>
        public readonly string sku;

        /// <summary>
        /// Name of purchased product
        /// </summary>
        public readonly string name;

        /// <summary>
        /// Icon url of purchased product
        /// </summary>
        public readonly string icon;

        /// <summary>
        /// Add on type of purchased product, 1:consumable, 2:non-consumable
        /// </summary>
        public readonly int addOnType;

        public IAPPurchasedProduct(AndroidJavaObject obj)
        {
            id = YVRPlatform.YVRIAPGetOrderIdOfPurchasedProduct(obj);
            sku = YVRPlatform.YVRIAPGetUniqueIdOfPurchasedProduct(obj);
            name = YVRPlatform.YVRIAPGetNameOfPurchasedProduct(obj);
            icon = YVRPlatform.YVRIAPGetIconOfPurchasedProduct(obj);
            addOnType = YVRPlatform.YVRIAPGetTypeOfPurchasedProduct(obj);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"id:[{id}],\n\r");
            str.Append($"sku:[{sku}],\n\r");
            str.Append($"name:[{name}],\n\r");
            str.Append($"icon:[{icon}],\n\r");
            str.Append($"addOnType:[{addOnType}],\n\r");

            return str.ToString();
        }
    }

    /// <summary>
    /// The storage of IAP purchased product data
    /// </summary>
    public class IAPPurchasedProductList : DeserializableList<IAPPurchasedProduct>
    {
        public IAPPurchasedProductList(AndroidJavaObject obj)
        {
            int count = YVRPlatform.YVRIAPGetSizeOfPurchasedProducts(obj);

            data = new List<IAPPurchasedProduct>(count);

            for (int i = 0; i < count; i++)
                data.Add(new IAPPurchasedProduct(YVRPlatform.YVRIAPGetElementOfPurchasedProduct(obj, i)));
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            foreach (var item in data)
                str.Append(item + "\n\r");

            return str.ToString();
        }
    }
}