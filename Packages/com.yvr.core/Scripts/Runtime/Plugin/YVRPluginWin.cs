using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Internal;
using Unity.Profiling;

namespace YVR.Core
{
    [ExcludeFromDocs]
    public class YVRPluginWin : YVRPlugin
    {
        private const string k_Plugin = "yvrPlugin";

        public static YVRPluginWin Create()
        {
            return new YVRPluginWin();
        }

        public override void SetVSyncCount(YVRQualityManager.VSyncCount vSyncCount)
        {
            QualitySettings.vSyncCount = (int) vSyncCount;
        }

        public override void GetEyeResolution(ref Vector2 resolution)
        {
            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }

        public override void GetEyeFov(int eyeSide, ref YVRCameraRenderer.EyeFov eyeFov)
        {
            eyeFov.UpFov = eyeFov.DownFov = eyeFov.LeftFov = eyeFov.RightFov = 45;
        }

        public override void GetCurrentInputDevice(ref ActiveInputDevice inputDevice)
        {
            YVRGetCurrentInputDevice(ref inputDevice);
        }

        private GCHandle m_LJointLocationsHandle;
        private GCHandle m_RJointLocationsHandle;
        private GCHandle m_LJointVelocitiesHandle;
        private GCHandle m_RJointVelocitiesHandle;

        public override void GetHandJointLocations(HandType handType, ref HandJointLocations jointLocations)
        {
            if (jointLocations.jointLocations == null || jointLocations.jointVelocities == null)
            {
                jointLocations.jointLocations = new HandJointLocation[(int) HandJoint.JointMax];
                jointLocations.jointVelocities = new HandJointVelocity[(int) HandJoint.JointMax];
            }

            if (handType == HandType.HandLeft)
            {
                m_LJointLocationsHandle = GCHandle.Alloc(jointLocations.jointLocations, GCHandleType.Pinned);
                m_LJointVelocitiesHandle = GCHandle.Alloc(jointLocations.jointVelocities, GCHandleType.Pinned);
            }
            else if (handType == HandType.HandRight)
            {
                m_RJointLocationsHandle = GCHandle.Alloc(jointLocations.jointLocations, GCHandleType.Pinned);
                m_RJointVelocitiesHandle = GCHandle.Alloc(jointLocations.jointVelocities, GCHandleType.Pinned);
            }

            var handData = new HandData();
            YVRGetHandHandJointLocations(handType, ref handData,
                                         (handType == HandType.HandLeft
                                             ? m_LJointLocationsHandle
                                             : m_RJointLocationsHandle)
                                        .AddrOfPinnedObject(),
                                         (handType == HandType.HandLeft
                                             ? m_LJointVelocitiesHandle
                                             : m_RJointVelocitiesHandle)
                                        .AddrOfPinnedObject());
            jointLocations.aimState = handData.aimState;
            jointLocations.jointCount = handData.jointCount;
            jointLocations.handScale = handData.handScale;
            jointLocations.isActive = handData.isActive;
            for (int i = 0; i < jointLocations.jointLocations.Length; i++)
            {
                jointLocations.jointLocations[i].pose.ToJointPosef(handType);
            }

            if (handType == HandType.HandLeft)
            {
                m_LJointLocationsHandle.Free();
                m_LJointVelocitiesHandle.Free();
            }
            else if (handType == HandType.HandRight)
            {
                m_RJointLocationsHandle.Free();
                m_RJointVelocitiesHandle.Free();
            }
        }

        #region Dll

        [DllImport(k_Plugin)]
        private static extern void YVRGetHandHandJointLocations(HandType handType, ref HandData handData,
                                                                IntPtr handJointLocationsPtr,
                                                                IntPtr jointVelocitiesHandle);

        [DllImport(k_Plugin)]
        private static extern void YVRGetCurrentInputDevice(ref ActiveInputDevice inputDevice);

        [DllImport(k_Plugin)]
        private static extern void DebugEnable(bool enable);

        #endregion
    }
}