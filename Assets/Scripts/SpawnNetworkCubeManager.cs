using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using YVR.Core;
using PhotonPun = Photon.Pun;

public class SpawnNetworkCubeManager : MonoBehaviour
{
    public XRRayInteractor leftRayInteractor;
    public GameObject cubePrefab;
    public GameObject cubeParent;
    public static SpawnNetworkCubeManager Instance;
    public Transform cubePose;
    public PhotonPun.PhotonView photonView;
    private string m_CurrentAlignAnchor;
    private List<GameObject> m_CacheCubeList = new List<GameObject>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (YVRInput.GetDown(YVRInput.RawButton.LIndexTrigger)&& !leftRayInteractor.IsOverUIGameObject())
        {
            if (!PhotonPun.PhotonNetwork.InRoom)
            {
                LogController.Instance.Log($"Create network cube after entering the room");
                return;
            }
            Vector3 position = cubePose.position;
            Quaternion rotation = cubePose.rotation;
            photonView.RPC("CreateCube",PhotonPun.RpcTarget.All,position,rotation);
        }

        FindAlignAnchor();
    }

    [PhotonPun.PunRPC]
    public void CreateCube(Vector3 position, Quaternion roation)
    {
         GameObject cube = Instantiate(cubePrefab,position,roation,cubeParent.transform);
         cube.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
         cube.SetActive(true);
         m_CacheCubeList.Add(cube);
         Debug.Log($"create cube position:{cube.transform.position}");
    }

    public void AlignAnchor(string uuid)
    {
        photonView.RPC("ShareAlignTarget",PhotonPun.RpcTarget.All,uuid);
    }

    [PhotonPun.PunRPC]
    private void ShareAlignTarget(string uuid)
    {
        m_CurrentAlignAnchor = uuid;
        LogController.Instance.Log($"Share align anchor uuid:{uuid}");
        if(!SpatialAnchorManager.Instance.anchorDic.ContainsKey(m_CurrentAlignAnchor))
        {
            LogController.Instance.Log("Can't find align anchor");
        }
    }

    private void FindAlignAnchor()
    {
        if (!string.IsNullOrEmpty(m_CurrentAlignAnchor) &&
            SpatialAnchorManager.Instance.anchorDic.ContainsKey(m_CurrentAlignAnchor))
        {
            cubeParent.transform.position = SpatialAnchorManager.Instance.anchorDic[m_CurrentAlignAnchor].transform.position;
            cubeParent.transform.rotation = SpatialAnchorManager.Instance.anchorDic[m_CurrentAlignAnchor].transform.rotation;
        }
    }
}
