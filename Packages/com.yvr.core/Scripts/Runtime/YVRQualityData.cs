using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace YVR.Core
{
    public partial class YVRQualityManager
    {
        /// <summary>
        /// VSync count every frame.
        /// </summary>
        public enum VSyncCount
        {
            /// <summary>
            /// VSync every frame
            /// </summary>
            k1 = 1,
            /// <summary>
            /// VSync every two frame,FPS reduced to half.
            /// </summary>
            k2 = 2,
        };

        /// <summary>
        /// The level of fixed foveated rendering
        /// </summary>
        public enum FixedFoveatedRenderingLevel
        {
            /// <summary>
            /// Disable fixed foveated rendering.
            /// </summary>
            Off = 0,
            /// <summary>
            ///  The lowest level of fixed foveated rendering
            /// </summary>
            Low = 1,
            /// <summary>
            ///  The Medium level of fixed foveated rendering
            /// </summary>
            Medium = 2,
            /// <summary>
            /// The High level of fixed foveated rendering
            /// </summary>
            High = 3
        }

        public enum FixedFoveatedRenderingDynamic
        {
            /// <summary>
            /// Disable dynamic level
            /// </summary>
            Disabled = 0,
            /// <summary>
            /// Enable dynamic level
            /// </summary>
            Enabled = 1
        }

        public enum LayerSettingsType
        {
            /// <summary>
            /// no super sampling or sharpen
            /// </summary>
            None,
            /// <summary>
            /// normal super sampling or sharpen
            /// </summary>
            Normal,
            /// <summary>
            /// expensive cost super sampling or sharpen
            /// </summary>
            Quality
        }
    }
}