using UnityEngine;
using YVR.Platform;

public class LaunchCheckoutFlowPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        LaunchCheckoutFlow();
    }

    private void LaunchCheckoutFlow()
    {
        string sku = GetInputValueByType(InputOptionType.ProductSKU);
        if (float.TryParse(GetInputValueByType(InputOptionType.AmountOfPayment), out float price))
            IAP.LaunchCheckoutFlow(sku, price).OnComplete(LaunchCheckoutFlowCallback);
        else
            Debug.LogError("LaunchCheckoutFlowPage.LaunchCheckoutFlow: price is invalid");
    }

    private void LaunchCheckoutFlowCallback(YVRMessage<IAPPurchaseInfo> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"LaunchCheckoutFlowPage.LaunchCheckoutFlowCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"LaunchCheckoutFlowPage.LaunchCheckoutFlowCallback: purchase info -> {msg.data}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}