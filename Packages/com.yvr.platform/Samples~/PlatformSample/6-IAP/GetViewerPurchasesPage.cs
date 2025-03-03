using UnityEngine;
using YVR.Platform;

public class GetViewerPurchasesPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetViewerPurchases();
    }

    private void GetViewerPurchases() { IAP.GetViewerPurchases().OnComplete(GetViewerPurchasesCallback); }

    private void GetViewerPurchasesCallback(YVRMessage<IAPPurchasedProductList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetViewerPurchasesPage.GetViewerPurchasesCallback: error -> {msg.error}");
            return;
        }

        Debug.Log($"GetViewerPurchasesPage.GetViewerPurchasesCallback: purchased products -> {msg.data}");

        m_TextResult.text = msg.data.ToString();
    }
}
