using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YVR.Core;
#if XR_CORE_UTILS
using Unity.XR.CoreUtils;
#endif

public class EquirectShapeHandler : ILayerShapeHandler
{
    public void HandleLayerPose(IYVRLayerHandle layerHandle, params object[] data)
    {
        int renderLayerId = (int)data[0];
        Transform transform = data[1] as Transform;
        YVRManager yvrManager = data[2] as YVRManager;
        XRPose pose = new XRPose();
#if XR_CORE_UTILS
        if (GameObject.FindObjectOfType<XROrigin>() != null)
        {
            pose = transform.ToXRTrackingSpacePose(GameObject.FindObjectOfType<XROrigin>().Camera);
        }
        else
        {
            pose = transform.ToXRTrackingSpacePose(yvrManager.cameraRenderer.centerEyeCamera);
        }
#else
        pose = transform.ToXRTrackingSpacePose(yvrManager.cameraRenderer.centerEyeCamera);
#endif

        layerHandle.SetLayerPose(renderLayerId, pose);
    }

    public void HandleLayerShape(IYVRLayerHandle layerHandle, params object[] data)
    {
        int renderLayerId = (int)data[0];
        float radius = (float)data[3];

        layerHandle.SetLayerEquirectParam(renderLayerId, radius);
    }
}
