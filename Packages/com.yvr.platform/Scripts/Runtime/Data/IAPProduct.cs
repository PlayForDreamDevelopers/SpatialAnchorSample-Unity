using System.Collections.Generic;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of IAP product
    /// </summary>
    public class IAPProduct
    {
        /// <summary>
        /// App ID of which product belongs to
        /// </summary>
        public readonly long appID;

        /// <summary>
        /// Unique ID of product
        /// </summary>
        public readonly string sku;

        /// <summary>
        /// Name of product
        /// </summary>
        public readonly string name;

        /// <summary>
        /// Description of product
        /// </summary>
        public readonly string description;

        /// <summary>
        /// Icon url of product
        /// </summary>
        public readonly string icon;

        /// <summary>
        /// Add on type of product, 1:consumable, 2:non-consumable
        /// </summary>
        public readonly int addOnType;

        /// <summary>
        /// Price of product
        /// </summary>
        public readonly float price;

        public IAPProduct(AndroidJavaObject obj)
        {
            appID = YVRPlatform.YVRIAPGetAppIdOfProduct(obj);
            sku = YVRPlatform.YVRIAPGetUniqueIdOfProduct(obj);
            name = YVRPlatform.YVRIAPGetNameOfProduct(obj);
            description = YVRPlatform.YVRIAPGetDescriptionOfProduct(obj);
            icon = YVRPlatform.YVRIAPGetIconOfProduct(obj);
            addOnType = YVRPlatform.YVRIAPGetTypeOfProduct(obj);
            price = YVRPlatform.YVRIAPGetPriceOfProduct(obj);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"appID:[{appID}],\n\r");
            str.Append($"sku:[{sku}],\n\r");
            str.Append($"name:[{name}],\n\r");
            str.Append($"description:[{description}],\n\r");
            str.Append($"icon:[{icon}],\n\r");
            str.Append($"addOnType:[{addOnType}],\n\r");
            str.Append($"price:[{price}],\n\r");

            return str.ToString();
        }
    }

    /// <summary>
    /// The storage of IAP product data
    /// </summary>
    public class IAPProductList : DeserializableList<IAPProduct>
    {
        public IAPProductList(AndroidJavaObject obj)
        {
            int count = YVRPlatform.YVRIAPGetSizeOfProducts(obj);

            data = new List<IAPProduct>(count);

            for (int i = 0; i < count; i++)
                data.Add(new IAPProduct(YVRPlatform.YVRIAPGetElementOfProduct(obj, i)));
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