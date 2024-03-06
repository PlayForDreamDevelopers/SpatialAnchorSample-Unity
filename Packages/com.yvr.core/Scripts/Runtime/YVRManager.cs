using System.Collections.Generic;
using UnityEngine;

namespace YVR.Core
{
    /// <summary>
    /// The general manager class of whole sdk which holds other managers and controls the lifecycle of the vr mode
    /// </summary>
    public class YVRManager : MonoBehaviorSingleton<YVRManager>
    {
        public enum HandTrackingSupport
        {
            ControllersOnly = 0,
            ControllersAndHands = 1,
            HandsOnly = 2
        }

        [SerializeField] private bool m_UseUnityRig = true;

        /// <summary>
        /// The instance of class @YVR.Core.YVRControllerRig
        /// </summary>
        public YVRControllerRig controllerRig { get; private set; }

        /// <summary>
        /// The instance of class @YVR.Core.YVRCameraRig
        /// </summary>
        public YVRCameraRig cameraRig { get; private set; }

        /// <summary>
        /// The instance of class @YVR.Core.YVRCameraRenderer
        /// </summary>
        public YVRCameraRenderer cameraRenderer = new YVRCameraRenderer();

        /// <summary>
        /// The instance of class @YVR.Core.YVRBoundary
        /// </summary>
        public YVRBoundary boundary = new YVRBoundary();

        /// <summary>
        /// The instance of class @YVR.Core.YVRQualityManager
        /// </summary>
        public YVRQualityManager qualityManager = new YVRQualityManager();

        /// <summary>
        /// The instance of class @YVR.Core.YVRTrackingStateManager
        /// </summary>
        public YVRTrackingStateManager trackingManager = new YVRTrackingStateManager();

        /// <summary>
        /// The instance of class @YVR.Core.YVRHMDManager
        /// </summary>
        public YVRHMDManager hmdManager = new YVRHMDManager();

        /// <summary>
        /// The instance of class @YVR.Core.YVREventsManager
        /// </summary>
        public YVREventsManager eventsManager = new YVREventsManager();

        /// <summary>
        /// The instance of class @YVR.Core.YVRHandManager
        /// </summary>
        public YVRHandManager handManager { get; set; }

        /// <summary>
        /// The position offset of the center eye, only works in editor mode
        /// </summary>
        public Vector3 headPoseRelativeOffsetTranslation { get; set; }

        /// <summary>
        /// The rotation offset of the center eye (in euler angle), only works in editor mode
        /// </summary>
        public Vector3 headPoseRelativeOffsetRotation { get; set; }

        #region External Settings

        /// <summary>
        /// Set or get current cpu level(0-4), see also @YVR.Core.YVRPerformanceManager
        /// </summary>
        public int cpuLevel
        {
            get => YVRPerformanceManager.cpuLevel;
            set => YVRPerformanceManager.cpuLevel = value;
        }

        /// <summary>
        /// Set or get current gpu level(0-5), see also @YVR.Core.YVRPerformanceManager
        /// </summary>
        public int gpuLevel
        {
            get => YVRPerformanceManager.gpuLevel;
            set => YVRPerformanceManager.gpuLevel = value;
        }

        /// <summary>
        /// Set or get current vSync count, see also @YVR.Core.YVRQualityManager
        /// </summary>
        public YVRQualityManager.VSyncCount vSyncCount
        {
            get => qualityManager.vSyncCount;
            set => qualityManager.vSyncCount = value;
        }

        /// <summary>
        /// Set or get current fixed foveated rendering level, see also @YVR.Core.YVRQualityManager
        /// </summary>
        public YVRQualityManager.FixedFoveatedRenderingLevel fixedFoveatedRenderingLevel
        {
            get => qualityManager.fixedFoveatedRenderingLevel;
            set => qualityManager.fixedFoveatedRenderingLevel = value;
        }

        /// <summary>
        /// Get whether fixed foveation rendering is enabled or not
        /// </summary>
        public bool fixedFoveationRenderingEnabled =>
            fixedFoveatedRenderingLevel != YVRQualityManager.FixedFoveatedRenderingLevel.Off;

        /// <summary>
        /// Get whether to use recommend MSAA level, see also @YVR.Core.YVRQualityManager
        /// </summary>
        public bool useRecommendedMSAALevel => qualityManager.useRecommendedMSAALevel;

        /// <summary>
        /// Set or get current fixed foveated rendering dynamic, see also @YVR.Core.YVRQualityManager
        /// </summary>
        public bool fixedFoveatedRenderingDynamic
        {
            get => qualityManager.fixedFoveatedRenderingDynamic;
            set => qualityManager.fixedFoveatedRenderingDynamic = value;
        }

        /// <summary>
        /// Set or get current eyebuffer sharpenType, see also @YVR.Core.YVRQualityManager
        /// </summary>
        public YVRQualityManager.LayerSettingsType EyeBufferSharpenType
        {
            get => qualityManager.sharpenType;
            set => qualityManager.sharpenType = value;
        }

        public YVRTrackingStateManager.TrackingSpace trackingSpace
        {
            get => trackingManager.trackingSpace;
            set => trackingManager.trackingSpace = value;
        }

        /// <summary>
        /// Get current battery level, see also @YVR.Core.YVRHMDManager
        /// </summary>
        public float batteryLevel => hmdManager.batteryLevel;

        /// <summary>
        /// Get current battery temperature, see also @YVR.Core.YVRHMDManager
        /// </summary>
        public float batteryTemperature => hmdManager.batteryTemperature;

        /// <summary>
        /// Get current battery status(charge or not), see also @YVR.Core.YVRHMDManager
        /// </summary>
        public int batteryStatus => hmdManager.batteryStatus;

        /// <summary>
        /// Get current volume level, see also @YVR.Core.YVRHMDManager
        /// </summary>
        public float volumeLevel => hmdManager.volumeLevel;

        /// <summary>
        /// Get whether user is currently wearing the display, see also @YVR.Core.YVRHMDManager
        /// </summary>
        public bool isUserPresent => hmdManager.isUserPresent;

        /// <summary>
        /// Get current gpu utilizing level(0.0-1.0), see also @YVR.Core.YVRHMDManager
        /// </summary>
        public float gpuUtilLevel => YVRPerformanceManager.gpuUtilLevel;

        /// <summary>
        /// Get current cpu utilizing level（0.0-1.0）, see also @YVR.Core.YVRHMDManager
        /// </summary>
        public float cpuUtilLevel => YVRPerformanceManager.cpuUtilLevel;

        public HandTrackingSupport handTrackingSupport;

        public bool spatialAnchorSupport;

        public bool sceneSupport;

        #endregion

        private Transform m_AppSpaceTransform;
        private DepthTextureMode m_CachedDepthTextureMode;

        /// <summary>
        /// The override function of @YVR.Core.MonoBehaviorSingleton`1.Init
        /// </summary>
        protected override void Init()
        {
            base.Init();

            if (!m_UseUnityRig)
            {
                cameraRig = this.AutoAddingGetComponent<YVRCameraRig>();
                controllerRig = this.AutoAddingGetComponent<YVRControllerRig>();
                trackingManager.Initialize();
                cameraRig.Initialize(this);
                controllerRig.Initialize(this);
                cameraRenderer.Initialize(this);
            }
            else
            {
                // While using unity rig, we should disable the cameraRig to avoid the rendering twice
                cameraRig = GetComponent<YVRCameraRig>();
                cameraRig?.centerEyeAnchor.gameObject.SetActive(false);
                cameraRig?.leftEyeAnchor.gameObject.SetActive(false);
                cameraRig?.rightEyeAnchor.gameObject.SetActive(false);
            }

            qualityManager.Initialize();

            handManager = this.AutoAddingGetComponent<YVRHandManager>();
            handManager.Initialize();

            eventsManager.Initialize();

            if (useRecommendedMSAALevel) QualitySettings.antiAliasing = qualityManager.recommendAntiAlisingLevel;

            PackageVersion.PrintPackagesVersion(typeof(YVRManager));
            PackageVersion.PrintPackagesVersion(typeof(PackageVersion));
        }


        public void SetAppSWEnable(bool enable)
        {
            YVRPlugin.Instance.SetAppSWSwitch(enable);
            Camera camera = FindMainCamera();

            if (YVRPlugin.Instance.GetAppSWEnable() && YVRPlugin.Instance.GetAppSWSwitch())
            {
                m_CachedDepthTextureMode = camera.depthTextureMode;
                camera.depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
                if (camera.transform.parent == null)
                {
                    m_AppSpaceTransform.position = Vector3.zero;
                    m_AppSpaceTransform.rotation = Quaternion.identity;
                }
                else
                {
                    m_AppSpaceTransform = camera.transform.parent;
                }
            }
            else
            {
                camera.depthTextureMode = m_CachedDepthTextureMode;
                m_AppSpaceTransform = null;
            }
        }

        public bool GetAppSWEnable()
        {
            return YVRPlugin.Instance.GetAppSWSwitch();
        }

        private void Update()
        {
            YVRInput.Update();

            eventsManager.Update();
        }

        private void LateUpdate()
        {
            if (YVRPlugin.Instance.GetAppSWEnable() && YVRPlugin.Instance.GetAppSWSwitch() && m_AppSpaceTransform != null)
            {
                YVRPlugin.Instance.SetAppSpacePosition(m_AppSpaceTransform.position.x, m_AppSpaceTransform.position.y, m_AppSpaceTransform.position.z);
                YVRPlugin.Instance.SetAppSpaceRotation(m_AppSpaceTransform.rotation.x, m_AppSpaceTransform.rotation.y, m_AppSpaceTransform.rotation.z, m_AppSpaceTransform.rotation.w);
            }
        }

        private Camera FindMainCamera()
        {
            Camera result = null;
            GameObject[] cameraObjects = GameObject.FindGameObjectsWithTag("MainCamera");
            List<Camera> cameras = new List<Camera>();
            foreach (GameObject cameraObject in cameraObjects)
            {
                Camera camera = cameraObject.GetComponent<Camera>();
                if (camera != null && camera.enabled)
                {
                    cameras.Add(camera);
                }
            }

            if (cameras.Count == 0)
            {
                result = Camera.main;
            }
            else if (cameras.Count == 1)
            {
                result = cameras[0];
            }
            else
            {
                cameras.Sort((Camera c0, Camera c1) =>
                {
                    return c0.depth < c1.depth ? -1 :(c0.depth > c1.depth ? 1 : 0);
                });
                result = cameras[0];
            }
            return result;
        }
    }
}