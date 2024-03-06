using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using YVR.Core;

public class SpatialAnchorManager : MonoBehaviour
{
    public GameObject anchorPrefab;

    public List<YVRSpatialAnchorResult> spaceResults = new List<YVRSpatialAnchorResult>();
    public Dictionary<string,GameObject> anchorDic = new Dictionary<string, GameObject>();

    public Action createSpaceAction;

    public Transform anchorPose;
    public XRRayInteractor rightRayInteractor;

    public static SpatialAnchorManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        YVRPlugin.Instance.SetPassthrough(true);
    }

    private void Update()
    {
        if (YVRInput.GetDown(YVRInput.RawButton.RIndexTrigger) && !rightRayInteractor.IsOverUIGameObject())
        {
            CreateSpatialAnchor();
        }
    }


    private void UpdateAnchorTransform(char[] uuid, Vector3 position, Quaternion rotation)
    {
        GameObject anchor = anchorDic[new string(uuid)];
        anchor.SetActive(true);
        anchor.transform.position = position;
        anchor.transform.rotation = rotation;
    }

    public void CreateSpatialAnchor()
    {
        Pose pose = Pose.identity;
        pose.position = anchorPose.position;
        pose.rotation = anchorPose.rotation;
        Debug.Log($"CreateSpatialAnchor position:{pose.position} rotation:{pose.rotation}");
        Debug.Log("Create spatial anchor start");
        YVRSpatialAnchor.instance.CreateSpatialAnchor(pose.position, pose.rotation, OnCreateSpatialAnchor);
    }

    private void OnCreateSpatialAnchor(YVRSpatialAnchorResult result, bool success)
    {
        if(!success)
        {
            Debug.Log("Create spatial anchor failed" );
            return;
        }

        Debug.Log("OnCreateSpatialAnchor:" + new string(result.uuid));
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        YVRAnchorLocationFlags anchorLocationFlags = default;
        YVRSpatialAnchor.instance.GetSpatialAnchorPose(result.anchorHandle, out position, out rotation, out anchorLocationFlags);
        spaceResults.Add(result);
        CreateAnchorTransform(result,position, rotation);
        createSpaceAction?.Invoke();
    }

    private void CreateAnchorTransform(YVRSpatialAnchorResult result, Vector3 position, Quaternion rotation)
    {
        GameObject anchor = Instantiate(anchorPrefab, position, rotation);
        anchor.GetComponent<SpatialAnchorItem>().SetSpatialAnchorData(result.uuid,result.anchorHandle);
        anchor.GetComponent<AnchorController>().SetAnchorName(new string(result.uuid));
        anchor.SetActive(true);
        anchor.transform.localScale = Vector3.one;
        anchorDic.Add(new string(result.uuid),anchor);
    }

    public void QueryLocalSpaces()
    {
        YVRSpatialAnchorQueryInfo queryInfo = new YVRSpatialAnchorQueryInfo();
        queryInfo.storageLocation = YVRSpatialAnchorStorageLocation.Local;
        YVRSpatialAnchor.instance.QuerySpatialAnchor(queryInfo, QuerySpatialAnchorComplete);
    }

    private void QuerySpatialAnchorComplete(List<YVRSpatialAnchorResult> results)
    {
        Debug.Log("QuerySpacesComplete count:" + results.Count);
        foreach (var item in results)
        {
            if (!spaceResults.Exists(space => new string(space.uuid) == new string(item.uuid)))
            {
                spaceResults.Add(item);
            }

            Vector3 position = default;
            Quaternion rotation = default;
            YVRAnchorLocationFlags anchorLocationFlags = default;
            YVRSpatialAnchor.instance.GetSpatialAnchorPose(item.anchorHandle, out position, out rotation, out anchorLocationFlags);
            Debug.Log($"item.uuid:{new string(item.uuid)},position:{position},rotation:{rotation},anchorLocationFlags");
            if (!anchorDic.ContainsKey(new string(item.uuid)))
            {
                CreateAnchorTransform(item,position, rotation);
            }
            else
            {
                UpdateAnchorTransform(item.uuid,position, rotation);
            }
        }
    }
}