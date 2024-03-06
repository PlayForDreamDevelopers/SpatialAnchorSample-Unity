using System.Data;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Internal;

namespace YVR.Core
{
    [ExcludeFromDocs]
    public abstract class YVRPlugin
    {
        private static YVRPlugin s_Instance;

        public static YVRPlugin Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    if (!Application.isEditor && Application.platform == RuntimePlatform.Android)
                        s_Instance = YVRPluginAndroid.Create();
                    else
                        s_Instance = YVRPluginWin.Create();
                }

                return s_Instance;
            }
        }

        // The Controller State Received from the native plugin
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ControllerState
        {
            public UInt32 Buttons; //4
            public UInt32 Touches; //8
            public float IndexTrigger; //12
            public Vector2 Thumbstick; //20
            public float BatteryPercentRemaining; //21
            public bool isCharging; // 22

            public void Clear()
            {
                Buttons = 0;
                Touches = 0;
                IndexTrigger = 0;
                Thumbstick.x = Thumbstick.y = 0;
                BatteryPercentRemaining = 0;
                isCharging = false;
            }
        }

        public virtual void SetTrackingSpace(YVRTrackingStateManager.TrackingSpace trackingSpace) { }
        public virtual void SetVSyncCount(YVRQualityManager.VSyncCount vSyncCount) { }
        public virtual void RecenterTracking() { }

        public virtual void SetControllerVibration(uint controllerMask, float frequency, float amplitude) { }

        public virtual void SetControllerVibration(uint controllerMask, float frequency, float amplitude,
                                                   float duration) { }

        public virtual float GetBatteryLevel() { return -1; }

        public virtual float GetBatteryTemperature() { return -1; }

        public virtual int GetBatteryStatus() { return -1; }

        public virtual float GetVolumeLevel() { return -1; }

        public virtual float GetGPUUtilLevel() { return 0; }

        public virtual float GetCPUUtilLevel() { return 0; }

        public virtual int GetCPULevel() { return 0; }

        public virtual int GetGPULevel() { return 0; }

        public virtual void SetPerformanceLevel(int cpuLevel, int gpuLevel) { }

        public virtual bool IsUserPresent() { return false; }

        public virtual void SetPassthrough(bool enable) { }

        public virtual bool IsFocusing() { return true; }

        public virtual bool IsVisible() { return true; }

        public virtual void SetEventCallback(Action<int> eventCallback) { }
        public virtual void GetEyeResolution(ref Vector2 resolution) { }
        public virtual void GetEyeFov(int eyeSide, ref YVRCameraRenderer.EyeFov eyeFov) { }

        public virtual float GetDisplayFrequency() { return 0.0f; }

        public virtual void SetDisplayFrequency(float value) { }

        public virtual float[] GetDisplayFrequenciesAvailable() { return new float[2] {30, 60}; }

        public virtual bool GetBoundaryConfigured() { return false; }

        public virtual void TestBoundaryNode(YVRBoundary.BoundaryNode node,
                                             ref YVRBoundary.BoundaryTestResult testResult) { }

        public virtual void TestBoundaryPoint(Vector3 targetPoint, ref YVRBoundary.BoundaryTestResult testResult) { }

        public virtual Vector3 GetBoundaryDimensions() { return Vector3.zero; }

        public virtual bool GetBoundaryVisible() { return false; }

        public virtual void SetBoundaryVisible(bool visible) { }

        public virtual void SetBoundaryDetectionEnable(bool enable) { }

        public virtual Vector3[] GetBoundaryGeometry() { return null; }

        public virtual void SetFoveation(int ffrLevel, int ffrDynamic) { }

        public virtual void SetAppSWEnable(bool enable) { }

        public virtual bool GetAppSWEnable() { return false; }

        public virtual void SetAppSWSwitch(bool isOn) { }

        public virtual bool GetAppSWSwitch() { return false; }

        public virtual void SetEyeBufferLayerSettings(bool enableSuperSample, bool expensiveSuperSample,
                                                      bool enableSharpen, bool expensiveSharpen) { }

        public virtual void GetPrimaryController(ref uint controllerMask) { }

        public virtual void SetPrimaryController(uint controllerMask) { }

        public virtual void GetHandJointLocations(HandType handType, ref HandJointLocations jointLocations) { }

        public virtual void GetCurrentInputDevice(ref ActiveInputDevice inputDevice) { }

        public virtual bool GetHandEnable() { return false; }

        public virtual int GetHandAutoActivateTime() { return 0; }

        public virtual void SetAPPHandEnable(bool enable) { }

        public virtual void SetAPPControllerEnable(bool enable) { }

        public virtual void SetAppSpacePosition(float x, float y, float z) { }

        public virtual void SetAppSpaceRotation(float x, float y, float z, float w) { }

        public virtual void CreateSpatialAnchor(Vector3 position, Quaternion rotation, ref UInt64 requestId)
        {
            requestId = 0;
        }

        public virtual void SaveSpatialAnchor(YVRSpatialAnchorSaveInfo saveInfo, ref UInt64 requestId) { }

        public virtual void QuerySpatialAnchor(YVRSpatialAnchorQueryInfo queryInfo, ref UInt64 requestId) { }

        public virtual void DestroySpatialAnchor(UInt64 space, YVRSpatialAnchorStorageLocation location,
                                                 ref UInt64 requestId) { }

        public virtual bool GetSpatialAnchorPose(UInt64 space, ref Vector3 position, ref Quaternion rotation,
                                                 ref YVRAnchorLocationFlags locationFlags)
        {
            return false;
        }

        public virtual void SetCreateSpatialAnchorCallback(Action<YVRSpatialAnchorResult, bool> callback) { }

        public virtual void SetQuerySpatialAnchorCallback(Action<YVRQuerySpatialAnchorResult, UInt64> callback) { }

        public virtual void SetEraseSpatialAnchorCallback(Action<YVRSpatialAnchorResult, bool> callback) { }

        public virtual void GetSpatialAnchorEnumerateSupported(UInt64 space,
                                                               ref YVRSpatialAnchorSupportedComponent components) { }

        public virtual bool SetSpatialAnchorComponentStatus(UInt64 space,
                                                            YVRSpatialAnchorComponentStatusSetInfo statusSetInfo,
                                                            ref UInt64 requestId)
        {
            return false;
        }

        public virtual void GetSpatialAnchorComponentStatus(UInt64 space, YVRSpatialAnchorComponentType componentType,
                                                            ref YVRSpatialAnchorComponentStatus status) { }

        public virtual void CreateSpatialAnchorUserHandle(UInt64 userId, ref UInt64 spaceUser) { }

        public virtual void ShareSpatialAnchor(YVRSpatialAnchorShareInfo shareInfo, ref UInt64 requestId) { }

        public virtual void GetSpatialAnchorUserId(UInt64 spaceUser, ref UInt64 userId) { }

        public virtual void SetSpatialAnchorShareCompleteCallback(Action<bool, UInt64> callback) { }

        public virtual void SetSpatialAnchorSaveCompleteCallback(
            Action<YVRSpatialAnchorSaveCompleteInfo, bool> callback) { }

        public virtual void SetSpatialAnchorSaveListCompleteCallback(Action<bool, UInt64> callback) { }

        public virtual void SetSpatialAnchorStatusCompleteCallback(
            Action<YVRSpatialAnchorSetStatusCompleteInfo, bool> callback) { }

        public virtual void SaveSpatialAnchorList(YVRSpatialAnchorListSaveInfo listSaveInfo, ref UInt64 requestId) { }

        public virtual void GetSpatialAnchorUUID(ulong anchorHandle, ref YVRSpatialAnchorUUID uuid) { }

        public virtual bool GetPlaneDetectionSupported() { return false; }

        public virtual void CreatePlaneDetection() { }

        public virtual void BeginPlaneDetection(YVRPlaneDetectorBeginInfo planeDetectorBeginInfo) { }

        public virtual YVRPlaneDetectorState GetPlaneDetectionState()
        {
            return YVRPlaneDetectorState.XR_PLANE_DETECTION_STATE_NONE_EXT;
        }

        public virtual IntPtr GetPlaneDetectionsInfo() { return IntPtr.Zero; }

        public virtual void EndPlaneDetection() { }

        public virtual void DeletePlaneIntptr() { }

        public virtual void CreateEyeTracker() { }

        public virtual void DestroyEyeTracker() { }

        public virtual void GetEyeGazes(ref EyeTrackingData.EyeGazesState eyeGazesState) { }

        public virtual int GetSpaceBoundingBox2D(ulong anchorHandle, ref YVRRect2D boundingBox2D) { return -1; }

        public virtual int GetSpaceBoundingBox3D(ulong anchorHandle, ref YVRRect3D boundingBox3D) { return -1; }

        public virtual int GetSpaceBoundary2D(ulong anchorHandle, ref YVRBoundary2D boundary2D) { return -1; }

        public virtual int GetSpaceSemanticLabels(ulong anchorHandle, ref YVRAnchorSemanticLabel anchorSemanticLabel)
        {
            return -1;
        }

        public virtual int GetSpaceRoomLayout(ulong anchorHandle, ref YVRRoomLayout roomLayout) { return -1; }

        public virtual int GetSpaceContainer(ulong anchorHandle, ref YVRSceneAnchorContainer sceneAnchorContainer)
        {
            return -1;
        }

        public virtual int RequestSceneCapture(ref YVRSceneCaptureRequest requestString, ref ulong requestId) { return -1; }
    
        public virtual void SetSceneCaptureCallback(Action<ulong, bool> callback) { }
    }
}