using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using YVR.Core;
using PhotonPun = Photon.Pun;

public class SpatialAnchorManager : PhotonPun.MonoBehaviourPunCallbacks
{
    public GameObject anchorPrefab;

    public List<YVRSpatialAnchorResult> spaceResults = new List<YVRSpatialAnchorResult>();
    public Dictionary<string,GameObject> anchorDic = new Dictionary<string, GameObject>();

    private List<ulong> m_ShareSpaceHandleList = new List<ulong>();

    public PhotonPun.PhotonView photonView;
    private bool m_LoopQueryAnchor = true;
    private Coroutine m_LoopQueryCoroutine;
    private List<ulong> m_UserIds = new List<ulong>();
    public List<ulong> userIds
    {
        get
        {
            m_UserIds.Clear();
            foreach (var player in PhotonPun.PhotonNetwork.PlayerList.ToList())
            {
                ulong userId = default;
                if (ulong.TryParse(player.UserId,out userId))
                {
                    m_UserIds.Add(userId);
                }
            }

            return m_UserIds;
        }
    }

    public List<string> querySpaceIds = new List<string>();
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

    public void AddShareAnchor(ulong anchorHandle)
    {
        if (m_ShareSpaceHandleList.Contains(anchorHandle))
        {
            return;
        }

        m_ShareSpaceHandleList.Add(anchorHandle);
    }

    public void RemoveShareAnchor(ulong anchorHandle)
    {
        if (m_ShareSpaceHandleList.Contains(anchorHandle))
        {
            m_ShareSpaceHandleList.Remove(anchorHandle);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        LogController.Instance.Log($"new player enterd room user name:{newPlayer.NickName}, user id:{newPlayer.UserId}");
        ShareAllAnchor();
    }

    public void PublishAnchorUuids(string uuids)
    {
        LogController.Instance.Log($"PublishAnchorUuids uuid:{new string(uuids)}");
        photonView.RPC("CheckForAnchorsShared",PhotonPun.RpcTarget.All,uuids);
    }

    [PhotonPun.PunRPC]
    private void CheckForAnchorsShared(string uuids)
    {
        LogController.Instance.Log($"CheckForAnchorsShared uuid:{new string(uuids)}");
        if (!querySpaceIds.Contains(uuids))
        {
            querySpaceIds.Add(uuids);
        }

        if (m_LoopQueryCoroutine != null) StopCoroutine(m_LoopQueryCoroutine);

        m_LoopQueryCoroutine = StartCoroutine(LoopQueryAnchor());
    }

    private IEnumerator LoopQueryAnchor()
    {
        while (m_LoopQueryAnchor)
        {
            if (querySpaceIds.Count > 0)
            {
                QuerySpaces();
            }

            yield return new WaitForSeconds(1);
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
        LogController.Instance.Log($"CreateSpatialAnchor position:{pose.position} rotation:{pose.rotation}");
        LogController.Instance.Log("Create spatial anchor start");
        YVRSpatialAnchor.instance.CreateSpatialAnchor(pose.position, pose.rotation, OnCreateSpatialAnchor);
    }

    private void OnCreateSpatialAnchor(YVRSpatialAnchorResult result, bool success)
    {
        if(!success)
        {
            LogController.Instance.Log("Create spatial anchor failed" );
            return;
        }

        LogController.Instance.Log("OnCreateSpatialAnchor:" + new string(result.uuid));
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        YVRAnchorLocationFlags anchorLocationFlags = default;
        YVRSpatialAnchor.instance.GetSpatialAnchorPose(result.anchorHandle, out position, out rotation, out anchorLocationFlags);
        spaceResults.Add(result);
        querySpaceIds.Add(new string(result.uuid));
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

    public void QueryCloudSpaces()
    {
        YVRSpatialAnchorQueryInfo queryInfo = new YVRSpatialAnchorQueryInfo();
        queryInfo.storageLocation = YVRSpatialAnchorStorageLocation.Cloud;
        YVRSpatialAnchor.instance.QuerySpatialAnchor(queryInfo, QuerySpatialAnchorComplete);
    }

    public void QuerySpaces()
    {
        YVRSpatialAnchorQueryInfo queryInfo = new YVRSpatialAnchorQueryInfo();
        queryInfo.MaxQuerySpaces = 20;
        queryInfo.Timeout = 0;
        queryInfo.storageLocation = YVRSpatialAnchorStorageLocation.Cloud;
        queryInfo.ids = new YVRSpatialAnchorUUID[querySpaceIds.Count];

        for (int i = 0; i < querySpaceIds.Count; i++)
        {
            queryInfo.ids[i] = new YVRSpatialAnchorUUID();
            queryInfo.ids[i].Id = querySpaceIds[i].ToCharArray();
            Debug.Log($"QuerySpaces ids[{i}],uuid:{new string(queryInfo.ids[i].Id)}");
        }

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

    private void ShareAllAnchor()
    {
        YVRSpatialAnchorShareInfo shareInfo = new YVRSpatialAnchorShareInfo();
        shareInfo.users = new ulong[userIds.Count];
        for (int i = 0; i < shareInfo.users.Length; i++)
        {
            shareInfo.users[i] = userIds[i];
        }

        shareInfo.anchorHandle = new ulong[m_ShareSpaceHandleList.Count];
        for (int i = 0; i < shareInfo.anchorHandle.Length; i++)
        {
            shareInfo.anchorHandle[i] = m_ShareSpaceHandleList[i];
        }

        YVRSpatialAnchor.instance.ShareSpatialAnchor(shareInfo,ShareSpaceAnchorCallback);
    }

    private void ShareSpaceAnchorCallback(bool result)
    {
        if (result)
        {
            photonView.RPC("NotifyOtherPlayerShareAnchor",PhotonPun.RpcTarget.All,querySpaceIds.ToArray());
        }
    }

    [PhotonPun.PunRPC]
    private void NotifyOtherPlayerShareAnchor(string[] anchorUUIDArray)
    {
        for (int i = 0; i < anchorUUIDArray.Length; i++)
        {
            if (!querySpaceIds.Contains(anchorUUIDArray[i]));
            {
                querySpaceIds.Add(anchorUUIDArray[i]);
            }
        }

        if (m_LoopQueryCoroutine != null) StopCoroutine(m_LoopQueryCoroutine);

        m_LoopQueryCoroutine = StartCoroutine(LoopQueryAnchor());
    }

    public void GetEnumerateSpaceSupported(ulong anchorHandle)
    {
        YVRSpatialAnchorSupportedComponent supportedComponent = default;
        UInt64 space = anchorHandle;
        YVRSpatialAnchor.instance.GetSpatialAnchorEnumerateSupported(space, out supportedComponent);
        LogController.Instance.Log($"supportedComponent.component size:{supportedComponent.numComponents}");
        for (int i = 0; i < supportedComponent.components.Length; i++)
        {
            LogController.Instance.Log($"supportedComponent.component:{supportedComponent.components[i]}");
        }
    }
}