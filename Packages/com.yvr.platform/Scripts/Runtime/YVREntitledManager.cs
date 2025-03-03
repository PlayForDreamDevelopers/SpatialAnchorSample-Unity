using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YVR.Platform;

namespace YVR.Platform
{
    public class YVREntitledManager : MonoBehaviour
    {
        public long appId = -1;
        public UnityEvent OnEntitleSuccess;
        public UnityEvent OnEntitleFail;

        private void Start()
        {
            if (!YVRPlatform.isInitialized)
            {
                YVRPlatform.Initialize(appId);
            }
            PlatformCore.GetViewerEntitled().OnComplete(GetViewerEntitledCallback);
        }

        private void GetViewerEntitledCallback(YVR.Platform.YVRMessage<YVR.Platform.Entitlement> msg)
        {
            if (msg.isError || !msg.data.isEntitled)
            {
                OnEntitleSuccess.Invoke();
            }
            else
            {
                OnEntitleFail.Invoke();
            }
        }
    }
}