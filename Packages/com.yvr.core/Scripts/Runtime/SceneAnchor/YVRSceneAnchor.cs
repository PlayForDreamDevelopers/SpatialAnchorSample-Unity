using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace YVR.Core
{
    public class YVRSceneAnchor : Singleton<YVRSceneAnchor>
    {

        private static Dictionary<ulong, Action<bool>> m_RequestSceneCaptureCallbacks = new Dictionary<ulong, Action<bool>>();
        internal static readonly Quaternion k_RotateY180 = Quaternion.Euler(0, 180, 0);


        [MonoPInvokeCallback(typeof(Action<ulong, bool>))]
        private static void OnSceneCaptureCallbacks(ulong requestId, bool result)
        {
            if (!m_RequestSceneCaptureCallbacks.ContainsKey(requestId)) return;

            m_RequestSceneCaptureCallbacks[requestId].Invoke(result);
            m_RequestSceneCaptureCallbacks.Remove(requestId);
        }

        protected override void OnInit()
        {
            YVRPlugin.Instance.SetSceneCaptureCallback(OnSceneCaptureCallbacks);
        }

        /// <summary>
        /// Retrieves the 2D bounding box of an anchor.
        /// </summary>
        /// <param name="anchorHandle">The unique identifier of the anchor.</param>
        /// <param name="boundingBox2D">2D bounding box of the anchor.</param>
        /// <returns>Returns true if the operation is successful, otherwise false.</returns>
        public bool GetAnchorBoundingBox2D(ulong anchorHandle, out YVRRect2D boundingBox2D)
        {
            boundingBox2D = default;
            int result = YVRPlugin.Instance.GetSpaceBoundingBox2D(anchorHandle, ref boundingBox2D);
            if (result != 0) Debug.LogError($"Get anchor bounding box2d error, xr result code:{result}");
            return result == 0;
        }

        /// <summary>
        /// Retrieves the 3D bounding box of an anchor.
        /// </summary>
        /// <param name="anchorHandle">The unique identifier of the anchor.</param>
        /// <param name="boundingBox3D">3D bounding box of the anchor.</param>
        /// <returns>Returns true if the operation is successful, otherwise false.</returns>
        public bool GetAnchorBoundingBox3D(ulong anchorHandle, out YVRRect3D boundingBox3D)
        {
            boundingBox3D = default;
            int result = YVRPlugin.Instance.GetSpaceBoundingBox3D(anchorHandle, ref boundingBox3D);
            if (result != 0) Debug.LogError($"Get anchor bounding box3d error, xr result code:{result}");
            return result == 0;
        }

        /// <summary>
        /// Retrieves the 2D boundary vertices of an anchor.
        /// </summary>
        /// <param name="anchorHandle">The unique identifier of the anchor.</param>
        /// <param name="boundary">List of 2D boundary vertices of the anchor.</param>
        /// <returns>Returns true if the operation is successful, otherwise false.</returns>
        public bool GetAnchorBoundary2D(ulong anchorHandle, out List<Vector2> boundary)
        {
            boundary = null;
            var boundaryInternal = new YVRBoundary2D()
            {
                vertexCapacityInput = 0,
                vertexCountOutput = 0,
            };

            int result = YVRPlugin.Instance.GetSpaceBoundary2D(anchorHandle, ref boundaryInternal);
            if (result != 0)
            {
                Debug.LogError($"Get anchor boundary 2d error, xr result code:{result}");
                return false;
            }

            Debug.Log("Get anchor boundary 2d output count :{boundary2D.vertexCountOutput}");
            boundaryInternal.vertexCapacityInput = boundaryInternal.vertexCountOutput;
            int size = Marshal.SizeOf<Vector2>();
            boundaryInternal.vertices = Marshal.AllocHGlobal(boundaryInternal.vertexCountOutput * size);
            result = YVRPlugin.Instance.GetSpaceBoundary2D(anchorHandle, ref boundaryInternal);
            boundary = new List<Vector2>();
            boundary = ConvertIntPtr2List<Vector2>(boundaryInternal.vertices, (uint)boundaryInternal.vertexCountOutput);
            for (int i = 0; i < boundaryInternal.vertexCountOutput; i++)
            {
                Debug.Log($"Get anchor boundary 2d vertex.x:{boundary[i].x},y:{boundary[i].y}");
            }

            Marshal.FreeHGlobal(boundaryInternal.vertices);
            return result == 0;
        }

        /// <summary>
        /// Retrieves the semantic labels associated with an anchor.
        /// </summary>
        /// <param name="anchorHandle">The unique identifier of the anchor.</param>
        /// <param name="labels">Semantic labels related to the anchor.</param>
        /// <returns>Returns true if the operation is successful, otherwise false.</returns>
        public bool GetAnchorSemanticLabels(ulong anchorHandle, out string labels)
        {
            labels = "";
            var labelsInternal = new YVRAnchorSemanticLabel()
            {
                byteCapacityInput = 0,
                byteCountOutput = 0,
            };

            int result = YVRPlugin.Instance.GetSpaceSemanticLabels(anchorHandle, ref labelsInternal);
            if (result != 0)
            {
                Debug.LogError($"Get anchor semantic labels error. xr result code:{result}");
                return false;
            }

            labelsInternal.byteCapacityInput = labelsInternal.byteCountOutput;
            labelsInternal.labels = Marshal.AllocHGlobal(sizeof(byte) * labelsInternal.byteCountOutput);
            result = YVRPlugin.Instance.GetSpaceSemanticLabels(anchorHandle, ref labelsInternal);
            if (result != 0)
            {
                Debug.LogError($"Get anchor semantic labels error. xr result code:{result}");
                return false;
            }

            labels = Marshal.PtrToStringAnsi(labelsInternal.labels, labelsInternal.byteCountOutput);
            Marshal.FreeHGlobal(labelsInternal.labels);
            return true;
        }

        /// <summary>
        /// Retrieves the 3D room layout information associated with an anchor.
        /// </summary>
        /// <param name="anchorHandle">The unique identifier of the anchor.</param>
        /// <param name="roomLayout">3D room layout information related to the anchor.</param>
        /// <returns>Returns true if the operation is successful, otherwise false.</returns>
        public bool GetAnchorRoomLayout(ulong anchorHandle, out YVRRoomLayout roomLayout)
        {
            roomLayout = new YVRRoomLayout();
            int result = YVRPlugin.Instance.GetSpaceRoomLayout(anchorHandle, ref roomLayout);
            if (result != 0)
            {
                Debug.LogError($"Get anchor room layout error, xr result code:{result}");
                return false;
            }

            Debug.Log($"Get room layout wall count :{roomLayout.wallUuidCountOutput}");
            roomLayout.wallUuidCapacityInput = roomLayout.wallUuidCountOutput;
            int size = Marshal.SizeOf<YVRSpatialAnchorUUID>();
            Debug.Log($"Get YVRSpatialAnchorUUID struct size:{size}");
            roomLayout.wallUuidsPtr = Marshal.AllocHGlobal(size * roomLayout.wallUuidCountOutput);
            result = YVRPlugin.Instance.GetSpaceRoomLayout(anchorHandle, ref roomLayout);
            if (result != 0)
            {
                Debug.LogError($"Get anchor room layout error, xr result code:{result}");
                return false;
            }

            Debug.Log($"Get anchor room layout floorUuid, uuid:{new string(roomLayout.floorUuid.Id)}");
            Debug.Log($"Get anchor room layout ceilingUuid, uuid:{new string(roomLayout.ceilingUuid.Id)}");
            List<YVRSpatialAnchorUUID> uuids = ConvertIntPtr2List<YVRSpatialAnchorUUID>(roomLayout.wallUuidsPtr, (uint)roomLayout.wallUuidCountOutput);
            for (int i = 0; i < roomLayout.wallUuidCountOutput; i++)
            {
                Debug.Log($"Get anchor room layout walluuids, i:{i},uuid:{new string(uuids[i].Id)}");
            }

            roomLayout.wallUuids = uuids.ToArray();
            Marshal.FreeHGlobal(roomLayout.wallUuidsPtr);
            return true;
        }

        /// <summary>
        /// Retrieves the container information associated with an anchor.
        /// </summary>
        /// <param name="anchorHandle">The unique identifier of the anchor.</param>
        /// <param name="containerUuids">List of UUIDs representing the container of the anchor.</param>
        /// <returns>Returns true if the operation is successful, otherwise false.</returns>
        public bool GetAnchorContainer(ulong anchorHandle, out List<YVRSpatialAnchorUUID> containerUuids)
        {
            containerUuids = null;
            YVRSceneAnchorContainer sceneAnchorContainer = default;
            int result = YVRPlugin.Instance.GetSpaceContainer(anchorHandle, ref sceneAnchorContainer);
            if (result != 0)
            {
                Debug.LogError($"Get anchor container error, xr result code:{result}");
                return false;
            }

            int size = Marshal.SizeOf<YVRSpatialAnchorUUID>();
            Debug.Log($"Get anchor container output count :{sceneAnchorContainer.uuidCountOutput}");
            Debug.Log($"Get YVRSpatialAnchorUUID struct size:{size}");
            sceneAnchorContainer.uuidCapacityInput = sceneAnchorContainer.uuidCountOutput;
            sceneAnchorContainer.uuids = Marshal.AllocHGlobal(size * sceneAnchorContainer.uuidCountOutput);
            result = YVRPlugin.Instance.GetSpaceContainer(anchorHandle, ref sceneAnchorContainer);
            if (result != 0)
            {
                Debug.LogError($"Get anchor container error, xr result code:{result}");
                return false;
            }

            containerUuids = ConvertIntPtr2List<YVRSpatialAnchorUUID>(sceneAnchorContainer.uuids, (uint)sceneAnchorContainer.uuidCountOutput);
            Marshal.FreeHGlobal(sceneAnchorContainer.uuids);
            return true;
        }

        /// <summary>
        /// Sends a request for scene capture with the specified request string.
        /// </summary>
        /// <param name="requestString">The request string for scene capture.</param>
        public bool RequestSceneCapture(string requestString,Action<bool> callback)
        {
            ulong requestId = default;
            YVRSceneCaptureRequest sceneCaptureRequest = new YVRSceneCaptureRequest
            {
                requestByteCount = requestString == null ? 0 : System.Text.Encoding.ASCII.GetByteCount(requestString),
                request = requestString,
            };

            int result = YVRPlugin.Instance.RequestSceneCapture(ref sceneCaptureRequest, ref requestId);
            if (!m_RequestSceneCaptureCallbacks.ContainsKey(requestId) && callback != null)
            {
                m_RequestSceneCaptureCallbacks.Add(requestId, callback);
            }

            return result == 0;
        }

        public List<T> ConvertIntPtr2List<T>(IntPtr ptr ,uint count)
        {
            List<T> objArray = new List<T>();
            for (int i = 0; i < count; i++)
            {
                IntPtr objPtr = IntPtr.Add(ptr, i * Marshal.SizeOf<T>());
                objArray.Add(Marshal.PtrToStructure<T>(objPtr));
            }

            return objArray;
        }
    }
}