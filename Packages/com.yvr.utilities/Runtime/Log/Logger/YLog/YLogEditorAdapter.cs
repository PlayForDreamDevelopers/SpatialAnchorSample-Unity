using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.TestTools;

namespace YVR.Utilities
{
    [ExcludeFromDocs, ExcludeFromCoverage]
    // YLogLoggerEditor will not handle RamLog related operations
    public class YLogEditorAdapter : IYLogAdapter
    {
#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        public void DebugHandle(string msg) { Debug.Log(msg); }

        public void RamDebugHandle(string msg) { }

#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        public void InfoHandle(string msg) { Debug.Log(msg); }

        public void RamInfoHandle(string msg) { }

#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        public void WarnHandle(string msg) { Debug.LogWarning(msg); }

        public void RamWarnHandle(string msg) { }

#if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
#endif
        public void ErrorHandle(string msg) { Debug.LogError(msg); }

        public void RamErrorHandle(string msg) { }

        public void ConfigureYLog(string tag, int ramLogSize = 5) { }

        public void SaveLog() { }

        public void SetLogcatOutputHandler(bool viaUnity) { }
    }
}