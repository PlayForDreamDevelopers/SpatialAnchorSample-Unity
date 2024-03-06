using UnityEngine;
using UnityEngine.Scripting;

#if XR_HANDS
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.ProviderImplementation;

namespace YVR.Core
{
    [Preserve]
    public class YVRHandSubsystem : XRHandSubsystem
    {
        XRHandProviderUtility.SubsystemUpdater m_Updater;

        // This method registers the subsystem descriptor with the SubsystemManager
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            var handsSubsystemCinfo = new XRHandSubsystemDescriptor.Cinfo
            {
                id = "YVR Hands",
                providerType = typeof(YVRHandProvider),
                subsystemTypeOverride = typeof(YVRHandSubsystem)
            };
            XRHandSubsystemDescriptor.Register(handsSubsystemCinfo);
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Updater = new XRHandProviderUtility.SubsystemUpdater(this);
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_Updater.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();
            m_Updater.Stop();
        }

        protected override void OnDestroy()
        {
            m_Updater.Destroy();
            m_Updater = null;
            base.OnDestroy();
        }
    }
}

#endif