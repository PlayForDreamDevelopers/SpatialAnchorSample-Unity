using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeSyncController : MonoBehaviour
{
    public XRBaseInteractable xrInteractable;
    private PhotonView m_PhotonView;
    private bool isSelect;
    public void Awake()
    {
        xrInteractable.selectEntered.AddListener(CubeSelectCallback);
        xrInteractable.selectExited.AddListener(CubeUnSelectCallback);
        m_PhotonView = this.AddComponent<PhotonView>();
        m_PhotonView.ViewID = PhotonNetwork.ViewCount + 1;
    }

    public void Start()
    {
        Vector3 position = this.transform.localPosition;
        Quaternion rotation = this.transform.localRotation;
        m_PhotonView.RPC("SyncCubePose",RpcTarget.Others,position,rotation);
    }

    private void OnDestroy()
    {
        xrInteractable.selectEntered.RemoveListener(CubeSelectCallback);
        xrInteractable.selectExited.RemoveListener(CubeUnSelectCallback);
    }

    private void CubeSelectCallback(SelectEnterEventArgs selectEnterEventArgs)
    {
        isSelect = true;
    }

    private void CubeUnSelectCallback(SelectExitEventArgs selectExitEventArgs)
    {
        isSelect = false;
    }

    private void FixedUpdate()
    {
        if (isSelect)
        {
            Vector3 localPosition = SpawnNetworkCubeManager.Instance.cubeParent.transform.InverseTransformPoint(this.transform.position);
            Quaternion localRotation = Quaternion.Inverse(SpawnNetworkCubeManager.Instance.cubeParent.transform.rotation) * this.transform.rotation;
            m_PhotonView.RPC("SyncCubePose",RpcTarget.Others,localPosition,localRotation);
        }
    }

    [PunRPC]
    private void SyncCubePose(Vector3 position,Quaternion quaternion)
    {
        LogController.Instance.Log($"cube sync localPosition:{position}");
        this.transform.localPosition = position;
        this.transform.localRotation = quaternion;
    }
}
