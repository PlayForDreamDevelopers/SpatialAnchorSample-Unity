using UnityEngine;
using YVR.Platform;

public class GetProductsBySKUPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetProductsBySKU();
    }

    private void GetProductsBySKU()
    {
        string[] skus = GetInputValueByType(InputOptionType.ProductSKU).Split(';');

        IAP.GetProductsBySKU(skus.Length > 0 ? skus : null).OnComplete(GetProductsBySKUCallback);
    }

    private void GetProductsBySKUCallback(YVRMessage<IAPProductList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetProductsBySKUPage.GetProductsBySKUCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetProductsBySKUPage.GetProductsBySKUCallback: products -> {msg.data}");
            m_TextResult.text = msg.data.ToString();
        }
    }
}
