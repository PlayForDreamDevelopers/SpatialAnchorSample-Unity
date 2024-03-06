using UnityEngine;

namespace YVR.Core
{
    public partial class YVRCameraRenderer
    {
        /// <summary>
        /// Contains full fov information per eye in degree
        /// Under Symmetric Fov mode, UpFov == DownFov and LeftFov == RightFov.
        /// </summary>
        public struct EyeFov
        {
            /// <summary>
            /// the fov of left screen
            /// </summary>
            public float LeftFov;
            /// <summary>
            ///  the fov of right screen
            /// </summary>
            public float RightFov;
            /// <summary>
            /// the fov of up screen
            /// </summary>
            public float UpFov;
            /// <summary>
            /// the fov of down screen
            /// </summary>
            public float DownFov;
        }

        /// <summary>
        /// Specifies the size and field-of-view for one eye texture.
        /// </summary>
        public struct EyeRenderDescription
        {
            /// <summary>
            /// The horizontal and vertical size of the texture.
            /// </summary>
            public Vector2 resolution;

            /// <summary>
            /// The angle of the horizontal and vertical field of view in degrees.
            /// For Symmetric FOV interface compatibility
            /// Note this includes the fov angle from both sides
            /// </summary>
            public Vector2 fov;

            /// <summary>
            /// The full information of field of view in degrees.
            /// </summary>
            public EyeFov fullFov;
        }
    }
}