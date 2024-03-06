using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EyeTrackingData
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EyeGazesState
    { 
        public EyeGazeState leftEyeGaze;
        public EyeGazeState rightEyeGaze;
        public Int64 time;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EyeGazeState
    {
        public Vector3 position;
        public Quaternion rotation;
        public float confidence;
        public bool isValid;
    }
}
