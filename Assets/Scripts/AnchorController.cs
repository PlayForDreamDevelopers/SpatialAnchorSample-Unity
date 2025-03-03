using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.UI;
using YVR.Core;

public class AnchorController : MonoBehaviour
{
    [SerializeField] private Text anchorName;

    [SerializeField] private Text anchorPose;

    [SerializeField] private Image saveIcon;

    [SerializeField] private GameObject shareButton;

    [SerializeField] private Image shareIcon;

    [SerializeField] private Image alignIcon;

    [SerializeField] private Color grayColor;

    [SerializeField] private Color greenColor;

    [SerializeField] private GameObject anchorGameObject;

    private SpatialAnchorItem m_spatialAnchor;

    private Vector3 m_PrePosition;
    private Quaternion m_PreRotation;

    public bool IsSavedLocally
    {
        set
        {
            if (saveIcon != null)
            {
                saveIcon.color = value ? greenColor : grayColor;
            }
        }
    }

    public bool IsSelectedForShare
    {
        set
        {
            if (shareIcon != null)
            {
                shareIcon.color = value ? greenColor : grayColor;
            }
        }
    }

    public bool IsSelectedForAlign
    {
        set
        {
            if (alignIcon != null)
            {
                alignIcon.color = value ? greenColor : grayColor;
            }
        }
    }

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
        LogController.Instance.Log($"Start save anchor uuid:{m_spatialAnchor.uuidString}");
    }

    private void OnSaveCompleteCallback(YVRSpatialAnchorSaveCompleteInfo saveResult,bool success)
    {
        IsSavedLocally = success;
        LogController.Instance.Log($"Save anchor uuid:{m_spatialAnchor.uuidString} {success}");
    }

    public void OnHideButtonPressed()
    {
        LogController.Instance.Log("OnHideButtonPressed: hiding anchor");
        SpatialAnchorManager.Instance.anchorDic.Remove(m_spatialAnchor.uuidString);
        SpatialAnchorManager.Instance.RemoveShareAnchor(m_spatialAnchor.spaceHandle);
        SpatialAnchorManager.Instance.querySpaceIds.Remove(m_spatialAnchor.uuidString);
        Destroy(gameObject);
    }

    public void OnEraseButtonPressed()
    {
        LogController.Instance.Log("OnEraseButtonPressed: erasing anchor");
        if (m_spatialAnchor.spaceHandle == 0) return;

        YVRSpatialAnchor.instance.EraseSpatialAnchor(m_spatialAnchor.spaceHandle, YVRSpatialAnchorStorageLocation.Local,
            OnEraseCompleteCallback);
    }

    private void OnEraseCompleteCallback(YVRSpatialAnchorResult result,bool success)
    {
        LogController.Instance.Log($"Erase anchor uuid:{m_spatialAnchor.uuidString} {success}");
        if (success)
        {
            SpatialAnchorManager.Instance.anchorDic.Remove(m_spatialAnchor.uuidString);
            SpatialAnchorManager.Instance.RemoveShareAnchor(m_spatialAnchor.spaceHandle);
            SpatialAnchorManager.Instance.querySpaceIds.Remove(m_spatialAnchor.uuidString);
            Destroy(gameObject);
        }
    }

    private bool IsReadyToShare()
    {
        if (!Photon.Pun.PhotonNetwork.IsConnected)
        {
            LogController.Instance.Log(
                $"Can't share - no users to share with because you are no longer connected to the Photon network");
            return false;
        }

        var userIds = SpatialAnchorManager.Instance.userIds;
        if (userIds.Count == 0)
        {
            LogController.Instance.Log(
                $"Can't share - no users to share with or can't get the user ids through photon custom properties");
            return false;
        }

        if (m_spatialAnchor == null)
        {
            LogController.Instance.Log("Can't share - no associated spatial anchor");
            return false;
        }

        return true;
    }

    public void OnShareButtonPressed()
    {
        LogController.Instance.Log($"Share anchor uuid:{m_spatialAnchor.uuidString} start");
        if (!IsReadyToShare())
        {
            return;
        }

        // SaveToCloudThenShare();
        ShareAnchor();
    }

    private void ShareAnchor()
    {
        YVRSpatialAnchorShareInfo shareInfo = new YVRSpatialAnchorShareInfo();
        shareInfo.users = new ulong[SpatialAnchorManager.Instance.userIds.Count];
        for (int i = 0; i < shareInfo.users.Length; i++)
        {
            shareInfo.users[i] = SpatialAnchorManager.Instance.userIds[i];
        }

        shareInfo.anchorHandle = new [] { m_spatialAnchor.spaceHandle };
        YVRSpatialAnchor.instance.SaveToCloudThenShare(shareInfo, ShareAnchorCompleteCallback);
    }

    private void SaveToCloud()
    {
        YVRSpatialAnchorSaveInfo saveInfo = new YVRSpatialAnchorSaveInfo();
        saveInfo.anchorHandle = m_spatialAnchor.spaceHandle;
        saveInfo.storageLocation = YVRSpatialAnchorStorageLocation.Cloud;
        YVRSpatialAnchor.instance.SaveSpatialAnchor(saveInfo, SaveAnchorToCloudCallback);
        LogController.Instance.Log($"Start Save anchor to the cloud");
    }

    private void SaveAnchorToCloudCallback(YVRSpatialAnchorSaveCompleteInfo result,bool success)
    {
        LogController.Instance.Log($"Save anchor to the cloud result{success},resultCode:{result.resultCode}");
    }

    private void ShareAnchorCompleteCallback(bool result)
    {
        LogController.Instance.Log($"Share anchor uuid:{m_spatialAnchor.uuidString} result:{result}");
        if (!result)
        {
            this.shareIcon.color = Color.red;
            return;
        }

        IsSelectedForShare = true;
        SpatialAnchorManager.Instance.AddShareAnchor(m_spatialAnchor.spaceHandle);
        SpatialAnchorManager.Instance.PublishAnchorUuids(m_spatialAnchor.uuidString);
    }

    public void OnAlignButtonPressed()
    {
        LogController.Instance.Log("OnAlignButtonPressed: aligning to anchor");
        //TODO 将创建的物体绑定至对应锚点下
        SpawnNetworkCubeManager.Instance.AlignAnchor(this.m_spatialAnchor.uuidString);
    }

    public void DisableShareIcon()
    {
        if (shareButton)
        {
            shareButton.SetActive(false);
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