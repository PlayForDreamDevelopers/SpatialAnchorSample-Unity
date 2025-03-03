using System.Collections.Generic;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// The manager of requests and messages
    /// </summary>
    public class YVRCallback
    {
        #region Request

        private static Dictionary<int, YVRRequest> s_DicRequestIDsToRequests = new Dictionary<int, YVRRequest>();

        internal static void AddRequest(YVRRequest request)
        {
            if (request.requestID == 0)
            {
                Debug.LogError($"[YVRPlatform] This request is invalid. Request failed. Request ID: {request.requestID}");
                return;
            }

            s_DicRequestIDsToRequests[request.requestID] = request;
        }

        #endregion

        /// <summary>
        /// This function pops all messages at one time.
        /// </summary>
        internal static void RunCallbacks()
        {
            while (true)
            {
                YVRMessage msg = YVRMessage.PopMessage();

                if (msg == null)
                {
                    break;
                }

                HandleMessage(msg);
            }
        }

        /// <summary>
        /// This function pops messages no more than limit at one time.
        /// </summary>
        /// <param name="limit"></param>
        internal static void RunCallbacks(uint limit)
        {
            for (int i = 0; i < limit; i++)
            {
                YVRMessage msg = YVRMessage.PopMessage();

                if (msg == null)
                {
                    break;
                }

                HandleMessage(msg);
            }
        }

        private static void HandleMessage(YVRMessage msg)
        {
            if (msg.requestID != 0 && s_DicRequestIDsToRequests.TryGetValue(msg.requestID, out YVRRequest req))
            {
                req.HandleMessage(msg);

                s_DicRequestIDsToRequests.Remove(msg.requestID);
            }
            else
            {
                Debug.LogErrorFormat("[YVRPlatform] This request ID is not recorded. Request ID: " + msg.requestID);
            }
        }

        internal static void OnApplicationQuit() { s_DicRequestIDsToRequests.Clear(); }
    }
}