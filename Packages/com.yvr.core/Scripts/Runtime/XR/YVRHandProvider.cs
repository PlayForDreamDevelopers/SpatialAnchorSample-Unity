using System;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

#if XR_HANDS
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.ProviderImplementation;

namespace YVR.Core
{
    [Preserve]
    public class YVRHandProvider : XRHandSubsystemProvider
    {
        private bool m_IsValid;
        private static HandJointLocations m_LeftHandJointLocations;
        private static HandJointLocations m_RightHandJointLocations;

        internal static string id { get; private set; }

        static YVRHandProvider() => id = "YVR Hands";

        /// <summary>
        /// See <see cref="UnityEngine.SubsystemsImplementation.SubsystemProvider{T}.Start"/>.
        /// </summary>
        public override void Start() { }

        /// <summary>
        /// See <see cref="UnityEngine.SubsystemsImplementation.SubsystemProvider{T}.Stop"/>.
        /// </summary>
        public override void Stop() { }

        /// <summary>
        /// See <see cref="UnityEngine.SubsystemsImplementation.SubsystemProvider{T}.Destroy"/>.
        /// </summary>
        public override void Destroy() { }

        public override void GetHandLayout(NativeArray<bool> handJointsInLayout)
        {
            XRHandJointID[] allJoints = (XRHandJointID[])Enum.GetValues(typeof(XRHandJointID));

            foreach (XRHandJointID jointID in allJoints)
            {
                if (jointID == XRHandJointID.Invalid || jointID == XRHandJointID.BeginMarker ||
                    jointID == XRHandJointID.EndMarker)
                {
                    continue; // Skip markers
                }

                handJointsInLayout[jointID.ToIndex()] = true;
            }

            m_IsValid = true;
        }


        public override XRHandSubsystem.UpdateSuccessFlags TryUpdateHands(
            XRHandSubsystem.UpdateType updateType,
            ref Pose leftHandRootPose,
            NativeArray<XRHandJoint> leftHandJoints,
            ref Pose rightHandRootPose,
            NativeArray<XRHandJoint> rightHandJoints)
        {
            if (!m_IsValid)
                return XRHandSubsystem.UpdateSuccessFlags.None;

            YVRPlugin.Instance.GetHandJointLocations(HandType.HandLeft, ref m_LeftHandJointLocations);
            YVRPlugin.Instance.GetHandJointLocations(HandType.HandRight, ref m_RightHandJointLocations);
            ChangeJointsOrientation(m_LeftHandJointLocations);
            ChangeJointsOrientation(m_RightHandJointLocations);
            if (m_LeftHandJointLocations.jointLocations == null || m_RightHandJointLocations.jointLocations == null)
            {
                return XRHandSubsystem.UpdateSuccessFlags.None;
            }

            leftHandRootPose = new()
            {
                position = m_LeftHandJointLocations.jointLocations[(int)XRHandJointID.Wrist].pose.position,
                rotation = m_LeftHandJointLocations.jointLocations[(int)XRHandJointID.Wrist].pose.orientation
            };
            rightHandRootPose = new()
            {
                position = m_RightHandJointLocations.jointLocations[(int)XRHandJointID.Wrist].pose.position,
                rotation = m_RightHandJointLocations.jointLocations[(int)XRHandJointID.Wrist].pose.orientation
            };

            SetHandJointData(leftHandJoints, m_LeftHandJointLocations);
            SetHandJointData(rightHandJoints, m_RightHandJointLocations);
            XRHandSubsystem.UpdateSuccessFlags successFlags = XRHandSubsystem.UpdateSuccessFlags.None;

            successFlags |= m_LeftHandJointLocations.isActive == 1
                ? XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose | XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints
                : 0;
            successFlags |= m_RightHandJointLocations.isActive == 1
                ? XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose | XRHandSubsystem.UpdateSuccessFlags.RightHandJoints
                : 0;

            if (m_LeftHandJointLocations.isActive == 1 && m_RightHandJointLocations.isActive == 1)
            {
                successFlags = XRHandSubsystem.UpdateSuccessFlags.All;
            }

            return successFlags;
        }

        private void ChangeJointsOrientation(HandJointLocations handJointLocations)
        {
            for (int i = 0; i < handJointLocations.jointLocations.Length; i++)
            {
                Quaternion quaternion = handJointLocations.jointLocations[i].pose.orientation;
                handJointLocations.jointLocations[i].pose.orientation = new Quaternion(
                    quaternion.x, quaternion.y, quaternion.z, quaternion.w) * Quaternion.AngleAxis(-180f, Vector3.up);
            }
        }

        private void SetHandJointData(NativeArray<XRHandJoint> handJoints, HandJointLocations handJointLocations)
        {
            if (handJointLocations.isActive != 1) return;

            for (int i = 0; i < handJoints.Length; i++)
            {
                int index = (i == 0) ? 1 : (i == 1) ? 0 : i;
                Pose pose = new Pose();
                pose.position = handJointLocations.jointLocations[index].pose.position;
                pose.rotation = handJointLocations.jointLocations[index].pose.orientation;
                handJoints[i] = XRHandProviderUtility.CreateJoint(
                    Handedness.Left,
                    handJointLocations.isActive == 1
                        ? XRHandJointTrackingState.Radius | XRHandJointTrackingState.Pose |
                          XRHandJointTrackingState.LinearVelocity | XRHandJointTrackingState.AngularVelocity
                        : XRHandJointTrackingState.None,
                    (XRHandJointID)i + 1,
                    pose,
                    handJointLocations.jointLocations[index].radius,
                    handJointLocations.jointVelocities[index].linearVelocity,
                    handJointLocations.jointVelocities[index].angularVelocity
                );
            }
        }
    }
}

#endif