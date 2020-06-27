﻿using System;
using DynamicOpenVR.IO;
using UnityEngine;

namespace CustomAvatar.Tracking
{
    internal class VRPlayerInput : IAvatarInput, IDisposable
    {
        public event Action inputChanged;

        private readonly TrackedDeviceManager _deviceManager;

        private readonly SkeletalInput _leftHandAnimAction;
        private readonly SkeletalInput _rightHandAnimAction;

        internal VRPlayerInput(TrackedDeviceManager trackedDeviceManager)
        {
            _deviceManager = trackedDeviceManager ? trackedDeviceManager : throw new ArgumentNullException(nameof(trackedDeviceManager));

            _deviceManager.deviceAdded += OnDevicesUpdated;
            _deviceManager.deviceRemoved += OnDevicesUpdated;
            _deviceManager.deviceTrackingAcquired += OnDevicesUpdated;
            _deviceManager.deviceTrackingLost += OnDevicesUpdated;
            
            _leftHandAnimAction  = new SkeletalInput("/actions/customavatars/in/lefthandanim");
            _rightHandAnimAction = new SkeletalInput("/actions/customavatars/in/righthandanim");
        }

        public bool TryGetHeadPose(out Pose pose) => TryGetPose(_deviceManager.head, out pose);
        public bool TryGetLeftHandPose(out Pose pose) => TryGetPose(_deviceManager.leftHand, out pose);
        public bool TryGetRightHandPose(out Pose pose) => TryGetPose(_deviceManager.rightHand, out pose);
        public bool TryGetWaistPose(out Pose pose) => TryGetPose(_deviceManager.waist, out pose);
        public bool TryGetLeftFootPose(out Pose pose) => TryGetPose(_deviceManager.leftFoot, out pose);
        public bool TryGetRightFootPose(out Pose pose) => TryGetPose(_deviceManager.rightFoot, out pose);

        public bool TryGetLeftHandFingerCurl(out FingerCurl curl)
        {
            SkeletalSummaryData leftHandAnim = _leftHandAnimAction.summaryData;

            if (!_leftHandAnimAction.isActive || leftHandAnim == null)
            {
                curl = null;
                return false;
            }

            curl = new FingerCurl(leftHandAnim.thumbCurl, leftHandAnim.indexCurl, leftHandAnim.middleCurl, leftHandAnim.ringCurl, leftHandAnim.littleCurl);
            return true;
        }

        public bool TryGetRightHandFingerCurl(out FingerCurl curl)
        {
            SkeletalSummaryData rightHandAnim = _rightHandAnimAction.summaryData;

            if (!_rightHandAnimAction.isActive || rightHandAnim == null)
            {
                curl = null;
                return false;
            }

            curl = new FingerCurl(rightHandAnim.thumbCurl, rightHandAnim.indexCurl, rightHandAnim.middleCurl, rightHandAnim.ringCurl, rightHandAnim.littleCurl);
            return true;
        }

        public void Dispose()
        {
            _deviceManager.deviceAdded -= OnDevicesUpdated;
            _deviceManager.deviceRemoved -= OnDevicesUpdated;
            _deviceManager.deviceTrackingAcquired -= OnDevicesUpdated;
            _deviceManager.deviceTrackingLost -= OnDevicesUpdated;

            _leftHandAnimAction.Dispose();
            _rightHandAnimAction.Dispose();
        }

        private bool TryGetPose(TrackedDeviceState device, out Pose pose)
        {
            if (!device.found || !device.tracked)
            {
                pose = Pose.identity;
                return false;
            }

            pose = new Pose(device.position, device.rotation);
            return true;
        }

        private void OnDevicesUpdated(TrackedDeviceState state, DeviceUse use)
        {
            inputChanged?.Invoke();
        }
    }
}
