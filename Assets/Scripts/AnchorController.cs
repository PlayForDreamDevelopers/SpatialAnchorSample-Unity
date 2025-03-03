using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YVR.Core;

public class AnchorController : MonoBehaviour
{
    [SerializeField] private Text anchorName;

    [SerializeField] private Text anchorPose;

    [SerializeField] private GameObject anchorGameObject;

    private SpatialAnchorItem m_spatialAnchor;

    private Vector3 m_PrePosition;
    private Quaternion m_PreRotation;

    private void Awake()
    {
        m_spatialAnchor = GetComponent<SpatialAnchorItem>();
    }

    public void SetAnchorName(string uuid)
    {
        anchorName.text = "UUID:" + m_spatialAnchor.uuidString;
    }

    public void OnSaveLocalButtonPressed()
    {
        if (m_spatialAnchor == null)
        {
            return;
        }

        YVRSpatialAnchorSaveInfo saveInfo = new YVRSpatialAnchorSaveInfo();
        saveInfo.anchorHandle = m_spatialAnchor.spaceHandle;
        saveInfo.storageLocation = YVRSpatialAnchorStorageLocation.Local;
        YVRSpatialAnchor.instance.SaveSpatialAnchor(saveInfo, OnSaveCompleteCallback);
    }

    private void OnSaveCompleteCallback(YVRSpatialAnchorSaveCompleteInfo saveResult,bool success)
    {
        Debug.Log($"Save spatial anchor result:{success}");
    }

    public void OnHideButtonPressed()
    {
        SpatialAnchorManager.Instance.anchorDic.Remove(m_spatialAnchor.uuidString);
        Destroy(gameObject);
    }

    public void OnEraseButtonPressed()
    {
        if (m_spatialAnchor.spaceHandle == 0) return;

        YVRSpatialAnchor.instance.EraseSpatialAnchor(m_spatialAnchor.spaceHandle, YVRSpatialAnchorStorageLocation.Local,
            OnEraseCompleteCallback);
    }

    private void OnEraseCompleteCallback(YVRSpatialAnchorResult result,bool success)
    {
        if (success)
        {
            SpatialAnchorManager.Instance.anchorDic.Remove(m_spatialAnchor.uuidString);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        QueryAnchorPose();
    }

    private void QueryAnchorPose()
    {
        Vector3 position = Vector3.zero;
        Quaternion quaternion = Quaternion.identity;
        bool result = YVRSpatialAnchor.instance.GetSpatialAnchorPose(m_spatialAnchor.spaceHandle,out position,out quaternion,out YVRAnchorLocationFlags anchorLocationFlags);
        anchorGameObject.SetActive(result);
        if (result)
        {
            if (m_PrePosition == position && m_PreRotation == quaternion) return;

            m_PrePosition = position;
            m_PreRotation = quaternion;
            this.transform.position = position;
            this.transform.rotation = quaternion;
            anchorPose.text = "Position:\n"+$"({position.x.ToString("0.###")},{position.y.ToString("0.###")},{position.z.ToString("0.###")})\n";
            anchorPose.text += "Rotation:\n"+$"({quaternion.x.ToString("0.###")},{quaternion.y.ToString("0.###")},{quaternion.z.ToString("0.###")},{quaternion.w.ToString("0.###")})";
        }
    }
}