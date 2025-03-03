using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerTip : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public GameObject tipObj;

    // Update is called once per frame
    void Update()
    {
        tipObj.SetActive(!rayInteractor.IsOverUIGameObject());
    }
}