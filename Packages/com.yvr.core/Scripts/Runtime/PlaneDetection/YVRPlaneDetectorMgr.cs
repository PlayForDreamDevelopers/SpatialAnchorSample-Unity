using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Profiling;
using UnityEngine;
using YVR.Core;

public class YVRPlaneDetectorMgr : Singleton<YVRPlaneDetectorMgr>
{
    public const int k_PlaneDetectorOrientationCount = 5;
    public const int k_SemanticTypeCount = 6;

    /// <summary>
    /// Checks if the device supports plane detection.
    /// </summary>
    /// <returns>True if the device supports plane detection, otherwise false.</returns>
    public bool GetPlaneDetectorSupported()
    {
        return YVRPlugin.Instance.GetPlaneDetectionSupported();
    }

    /// <summary>
    /// Creates plane detection.
    /// </summary>
    public void CreatePlaneDetector()
    {
        YVRPlugin.Instance.CreatePlaneDetection();
    }

    /// <summary>
    /// Set plane detector filter.
    /// </summary>
    public void BeginPlaneDetector(YVRPlaneDetectorBeginInfo planeDetectorBeginInfo = default)
    {
        if (planeDetectorBeginInfo.orientations != null &&
            planeDetectorBeginInfo.orientations.Length > k_PlaneDetectorOrientationCount)
        {
            Debug.LogError("PlaneDetector oriented filter cannot exceed the maximum, number supported: " +k_PlaneDetectorOrientationCount);
            return;
        }

        if (planeDetectorBeginInfo.semanticTypes!=null && planeDetectorBeginInfo.semanticTypes.Length>k_SemanticTypeCount)
        {
            Debug.LogError("PlaneDetector semantic filter cannot exceed the maximum, number supported: " +k_SemanticTypeCount);
            return;
        }

        if (planeDetectorBeginInfo.orientations!=null)
        {
            planeDetectorBeginInfo.orientationCount = planeDetectorBeginInfo.orientations.Length;
            if (planeDetectorBeginInfo.orientations.Length!=k_PlaneDetectorOrientationCount)
            {
                Array.Resize(ref planeDetectorBeginInfo.orientations,k_PlaneDetectorOrientationCount);
            }
        }

        if (planeDetectorBeginInfo.semanticTypes!=null)
        {
            planeDetectorBeginInfo.semanticTypeCount = planeDetectorBeginInfo.semanticTypes.Length;
            if (planeDetectorBeginInfo.semanticTypes.Length != k_SemanticTypeCount)
            {
                Array.Resize(ref planeDetectorBeginInfo.semanticTypes,k_SemanticTypeCount);
            }
        }

        YVRPlugin.Instance.BeginPlaneDetection(planeDetectorBeginInfo);
    }

    /// <summary>
    /// Gets the current state of plane detection.
    /// </summary>
    public void GetPlaneDetectorState()
    {
        YVRPlugin.Instance.GetPlaneDetectionState();
    }

    /// <summary>
    /// Ends plane detection.
    /// </summary>
    public void EndPlaneDetector()
    {
        YVRPlugin.Instance.EndPlaneDetection();
    }

    public void GetPlaneDetectorInfo(ref YVRPlaneDetectorLocations yvrPlaneDetectorLocations)
    {
        IntPtr  planeInfoIntPtr= YVRPlugin.Instance.GetPlaneDetectionsInfo();
        if (planeInfoIntPtr != IntPtr.Zero)
        {
            yvrPlaneDetectorLocations = Marshal.PtrToStructure<YVRPlaneDetectorLocations>(planeInfoIntPtr);
            List<YVRPlaneDetectorLocation> planeDetectorLocation =
                ConvertIntPtr2List<YVRPlaneDetectorLocation>(yvrPlaneDetectorLocations.planeLocationsIntPtr,
                    yvrPlaneDetectorLocations.planeLocationCountOutput);
            yvrPlaneDetectorLocations.planeLocation = planeDetectorLocation.ToArray();
            for (int i = 0; i < planeDetectorLocation.Count; i++)
            {
                List<YVRPlaneDetectorPolygonBuffer> polygonBuffers =
                    ConvertIntPtr2List<YVRPlaneDetectorPolygonBuffer>(planeDetectorLocation[i].polygonBufferIntPtr,
                        planeDetectorLocation[i].polygonBufferCount);
                yvrPlaneDetectorLocations.planeLocation[i].polygonBuffers = polygonBuffers.ToArray();
                for (int j = 0; j < polygonBuffers.Count; j++)
                {
                    List<Vector2> vector2s = ConvertIntPtr2List<Vector2>(polygonBuffers[j].verticesIntPtr,
                        polygonBuffers[j].vertexCountOutput);
                    yvrPlaneDetectorLocations.planeLocation[i].polygonBuffers[j].point = vector2s.ToArray();
                }
            }

            YVRPlugin.Instance.DeletePlaneIntptr();
        }
        else
        {
            yvrPlaneDetectorLocations = default;
        }
    }

    public static List<T> ConvertIntPtr2List<T>(IntPtr ptr ,uint count)
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
