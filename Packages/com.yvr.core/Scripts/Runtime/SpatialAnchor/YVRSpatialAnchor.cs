using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using AOT;
using Unity.Collections;
using UnityEngine;

namespace YVR.Core
{
    public class YVRSpatialAnchor : Singleton<YVRSpatialAnchor>
    {
        public const int k_SpaceUuidMaxSize = 1024;
        public const int k_ShareUserMaxSize = 1024;
        public const int k_SpaceUUIDLength = 32;
        public const int k_SpaceFilterInfoComponentsMaxSize = 16;

        private static Dictionary<ulong, Action<YVRSpatialAnchorResult,bool>> m_CreateSpatialAnchorCallbacks =
            new Dictionary<ulong, Action<YVRSpatialAnchorResult,bool>>();

        private static Dictionary<ulong, Action<List<YVRSpatialAnchorResult>>> m_QuerySpatialAnchorCallbacks =
            new Dictionary<ulong, Action<List<YVRSpatialAnchorResult>>>();

        private static Dictionary<ulong, Action<YVRSpatialAnchorResult,bool>> m_EraseSpaceCallbacks =
            new Dictionary<ulong, Action<YVRSpatialAnchorResult,bool>>();

        private static Dictionary<ulong, Action<bool>> m_SpaceShareCompleteCallback =
            new Dictionary<ulong, Action<bool>>();

        private static Dictionary<ulong, Action<YVRSpatialAnchorSaveCompleteInfo,bool>> m_SpaceSaveCompleteCallback =
            new Dictionary<ulong, Action<YVRSpatialAnchorSaveCompleteInfo,bool>>();

        private static Dictionary<ulong, Action<YVRSpatialAnchorSetStatusCompleteInfo,bool>> m_SpaceSetStatusCompleteCallback =
            new Dictionary<ulong, Action<YVRSpatialAnchorSetStatusCompleteInfo,bool>>();

        private static Dictionary<ulong, Action<bool>> m_SpaceListSaveCompleteCallback =
            new Dictionary<ulong, Action<bool>>();

        [MonoPInvokeCallback(typeof(Action<YVRSpatialAnchorResult,bool>))]
        private static void OnCreateSpatialAnchor(YVRSpatialAnchorResult result ,bool success)
        {
            Debug.Log($"OnCreateSpatialAnchor requestId:{result.requestId} ,spaceId:{result.anchorHandle},success:{new string(result.uuid)}");
            if (!m_CreateSpatialAnchorCallbacks.ContainsKey(result.requestId)) return;

            if(success)
            {
                var componentStatusSetInfo = new YVRSpatialAnchorComponentStatusSetInfo();
                componentStatusSetInfo.component = YVRSpatialAnchorComponentType.Storable;
                componentStatusSetInfo.enable = true;
                instance.SetSpatialAnchorComponentStatus(result.anchorHandle, componentStatusSetInfo, null);
                componentStatusSetInfo.component = YVRSpatialAnchorComponentType.Sharable;
                instance.SetSpatialAnchorComponentStatus(result.anchorHandle, componentStatusSetInfo, null);
            }

            m_CreateSpatialAnchorCallbacks[result.requestId]?.Invoke(result,success);
            m_CreateSpatialAnchorCallbacks.Remove(result.requestId);
        }

        [MonoPInvokeCallback(typeof(Action<YVRQuerySpatialAnchorResult,ulong>))]
        private static void OnQuerySpatialAnchor(YVRQuerySpatialAnchorResult results ,ulong requestId)
        {
            Debug.Log($"OnQuerySpatialAnchor results:{results.numResults}");
            if (!m_QuerySpatialAnchorCallbacks.ContainsKey(requestId)) return;

            List<YVRSpatialAnchorResult> spaceResults = new List<YVRSpatialAnchorResult>();
            for (int i = 0; i < results.numResults; i++)
            {
                spaceResults.Add(results.results[i]);
            }

            m_QuerySpatialAnchorCallbacks[requestId]?.Invoke(spaceResults);
            m_QuerySpatialAnchorCallbacks.Remove(requestId);
        }

        [MonoPInvokeCallback(typeof(Action<YVRSpatialAnchorResult,bool>))]
        private static void OnEraseSpatialAnchor(YVRSpatialAnchorResult result,bool success)
        {
            Debug.Log($"OnEraseSpatialAnchor requestId:{result.requestId}");
            if (!m_EraseSpaceCallbacks.ContainsKey(result.requestId)) return;
            m_EraseSpaceCallbacks[result.requestId]?.Invoke(result,success);
            m_EraseSpaceCallbacks.Remove(result.requestId);
        }

        [MonoPInvokeCallback(typeof(Action<bool,ulong>))]
        private static void OnSpatialAnchorShareComplete(bool success, ulong requestId)
        {
            Debug.Log($"OnSpatialAnchorShareComplete requestId:{requestId},success:{success}");
            if (!m_SpaceShareCompleteCallback.ContainsKey(requestId)) return;

            m_SpaceShareCompleteCallback[requestId]?.Invoke(success);
            m_SpaceShareCompleteCallback.Remove(requestId);
        }

        [MonoPInvokeCallback(typeof(Action<YVRSpatialAnchorSaveCompleteInfo,bool>))]
        private static void OnSpatialAnchorSaveComplete(YVRSpatialAnchorSaveCompleteInfo resultInfo,bool success)
        {
            Debug.Log($"OnSpatialAnchorSaveComplete requestId:{resultInfo.requestId},uuid:{new string(resultInfo.uuid)}");
            if (!m_SpaceSaveCompleteCallback.ContainsKey(resultInfo.requestId)) return;

            m_SpaceSaveCompleteCallback[resultInfo.requestId]?.Invoke(resultInfo,success);
            m_SpaceSaveCompleteCallback.Remove(resultInfo.requestId);
        }

        [MonoPInvokeCallback(typeof(Action<YVRSpatialAnchorSetStatusCompleteInfo,bool>))]
        private static void OnSpatialAnchorSetStatusComplete(YVRSpatialAnchorSetStatusCompleteInfo resultInfo,bool success)
        {
            Debug.Log($"OnSpatialAnchorSetStatusComplete requestId:{resultInfo.requestId},uuid:{new string(resultInfo.uuid)}");
            if (!m_SpaceSetStatusCompleteCallback.ContainsKey(resultInfo.requestId)) return;

            m_SpaceSetStatusCompleteCallback[resultInfo.requestId]?.Invoke(resultInfo,success);
            m_SpaceSetStatusCompleteCallback.Remove(resultInfo.requestId);
        }

        [MonoPInvokeCallback(typeof(Action<bool,ulong>))]
        private static void OnSpatialAnchorListSaveComplete(bool success, ulong requestId)
        {
            Debug.Log($"OnSpatialAnchorListSaveComplete requestId:{requestId},success:{success}");
            if (!m_SpaceListSaveCompleteCallback.ContainsKey(requestId)) return;

            m_SpaceListSaveCompleteCallback[requestId]?.Invoke(success);
            m_SpaceListSaveCompleteCallback.Remove(requestId);
        }

        protected override void OnInit()
        {
            YVRPlugin.Instance.SetCreateSpatialAnchorCallback(OnCreateSpatialAnchor);
            YVRPlugin.Instance.SetQuerySpatialAnchorCallback(OnQuerySpatialAnchor);
            YVRPlugin.Instance.SetEraseSpatialAnchorCallback(OnEraseSpatialAnchor);
            YVRPlugin.Instance.SetSpatialAnchorShareCompleteCallback(OnSpatialAnchorShareComplete);
            YVRPlugin.Instance.SetSpatialAnchorSaveCompleteCallback(OnSpatialAnchorSaveComplete);
            YVRPlugin.Instance.SetSpatialAnchorStatusCompleteCallback(OnSpatialAnchorSetStatusComplete);
            YVRPlugin.Instance.SetSpatialAnchorSaveListCompleteCallback(OnSpatialAnchorListSaveComplete);
        }

        /// <summary>
        /// Creates a spatial anchor using the provided position and rotation.
        /// </summary>
        /// <param name="position">The position of the anchor</param>
        /// <param name="rotation">The rotation of the anchor</param>
        /// <param name="result">Callback for handling the result of the anchor creation process</param>
        public void CreateSpatialAnchor(Vector3 position, Quaternion rotation, Action<YVRSpatialAnchorResult,bool> result)
        {
            UInt64 requestId = 0;
            YVRPlugin.Instance.CreateSpatialAnchor(position,rotation, ref requestId);
            if (!m_CreateSpatialAnchorCallbacks.ContainsKey(requestId) && result!=null)
                m_CreateSpatialAnchorCallbacks.Add(requestId,result);

            if (requestId != 0)
                Debug.Log($"Creating spatial anchor... requestId:{requestId}");
            else
                Debug.LogError(
                    $"CreateSpatialAnchor failed with error code {requestId}");
        }

        /// <summary>
        /// Saves a spatial anchor using the provided save information.
        /// </summary>
        /// <param name="saveInfo">Information about the spatial anchor to be saved</param>
        /// <param name="callback">Callback for handling the completion of the spatial anchor save process</param>
        public void SaveSpatialAnchor(YVRSpatialAnchorSaveInfo saveInfo, Action<YVRSpatialAnchorSaveCompleteInfo,bool> callback)
        {
            UInt64 requestID = 0;
            YVRPlugin.Instance.SaveSpatialAnchor(saveInfo, ref requestID);
            if (!m_SpaceSaveCompleteCallback.ContainsKey(requestID) && callback!=null)
            {
                m_SpaceSaveCompleteCallback.Add(requestID,callback);
            }

            Debug.Log($"Saving spatial anchor...,requestId:{requestID}");
        }

        /// <summary>
        /// Erases a spatial anchor from the specified storage location.
        /// </summary>
        /// <param name="anchorHandle">The handle of the spatial anchor to be erased</param>
        /// <param name="location">The storage location from which the spatial anchor should be erased</param>
        /// <param name="callback">Callback for handling the result of the spatial anchor erasure process</param>
        public void EraseSpatialAnchor(UInt64 anchorHandle, YVRSpatialAnchorStorageLocation location,Action<YVRSpatialAnchorResult,bool> callback)
        {
            UInt64 requestID = default;
            YVRPlugin.Instance.DestroySpatialAnchor(anchorHandle, location, ref requestID);
            Debug.Log($"Erasing spatial anchor...,requestId:{requestID}");
            if (!m_EraseSpaceCallbacks.ContainsKey(requestID) && callback!=null)
                m_EraseSpaceCallbacks.Add(requestID,callback);
        }

        /// <summary>
        /// Queries spatial anchors based on the provided query information.
        /// </summary>
        /// <param name="queryInfo">Information for the spatial anchor query</param>
        /// <param name="queryCallback">Callback for handling the results of the spatial anchor query</param>
        public void QuerySpatialAnchor(YVRSpatialAnchorQueryInfo queryInfo , Action<List<YVRSpatialAnchorResult>> queryCallback)
        {
            if (queryInfo.ids != null && queryInfo.ids?.Length > k_SpaceUuidMaxSize)
            {
                Debug.LogError("QuerySpaitalAnchor attempted to query more uuids than the maximum, number supported: " +
                               k_SpaceUuidMaxSize);
                return;
            }

            if (queryInfo.ids != null)
            {
                queryInfo.numIds = queryInfo.ids.Length;
                if (queryInfo.ids.Length != k_SpaceUuidMaxSize)
                {
                    Array.Resize(ref queryInfo.ids,k_SpaceUuidMaxSize);
                }
            }

            UInt64 requestID = default;
            YVRPlugin.Instance.QuerySpatialAnchor(queryInfo, ref requestID);
            if (!m_QuerySpatialAnchorCallbacks.ContainsKey(requestID) && queryCallback != null)
                m_QuerySpatialAnchorCallbacks.Add(requestID,queryCallback);
        }

        /// <summary>
        /// Retrieves the pose of a spatial anchor identified by the given handle.
        /// </summary>
        /// <param name="anchorHandle">The handle of the spatial anchor</param>
        /// <param name="position">The position of the spatial anchor</param>
        /// <param name="rotation">The rotation of the spatial anchor</param>
        /// <param name="locationFlags">Flags indicating the location of the spatial anchor</param>
        /// <returns>True if the spatial anchor pose was successfully retrieved; otherwise, false</returns>
        /// <returns></returns>
        public bool GetSpatialAnchorPose(ulong anchorHandle, out Vector3 position, out Quaternion rotation,out YVRAnchorLocationFlags locationFlags)
        {
            locationFlags = default;
            position = default;
            rotation = default;
 
            bool result = YVRPlugin.Instance.GetSpatialAnchorPose(anchorHandle, ref position, ref rotation, ref locationFlags);
            GetSpatialAnchorComponentStatus(anchorHandle, YVRSpatialAnchorComponentType.SemanticLabels, out YVRSpatialAnchorComponentStatus status);
            if (status.enable)
            {
                rotation = rotation * YVRSceneAnchor.k_RotateY180;
            }
            return result;
        }

        /// <summary>
        /// Retrieves the supported components of a spatial anchor identified by the given handle.
        /// </summary>
        /// <param name="anchorHandle">The handle of the spatial anchor</param>
        /// <param name="components">Supported components of the spatial anchor</param>
        public void GetSpatialAnchorEnumerateSupported(ulong anchorHandle, out YVRSpatialAnchorSupportedComponent components)
        {
            components = default;
            YVRPlugin.Instance.GetSpatialAnchorEnumerateSupported(anchorHandle, ref components);
        }

        /// <summary>
        /// Sets the status of a component for a specific spatial anchor.
        /// </summary>
        /// <param name="anchorHandle">The handle of the spatial anchor</param>
        /// <param name="setInfo">Information for setting the status of the spatial anchor component</param>
        /// <param name="callback">Callback for handling the completion of the status setting process</param>
        /// <returns>True if setting the spatial anchor component status was successful; otherwise, false</returns>
        public bool SetSpatialAnchorComponentStatus(ulong anchorHandle, YVRSpatialAnchorComponentStatusSetInfo setInfo,
            Action<YVRSpatialAnchorSetStatusCompleteInfo,bool> callback)
        {
            UInt64 requestId = default;
            bool result = YVRPlugin.Instance.SetSpatialAnchorComponentStatus(anchorHandle, setInfo, ref requestId);
            if (!m_SpaceSetStatusCompleteCallback.ContainsKey(requestId) && callback != null)
            {
                m_SpaceSetStatusCompleteCallback.Add(requestId,callback);
            }

            Debug.Log($"SetSpatialAnchorComponentStatus...,requestId:{requestId}");
            return result;
        }

        /// <summary>
        /// Retrieves the status of a specific component for a spatial anchor identified by the given handle.
        /// </summary>
        /// <param name="anchorHandle">The handle of the spatial anchor</param>
        /// <param name="componentType">The type of the spatial anchor component</param>
        /// <param name="status">The status of the component for the spatial anchor</param>
        public void GetSpatialAnchorComponentStatus(ulong anchorHandle, YVRSpatialAnchorComponentType componentType, out YVRSpatialAnchorComponentStatus status)
        {
            status = default;
            YVRPlugin.Instance.GetSpatialAnchorComponentStatus(anchorHandle, componentType, ref status);
            Debug.Log($"GetSpatialAnchorComponentStatus...,status:{status.enable}");
        }

        /// <summary>
        /// Shares a spatial anchor with specified users.
        /// </summary>
        /// <param name="shareInfo">Information about the spatial anchor to be shared and the users it will be shared with</param>
        /// <param name="callback">Callback for handling the result of the sharing process</param>
        public void ShareSpatialAnchor(YVRSpatialAnchorShareInfo shareInfo,Action<bool> callback)
        {
            if (shareInfo.anchorHandle==null || shareInfo.users==null)
            {
                Debug.LogError($"Share spaces or users can't be null");
                return;
            }

            if (shareInfo.anchorHandle.Length > k_SpaceUuidMaxSize)
            {
                Debug.LogError(
                    $"The number of shared anchors cannot exceed the maximum value. number supported:{k_SpaceUuidMaxSize}");
            }

            if (shareInfo.users.Length>k_ShareUserMaxSize)
            {
                Debug.LogError(
                    $"The number of shared users cannot exceed the maximum value. number supported:{k_ShareUserMaxSize}");
            }

            shareInfo.anchorCount = shareInfo.anchorHandle.Length;
            if (shareInfo.anchorHandle.Length != k_SpaceUuidMaxSize)
            {
                Array.Resize(ref shareInfo.anchorHandle,k_SpaceUuidMaxSize);
            }

            shareInfo.userCount = shareInfo.users.Length;
            for (int i = 0; i < shareInfo.users.Length; i++)
            {
                ulong userHandle = 0;
                GetSpatialAnchorUserHandle(shareInfo.users[i], ref userHandle);
                shareInfo.users[i] = userHandle;
            }

            if (shareInfo.users.Length != k_ShareUserMaxSize)
            {
                Array.Resize(ref shareInfo.users, k_ShareUserMaxSize);
            }

            Debug.LogError($"shareInfo.userCount:{shareInfo.userCount}");
            UInt64 requestId = default;
            YVRPlugin.Instance.ShareSpatialAnchor(shareInfo, ref requestId);
            Debug.Log($"ShareSpatialAnchor...,requestId:{requestId}");
            if (!m_SpaceShareCompleteCallback.ContainsKey(requestId) && callback != null)
                m_SpaceShareCompleteCallback.Add(requestId,callback);
        }

        /// <summary>
        /// Saves the spatial anchor list to the cloud and then shares it with specified users.
        /// </summary>
        /// <param name="shareInfo">Information about the spatial anchor list to be shared and the users with whom it will be shared</param>
        /// <param name="callback">Callback for handling the result of the combined save and share process</param>
        public void SaveToCloudThenShare(YVRSpatialAnchorShareInfo shareInfo,Action<bool> callback)
        {
            var saveList = new List<ulong>();
            if (shareInfo.anchorHandle == null || shareInfo.anchorHandle.Length == 0)
            {
                Debug.LogError($"SaveCloudThenShare anchorHandle can't be null");
                return;
            }

            for (int i = 0; i < shareInfo.anchorHandle.Length; i++)
            {
                GetSpatialAnchorUUIDForHandle(shareInfo.anchorHandle[i], out YVRSpatialAnchorUUID uuid);
                if (string.IsNullOrEmpty(new string(uuid.Id)))
                {
                    Debug.LogError($"Share anchor list has invalid anchorHandle:{shareInfo.anchorHandle[i]}");
                    return;
                }
                saveList.Add(shareInfo.anchorHandle[i]);
            }

            SaveSpatialAnchorList(saveList, YVRSpatialAnchorStorageLocation.Cloud, (result) =>
            {
                if (result)
                    ShareSpatialAnchor(shareInfo,callback);
                else
                    Debug.LogError($"Save spatial anchor list cloud failed");
            });
        }

        /// <summary>
        /// Saves a list of spatial anchors to the specified storage location.
        /// </summary>
        /// <param name="spatialAnchorHandleList">The list of spatial anchor handles to be saved</param>
        /// <param name="location">The storage location for saving the spatial anchor list</param>
        /// <param name="callback">Callback for handling the result of the spatial anchor list save process</param>
        public void SaveSpatialAnchorList(List<ulong> spatialAnchorHandleList, YVRSpatialAnchorStorageLocation location,
            Action<bool> callback)
        {
            if (spatialAnchorHandleList== null || spatialAnchorHandleList.Count == 0)
            {
                Debug.LogError($"SaveSpatialAnchorList spatialAnchorHandleList can't be null");
                return;
            }

            var listSaveInfo = new YVRSpatialAnchorListSaveInfo();
            listSaveInfo.anchorCount = spatialAnchorHandleList.Count;
            listSaveInfo.storageLocation = location;
            int size = Marshal.SizeOf<ulong>();
            IntPtr intPtr = Marshal.AllocHGlobal(size * spatialAnchorHandleList.Count);
            for (int i = 0; i < spatialAnchorHandleList.Count; i++)
            {
                IntPtr anchorIntPtr = IntPtr.Add(intPtr, size * i);
                Marshal.StructureToPtr(spatialAnchorHandleList[i], anchorIntPtr, false);
            }

            listSaveInfo.anchorHandles = intPtr;
            UInt64 requestId = default;
            YVRPlugin.Instance.SaveSpatialAnchorList(listSaveInfo,ref requestId);
            if (!m_SpaceListSaveCompleteCallback.ContainsKey(requestId) && callback!=null)
            {
                m_SpaceListSaveCompleteCallback.Add(requestId,callback);
            }

            Marshal.FreeHGlobal(intPtr);
        }

        /// <summary>
        /// Retrieves the UUID for a spatial anchor identified by the given handle.
        /// </summary>
        /// <param name="anchorHandle">The handle of the spatial anchor</param>
        /// <param name="uuid">The UUID of the spatial anchor</param>
        public void GetSpatialAnchorUUIDForHandle(ulong anchorHandle, out YVRSpatialAnchorUUID uuid)
        {
            uuid = default;
            YVRPlugin.Instance.GetSpatialAnchorUUID(anchorHandle, ref uuid);
        }

        /// <summary>
        /// Checks if the position is valid based on the specified location flags.
        /// </summary>
        /// <param name="value">The anchor location flags to be evaluated</param>
        /// <returns>True if the position is valid; otherwise, false</returns>
        public bool IsPositionValid(YVRAnchorLocationFlags value) =>
            (value & YVRAnchorLocationFlags.LocationPositionValid) != 0;

        /// <summary>
        /// Checks if the orientation is valid based on the specified location flags.
        /// </summary>
        /// <param name="value">The anchor location flags to be evaluated</param>
        /// <returns>True if the orientation is valid; otherwise, false</returns>
        public bool IsOrientationValid(YVRAnchorLocationFlags value) =>
            (value & YVRAnchorLocationFlags.LocationOrientationValid) != 0;

        /// <summary>
        /// Checks if the position is tracked based on the specified location flags.
        /// </summary>
        /// <param name="value">The anchor location flags to be evaluated</param>
        /// <returns>True if the position is tracked; otherwise, false</returns
        public bool IsPositionTracked(YVRAnchorLocationFlags value) =>
            (value & YVRAnchorLocationFlags.LocationPositionTracked) != 0;

        /// <summary>
        /// Checks if the orientation is tracked based on the specified location flags.
        /// </summary>
        /// <param name="value">The anchor location flags to be evaluated</param>
        /// <returns>True if the orientation is tracked; otherwise, false</returns>
        public bool IsOrientationTracked(YVRAnchorLocationFlags value) =>
            (value & YVRAnchorLocationFlags.LocationOrientationTracked) != 0;

        private void GetSpatialAnchorUserHandle(ulong userId, ref UInt64 spaceUser)
        {
            YVRPlugin.Instance.CreateSpatialAnchorUserHandle(userId,ref spaceUser);
        }

        private void GetSpatialAnchorUserIdforHandle(ulong spaceUser, out ulong userId)
        {
            userId = default;
            YVRPlugin.Instance.GetSpatialAnchorUserId(spaceUser, ref userId);
        }
    }
}