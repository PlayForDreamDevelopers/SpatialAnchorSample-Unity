using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SpatialAnchorItem : MonoBehaviour
{
    public char[] uuidChar;
    public ulong spaceHandle;
    private string m_uuidString;
    public string uuidString
    {
        get
        {
            if (m_uuidString.IsNullOrEmpty() && uuidChar !=null)
            {
                m_uuidString = new string(uuidChar);
            }

            return m_uuidString;
        }
    }

    public void SetSpatialAnchorData(char[] uuid, ulong spacehandle)
    {
        this.uuidChar = uuid;
        this.spaceHandle = spacehandle;
    }
}
