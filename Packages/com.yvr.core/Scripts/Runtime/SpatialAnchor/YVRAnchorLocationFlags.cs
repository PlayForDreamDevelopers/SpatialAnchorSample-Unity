using System.Runtime.InteropServices;

namespace YVR.Core
{
    public enum YVRAnchorLocationFlags
    {
        LocationOrientationValid = 1,
        LocationPositionValid = 2,
        LocationOrientationTracked = 4,
        LocationPositionTracked = 8,
    }
}