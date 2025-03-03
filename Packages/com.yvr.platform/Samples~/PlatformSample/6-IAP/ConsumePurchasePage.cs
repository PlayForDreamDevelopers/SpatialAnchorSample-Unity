using UnityEngine;
using YVR.Platform;

public class ConsumePurchasePage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        ConsumePurchased();
    }

    private void ConsumePurchased()
    {
        string sku = GetInputValueByType(InputOptionType.ProductSKU);

        IAP.ConsumePurchase(sku).OnComplete(ConsumePurchasedCallback);
    }

    private void ConsumePurchasedCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"ConsumePurchasePage.ConsumePurchasedCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"ConsumePurchasePage.ConsumePurchasedCallback: products -> {msg}");
            m_TextResult.text = "Consume success.\nPlease refresh in GetViewerPurchasesPage";
        }
    }
}
