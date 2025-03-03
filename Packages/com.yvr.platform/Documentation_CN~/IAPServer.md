# 内购服务端 API

## 获取已购买商品列表
功能等同于客户端接口GetViewerPurchases()。完成购买流程后，你可以调用 GetViewerPurchases 接口来向用户展示已购买的商品列表，包括永久型商品和尚未履约的消耗型商品。

### 请求地址

访问链接：https://api.yvr.cn/vrmcsys/s2s/iap/getViewerPurchases

### 请求方式

- POST

### 请求参数

| **参数** | **类型** | **是否必填** | **说明** |
| ------- | -------- | ----------- | -------- |
| access_token | string | 是 | 应用 ID 和应用密钥。<br /> YVR|$app_id|$app_secret |
| user_id | string | 是 | 要查询的用户 ID。|

### 请求示例

```
{
    "accessToken":"YVR|4124748684|b795708b97c12f39315456c7169b2f0bcccd5d83",
    "userId":456892
}
```

### 响应参数

| **参数** | **类型** | **说明** |
| ------- | -------- | -------- |
| errcode | int | 错误码 |
| error message | string | 错误信息 |
| data |  |  |

#### Data

| **参数** | **说明** |
| ------- | -------- |
| tradeNo | 订单号 | 
| sku | 附加内容的唯一标识符 |
| name | 附加内容的名称 |
| type | 附加内容的类型: 0-持久型, 1-消耗型 |
| amount | 订单支付金额，单位为分 | 
| payType | 支付方式：0-免费，1-微信，2-支付宝, 3-Y币 4-paypal 5-支付宝签约并支付 6-支付宝免密支付 |
| scover | 附加内容正方形封面图URL |
| rcover | 附加内容长方形封面图URL |

#### Error

| **错误码** | **错误信息** | **说明** |
| --------- | ----------- | -------- |
| 0 | The request succeeded. | 正确 |
| 11001 | The required parameter(s) are missing. | 参数错误 |
| 10001 | The user ID is invalid. | 用户不存在 |
| 17100 | The Access_Token is invalid. | accessToken校验错误 |

### 响应示例

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

## 履约已购买商品

对于消耗型商品，你需要在自己的业务服务端实现履约逻辑。以游戏金币为例，若你添加的商品为“10 个金币”，用户购买该商品后，后台须向指定用户的帐号充值 10 金币。你可以调用 ConsumePurchase 接口来记录履约结果。履约后，GetViewerPurchases 接口将不再返回该商品。
在之前的订单完成之前，用户无法重新购买相同的消耗型。
永久型商品不能履约核销。

### 请求地址

访问链接：https://api.yvr.cn/vrmcsys/s2s/iap/consumePurchase

### 请求方式

- POST

### 请求参数

| **参数** | **类型** | **是否必填** | **说明** |
| ------- | -------- | ----------- | -------- |
| access_token | string | 是 | 应用 ID 和应用密钥 <br /> YVR|$app_id|$app_secret |
| sku | string | 是 | 要履约的商品的 SKU |
| user_id | string | 是 | 要查询的用户 ID |

### 请求示例

```
{
    "accessToken":"YVR|4124748684|b795708b97c12f39315456c7169b2f0bcccd5d83",
    "userId":456892,
    "sku":"test"
}
```

### 响应参数

| **参数** | **类型** | **说明** |
| ------- | -------- | -------- |
| errcode | int | 错误码 |
| error message | string | 错误信息 |
| data |  |  |
<br />

#### Data

| **参数** | **类型** | **说明** |
| ------- | -------- | -------- |
| consumed | int | 商品是否履约成功：0-未核销成功, 1-核销成功 |

#### Error

| **错误码** | **错误信息** | **说明** |
| --------- | ----------- | -------- |
| 0 | The request succeeded. | 正确 |
| 11001 | The required parameter(s) are missing. | 参数错误 |
| 10001 | The user ID is invalid. | 用户不存在 |
| 17100 | The Access_Token is invalid. | accessToken校验错误 |
| 23001	| The SKU is invalid. | sku不存在 |
| 23002	| The product was not purchased. | 商品未购买 |
| 23110	| The product cannot be consumed. | 产品不能核销（持久型） |

### 响应示例

```
{
    "data": {
        "consumed":1
    },
    "errCode": 0,
    "errMsg": "success"
}
```

## 用户是否拥有商品

验证某个用户是否已经拥有某件商品，“拥有”表示“已购买”且“当前尚未履约”，等同于viewer_purchases获取的商品。

### 请求参数


| **参数** | **类型** | **是否必填** | **说明** |
| ------- | -------- | ----------- | -------- |
| access_token | string | 是 | 应用 ID 和应用密钥。<br /> YVR|$app_id|$app_secret |
| sku | string | 是 | 要验证的商品的 SKU |
| user_id | string | 是 | 要查询的用户 ID |

### 请求示例

```
{
    "accessToken":"YVR|4124748684|b795708b97c12f39315456c7169b2f0bcccd5d83",
    "userId":456892,
    "sku":"test"
}
```

### 响应参数

| **参数** | **类型** | **说明** |
| ------- | -------- | -------- |
| errcode | int | 错误码 |
| error message | string | 错误信息 |
| data |  |  |
<br />

#### Data

| **参数** | **类型** | **说明** |
| ------- | -------- | -------- |
| verified | int | 判断用户是否拥有某件商品的结果：0-未拥有 1-拥有 |


#### Error

| **错误码** | **错误信息** | **说明** |
| --------- | ----------- | -------- |
| 0 | The request succeeded. | 正确 |
| 11001 | The required parameter(s) are missing. | 参数错误 |
| 10001 | The user ID is invalid. | 用户不存在 |
| 17100 | The Access_Token is invalid. | accessToken校验错误 |
| 23001	| The SKU is invalid. | sku不存在 |

### 响应示例

```
{
    "data": {
        "verified":1
    },
    "errCode": 0,
    "errMsg": "success"
}
```