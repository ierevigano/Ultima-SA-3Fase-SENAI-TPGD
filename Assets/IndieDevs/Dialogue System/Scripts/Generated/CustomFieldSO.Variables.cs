// Auto-generated code, do not modify by hand
using System;
using UnityEngine;

namespace DialogueSystem
{
    public partial class CustomFieldSO
    {
        [HideInInspector] public Emotion emotionValue;
        public Emotion EmotionValue => emotionValue;
        [HideInInspector] public Pose poseValue;
        public Pose PoseValue => poseValue;
        [HideInInspector] public Position positionValue;
        public Position PositionValue => positionValue;

#if UNITY_EDITOR
        public void OnEmotionChanged(Enum value)
        {
            emotionValue = (Emotion)value;
            Save();
        }
        public void OnPoseChanged(Enum value)
        {
            poseValue = (Pose)value;
            Save();
        }
        public void OnPositionChanged(Enum value)
        {
            positionValue = (Position)value;
            Save();
        }
#endif

        public Enum GetEnumValue(FieldSO fieldSO)
        {
            if (fieldSO.label == nameof(Emotion))
            {
                return EmotionValue;
            }
            if (fieldSO.label == nameof(Pose))
            {
                return PoseValue;
            }
            if (fieldSO.label == nameof(Position))
            {
                return PositionValue;
            }
            return null;
        }
    }
}
