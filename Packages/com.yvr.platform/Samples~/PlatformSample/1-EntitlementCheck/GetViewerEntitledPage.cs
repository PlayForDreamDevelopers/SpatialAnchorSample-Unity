using UnityEngine;
using YVR.Platform;

public class GetViewerEntitledPage : OperationPage
{
    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        GetViewerEntitled();
    }

    private void GetViewerEntitled() { PlatformCore.GetViewerEntitled().OnComplete(GetViewerEntitledCallback); }

    private void GetViewerEntitledCallback(YVRMessage<Entitlement> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"GetViewerEntitledPage.GetViewerEntitledCallback: error -> {msg.error}");
            m_TextResult.text = msg.error.ToString();
        }
        else
        {
            Debug.Log($"GetViewerEntitledPage.GetViewerEntitledCallback: isEntitled -> {msg.data.isEntitled}");
            m_TextResult.text = msg.data.ToString();

            if (!msg.data.isEntitled && GetInputValueByType(InputOptionType.QuitAppWhenEntitlementCheckFail).Equals("True"))
                Application.Quit();
        }
    }
}