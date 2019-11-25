using System;
using CustomAvatar;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace AvatarScriptPack
{
    [Obsolete("Use VRIKManager")]
    class IKManager : MonoBehaviour
    {
        public Transform HeadTarget;
        public Transform LeftHandTarget;
        public Transform RightHandTarget;

        public void Start()
        {
            var vrikManager = gameObject.AddComponent<VRIKManager>();

            vrikManager.solver_spine_headTarget = HeadTarget;
            vrikManager.solver_leftArm_target = LeftHandTarget;
            vrikManager.solver_rightArm_target = RightHandTarget;
        }
    }
}
