using System;
using UnityEngine;

namespace YVR.Core
{
    /// <summary>
    /// Encapsulate all tracking related operation
    /// </summary>
    [Serializable]
    public partial class YVRTrackingStateManager
    {
        [SerializeField] private TrackingSpace _trackingSpace = TrackingSpace.EyeLevel;

        /// <summary>
        /// Set or get the tracking space of the rigid poses
        /// </summary>
        public TrackingSpace trackingSpace
        {
            get => _trackingSpace;
            set
            {
                if (_trackingSpace == value) return;
                _trackingSpace = value;
                ApplyTrackingSpace();
            }
        }
        /// <summary>
        ///Initialize tracking space and IPD setting
        /// </summary>
        public void Initialize()
        {
            ApplyTrackingSpace();
        }

        private void ApplyTrackingSpace()
        {
            YVRPlugin.Instance.SetTrackingSpace(_trackingSpace);
        }
    }
}