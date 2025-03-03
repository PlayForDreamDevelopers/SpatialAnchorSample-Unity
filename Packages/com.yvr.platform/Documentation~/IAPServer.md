# In-App Purchase Server API

## Retrieve Purchased Item List

The functionality is equivalent to the client API GetViewerPurchases(). After completing the purchase process, you can call the GetViewerPurchases API to show the user a list of purchased items, including non-consumable items and consumable items that have not yet been fulfilled.

### URL

URL: https://api.yvr.cn/vrmcsys/s2s/iap/getViewerPurchases

### Method

- POST

### Request Parameters

| **Parameter** | **Type** | **Required** | **Description** |
| ------------- | -------- | ------------ | --------------- |
| access_token | string | Yes | App ID and app secret <br /> YVR|$app_id|$app_secret |
| user_id | string | Yes | User ID to query |

### Request Example

```
{
    "accessToken":"YVR|4124748684|b795708b97c12f39315456c7169b2f0bcccd5d83",
    "userId":456892
}
```

### Response Parameters

| **Parameter** | **Type** | **Description** |
| ------------- | -------- | --------------- |
| errcode | int | Error code |
| error message | string | Error message |
| data |  |  |
<br />

#### Data

| **Parameter** | **Description** |
| ------------- | --------------- |
| tradeNo | Order ID. | 
| sku | Product ID |
| name | Product name. |
| type | Product type: 0-non-consumable, 1-consumable |
| amount | Order amount, in cents | 
| payType | Payment type: 0-Free, 1-Wechat，2-Alipay, 3-Y Coin 4-Paypal 5-Alipay sign and pay 6-Alipay w/o password |
| scover | Product square icon URL |
| rcover | Product rectangle icon URL |

#### Error

| **Error Code** | **Error Message** | **Description** |
| -------------- | ----------------- | --------------- |
| 0 | The request succeeded. | Correct |
| 11001 | The required parameter(s) are missing. | Incorrect parameter |
| 10001 | The user ID is invalid. | User does not exist |
| 17100 | The Access_Token is invalid. | accessToken is invalid |

### Response Example

```
{
    "data": {
        "purchases":[{
            "scover": "xxx",
            "rcover": "xxx",
            "tradeNo": "A106810000014402",
            "type": 2,
            "sku": "我们都有一个家，名字叫中国",
            "name":"{\"en_US\": \"Mango Phantown\", \"zh_CN\": \"芒果幻城3\"}",
            "amount":1200,
            "payType":1
        }]

    },
    "errCode": 0,
    "errMsg": "success"
}
```

## Consume Purchased Item

For consumable items, need to implement fulfilment logic in the app. For example, if you add a "10 coins" product, after the user purchases the item, the backend must recharge the specified user's account with 10 coins. You can call the ConsumePurchase API to record the fulfilment result. You can call ConsumePurchase API to record the result. After fulfillment, the GetViewerPurchases API will no longer return the item. The user cannot re-purchase the same consumable type until the previous order is completed. Non-consumable items cannot be consumed.


### URL

URL: https://api.yvr.cn/vrmcsys/s2s/iap/consumePurchase

### Method

- POST

### Request Parameters

| **Parameter** | **Type** | **Required** | **Description** |
| ------------- | -------- | ------------ | --------------- |
| access_token | string | Yes | App ID and app secret <br /> YVR|$app_id|$app_secret |
| sku | string | Yes | SKU of the item to consume |
| user_id | string | Yes | User ID to query |

### Request Example

```
{
    "accessToken":"YVR|4124748684|b795708b97c12f39315456c7169b2f0bcccd5d83",
    "userId":456892,
    "sku":"test"
}
```

### Response Parameters

| **Parameter** | **Type** | **Description** |
| ------------- | -------- | --------------- |
| errcode | int | Error code |
| error message | string | Error message |
| data |  |  |
<br />

#### Data 

| **Parameter** | **Description** |
| ------------- | --------------- |
| consumed | Whether product is successfully consumed: 0-Unsuccessful, 1-Successful |

#### Error

| **Error Code** | **Error Message** | **Description** |
| -------------- | ----------------- | --------------- |
| 0 | The request succeeded. | Correct |
| 11001 | The required parameter(s) are missing. | Incorrect parameter |
| 10001 | The user ID is invalid. | User does not exist |
| 17100 | The Access_Token is invalid. | accessToken is invalid |
| 23001	| The SKU is invalid. | sku does not exist |
| 23002	| The product was not purchased. | Product was not purchased |
| 23110	| The product cannot be consumed. | Product is non-consumable |

### Response Example

```
{
    "data": {
        "consumed":1
    },
    "errCode": 0,
    "errMsg": "success"
}
```

## Verify Product Purchase

Validates whether a user already owns an item, "owns" means "purchased" and "not currently performing", equivalent to the item retrieved by viewer_purchases. 


### Request Parameters

| **Parameter** | **Type** | **Required** | **Description** |
| ------------- | -------- | ------------ | --------------- |
| access_token | string | Yes | App ID and app secret <br /> YVR|$app_id|$app_secret |
| sku | string | Yes | SKU of the item to verify |
| user_id | string | Yes | User ID to query |


### Request Example

```
{
    "accessToken":"YVR|4124748684|b795708b97c12f39315456c7169b2f0bcccd5d83",
    "userId":456892,
    "sku":"test"
}
```

### Response Parameters

| **Parameter** | **Type** | **Description** |
| ------------- | -------- | --------------- |
| errcode | int | Error code |
| error message | string | Error message |
| data |  |  |
<br />

#### Data

| **Parameter** | **Description** |
| ------------- | --------------- |
| verified | int | The result of determining whether a user owns an item: 0-does not own 1 - own |

#### Error

| **Error Code** | **Error Message** | **Description** |
| -------------- | ----------------- | --------------- |
| 0 | The request succeeded. | Correct |
| 11001 | The required parameter(s) are missing. | Incorrect parameter |
| 10001 | The user ID is invalid. | User does not exist |
| 17100 | The Access_Token is invalid. | accessToken is invalid |
| 23001	| The SKU is invalid. | sku does not exist |


### Response Example

```
{
    "data": {
        "verified":1
    },
    "errCode": 0,
    "errMsg": "success"
}
```