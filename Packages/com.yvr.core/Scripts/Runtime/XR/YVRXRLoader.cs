using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using UnityEngine.XR;
using UnityEngine.XR.Management;
#if UNITY_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;
#endif
#if XR_HANDS
using UnityEngine.XR.Hands;
#endif

namespace YVR.Core.XR
{
    public class YVRXRLoader : XRLoaderHelper
    {
        [DllImport("yvrplugin")]
        private static extern void YVRSetXRUserDefinedSettings(ref YVRXRUserDefinedSettings userDefinedSettings);

        [DllImport("yvrplugin")]
        private static extern int YVRSetDevelopmentBuildMode(bool isDevelopmentBuild);

        private static List<XRDisplaySubsystemDescriptor> displaySubsystemDescriptors
            = new List<XRDisplaySubsystemDescriptor>();

        private static List<XRInputSubsystemDescriptor> inputSubsystemDescriptors
            = new List<XRInputSubsystemDescriptor>();
#if XR_HANDS
        private static List<XRHandSubsystemDescriptor> handSubsystemDescriptors
            = new List<XRHandSubsystemDescriptor>();
#endif

        public override bool Initialize()
        {
#if UNITY_INPUT_SYSTEM
            InputLayoutLoader.RegisterInputLayouts();
#endif

            YVRXRSettings settings = null;
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject<YVRXRSettings>("YVR.Core.XR.YVRXRSettings", out settings);
#elif UNITY_ANDROID
            settings = YVRXRSettings.xrSettings;
            YVREventTracking.instance.UploadSDKInfo();
#endif
            YVRXRUserDefinedSettings userDefinedSettings = new YVRXRUserDefinedSettings();

            userDefinedSettings.stereoRenderingMode = settings.GetStereoRenderingMode();
            userDefinedSettings.eyeRenderScale = settings.eyeResolutionScale;
            userDefinedSettings.use16BitDepthBuffer = settings.use16BitDepthBuffer;
            userDefinedSettings.useMonoscopic = settings.useMonoscopic;
            userDefinedSettings.useLinearColorSpace = QualitySettings.activeColorSpace == ColorSpace.Linear;
            userDefinedSettings.UseVRWidget = settings.useVRWidget;
            userDefinedSettings.useAppSW = settings.useAppSW;
            userDefinedSettings.optimizeBufferDiscards = settings.optimizeBufferDiscards;
            YVRSetXRUserDefinedSettings(ref userDefinedSettings);

#if DEVELOPMENT_BUILD
            YVRSetDevelopmentBuildMode(true);
#else
            YVRSetDevelopmentBuildMode(false);
#endif

            CreateSubsystem<XRDisplaySubsystemDescriptor, XRDisplaySubsystem>(displaySubsystemDescriptors, "Display");
            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(inputSubsystemDescriptors, "Tracking");
#if XR_HANDS
            CreateSubsystem<XRHandSubsystemDescriptor,XRHandSubsystem>(handSubsystemDescriptors, "YVR Hands");
#endif

            return true;
        }

        public override bool Start()
        {
            StartSubsystem<XRDisplaySubsystem>();
            StartSubsystem<XRInputSubsystem>();
#if XR_HANDS
            StartSubsystem<XRHandSubsystem>();
#endif

            return true;
        }

        public override bool Stop()
        {
            StopSubsystem<XRDisplaySubsystem>();
            StopSubsystem<XRInputSubsystem>();
#if XR_HANDS
            StopSubsystem<XRHandSubsystem>();
#endif

            return true;
        }

        public override bool Deinitialize()
        {
            DestroySubsystem<XRDisplaySubsystem>();
            DestroySubsystem<XRInputSubsystem>();
#if XR_HANDS
            DestroySubsystem<XRHandSubsystem>();
#endif

            return true;
        }
    }
}