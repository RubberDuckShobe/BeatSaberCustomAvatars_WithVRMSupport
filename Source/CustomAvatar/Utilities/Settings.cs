using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace CustomAvatar.Utilities
{
    // ReSharper disable ClassNeverInstantiated.Global
    // ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    // ReSharper disable RedundantDefaultMemberInitializer
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable UnusedMember.Global
    // ReSharper disable FieldCanBeMadeReadOnly.Global
    // ReSharper disable InconsistentNaming
    internal class Settings
    {
        public bool isAvatarVisibleInFirstPerson = true;
        [JsonConverter(typeof(StringEnumConverter))] public AvatarResizeMode resizeMode = AvatarResizeMode.Height;
        public bool enableFloorAdjust = false;
        public bool moveFloorWithRoomAdjust = false;
        public string previousAvatarPath = null;
        public float playerArmSpan = AvatarTailor.kDefaultPlayerArmSpan;
        public bool calibrateFullBodyTrackingOnStart = false;
        public float cameraNearClipPlane = 0.1f;
        public Mirror mirror { get; private set; } = new Mirror();
        public FullBodyMotionSmoothing fullBodyMotionSmoothing { get; private set; } = new FullBodyMotionSmoothing();
        [JsonProperty] private Dictionary<string, AvatarSpecificSettings> avatarSpecificSettings = new Dictionary<string, AvatarSpecificSettings>();
        
        public class Mirror
        {
            public Vector3 positionOffset = new Vector3(0, -1f, 0);
            public Vector2 size = new Vector2(5f, 4f);
            public float renderScale = 1.0f;
        }

        public class FullBodyMotionSmoothing
        {
            public TrackedPointSmoothing waist { get; private set; } = new TrackedPointSmoothing { position = 15, rotation = 10 };
            public TrackedPointSmoothing feet { get; private set; } = new TrackedPointSmoothing { position = 13, rotation = 17 };
        }

        public class TrackedPointSmoothing
        {
            public float position;
            public float rotation;
        }

        public class FullBodyCalibration
        {
            public Pose leftLeg = Pose.identity;
            public Pose rightLeg = Pose.identity;
            public Pose pelvis = Pose.identity;

            [JsonIgnore] public bool isDefault => leftLeg.Equals(Pose.identity) && rightLeg.Equals(Pose.identity) && pelvis.Equals(Pose.identity);
        }

        public class AvatarSpecificSettings
        {
            public FullBodyCalibration fullBodyCalibration { get; private set; } = new FullBodyCalibration();
            public bool useAutomaticCalibration = false;
        }

        public AvatarSpecificSettings GetAvatarSettings(string fullPath)
        {
            if (!avatarSpecificSettings.ContainsKey(fullPath))
            {
                avatarSpecificSettings.Add(fullPath, new AvatarSpecificSettings());
            }

            return avatarSpecificSettings[fullPath];
        }
    }
}
