using Unity.Profiling;
using UnityEngine;

namespace YVR.Core
{
    public class HandInteractEffectController : MonoBehaviour
    {
        public GameObject pointer;
        public HandType handType;
        public YVRHand yvrHand;
        public SkinnedMeshRenderer handSkinnedMeshRenderer;
        private MaterialPropertyBlock m_HandMaterialPropertyBlock;
        private MaterialPropertyBlock m_PointerMaterialPropertyBlock;
        private MeshRenderer m_PointerMeshRenderer;
        private float m_CompressLimit = 0.48f;
        private float m_ScaleLimit = 0.2f;
        private float m_IndexFingerPinchStrength;
        private float m_IndexFingerPinchStrengthLimit = 0.9f;
        private float m_SoftMinValue = 0.319f;
        private float m_SoftMaxValue = 1.54f;
        private string m_SoftMin = "_SoftMin";
        private string m_SoftMax = "_SoftMax";
        private string m_ForceState = "_ForceState";
        private float m_ForceStateMax = 1;
        private float m_PinchFactor;
        private float m_PointerZOffset = 0.03f;
        private HandJointLocations m_HandJointLocations;
        private Transform m_PointerTransform;
        private static ProfilerMarker s_ProfilerMarker = new ProfilerMarker("HandInteractEffectController.Update");
        private int m_SoftMinPropertyID;
        private int m_SoftMaxPropertyID;
        private int m_ForceStatePropertyID;
        private void Start()
        {
            m_PointerMaterialPropertyBlock = new MaterialPropertyBlock();
            m_HandMaterialPropertyBlock = new MaterialPropertyBlock();
            m_PointerMeshRenderer = pointer.GetComponent<MeshRenderer>();
            m_PointerTransform = pointer.GetComponent<Transform>();
            m_SoftMinPropertyID = Shader.PropertyToID(m_SoftMin);
            m_SoftMaxPropertyID = Shader.PropertyToID(m_SoftMax);
            m_ForceStatePropertyID = Shader.PropertyToID(m_ForceState);
        }

        private void Update()
        {
            using (s_ProfilerMarker.Auto())
            {
                m_HandJointLocations = handType == HandType.HandLeft
                    ? YVRHandManager.instance.leftHandData
                    : YVRHandManager.instance.rightHandData;
                m_IndexFingerPinchStrength = YVRHandManager.instance.GetFingerPinchStrength(handType, XRHandFingerID.Index);
                m_PinchFactor = (1 - Mathf.Clamp(m_IndexFingerPinchStrength - m_CompressLimit, 0, 1 - m_CompressLimit) /
                    (1 - m_CompressLimit));
                UpdatePointerPose();
                UpdateHandEffect();
            }
        }

        private void UpdatePointerPose()
        {
            if (pointer == null) return;

            if ((((HandStatus)m_HandJointLocations.aimState.status & HandStatus.InputStateValid) != 0) &&
                m_HandJointLocations.isActive == 1)
            {
                pointer.SetActive(true);
                Vector3 pointerPosition =
                    (2 * yvrHand.handJoints[(int)HandJoint.JointThumbTip].position +
                     yvrHand.handJoints[(int)HandJoint.JointIndexTip].position) / 3;
                m_PointerTransform.position = pointerPosition + m_PointerTransform.up * m_PointerZOffset;
                if (m_IndexFingerPinchStrength >= m_CompressLimit)
                {
                    float pointerScale = m_ScaleLimit + m_PinchFactor;
                    m_PointerTransform.localScale = new Vector3(pointerScale, 1, pointerScale);
                    float softMin = m_IndexFingerPinchStrength >= m_IndexFingerPinchStrengthLimit
                        ? 0
                        : m_SoftMinValue * m_PinchFactor;
                    float softMax = m_IndexFingerPinchStrength >= m_IndexFingerPinchStrengthLimit
                        ? 0
                        : m_SoftMaxValue * m_PinchFactor;
                    m_PointerMaterialPropertyBlock.SetFloat(m_SoftMinPropertyID, softMin);
                    m_PointerMaterialPropertyBlock.SetFloat(m_SoftMaxPropertyID, softMax);
                    m_PointerMeshRenderer.SetPropertyBlock(m_PointerMaterialPropertyBlock);
                }
                else
                {
                    float pointerScale = Mathf.Clamp(m_PointerTransform.localScale.x + Time.deltaTime, 0, 1);
                    m_PointerTransform.localScale = new Vector3(pointerScale, 1, pointerScale);
                    m_PointerMaterialPropertyBlock.SetFloat(m_SoftMinPropertyID, m_SoftMinValue);
                    m_PointerMaterialPropertyBlock.SetFloat(m_SoftMaxPropertyID, m_SoftMaxValue);
                    m_PointerMeshRenderer.SetPropertyBlock(m_PointerMaterialPropertyBlock);
                }
            }
            else
            {
                pointer.SetActive(false);
            }
        }

        private void UpdateHandEffect()
        {
            if (handSkinnedMeshRenderer == null) return;

            if (m_IndexFingerPinchStrength >= m_CompressLimit)
            {
                m_HandMaterialPropertyBlock.SetFloat(m_ForceStatePropertyID, (1 - m_PinchFactor) * m_ForceStateMax);
            }
            else
            {
                float forceValue = Mathf.Clamp(m_HandMaterialPropertyBlock.GetFloat(m_ForceStatePropertyID) - Time.deltaTime * 2,
                    0, m_ForceStateMax);
                m_HandMaterialPropertyBlock.SetFloat(m_ForceStatePropertyID, forceValue);
            }

            handSkinnedMeshRenderer.SetPropertyBlock(m_HandMaterialPropertyBlock);
        }
    }
}