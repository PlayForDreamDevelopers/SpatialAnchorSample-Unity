using UnityEngine;
using YVR.Platform;

public class InitializePage : OperationPage
{
    [SerializeField] private bool m_AutoHideWhenInitializeSuccess = true;

    protected override void Start()
    {
        base.Start();

        if (YVRPlatform.isInitialized && m_AutoHideWhenInitializeSuccess)
            gameObject.SetActive(false);

        string appIdCache = PlayerPrefs.GetString(CommonDefine.appIDCache, string.Empty);

        GetInputOption(InputOptionType.AppID)?.SetDefaultData(appIdCache);
    }

    protected override void OnClickExecute()
    {
        base.OnClickExecute();

        if (long.TryParse(GetInputValueByType(InputOptionType.AppID), out long appId))
            YVRPlatform.Initialize(appId);
        else
        {
            Debug.LogError("InitializePage.OnClickExecute: appId is invalid");
            return;
        }

        bool result = YVRPlatform.isInitialized;

        m_TextResult.text = $"InitializePage.OnClickExecute: result -> {result}";

        if (!result) return;

        PlayerPrefs.SetString(CommonDefine.appIDCache, appId.ToString());
        PlayerPrefs.Save();

        gameObject.SetActive(!m_AutoHideWhenInitializeSuccess);
    }
}