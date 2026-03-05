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
            OnValueChanged();
        }
        public void OnPoseChanged(Enum value)
        {
            poseValue = (Pose)value;
            OnValueChanged();
        }
        public void OnPositionChanged(Enum value)
        {
            positionValue = (Position)value;
            OnValueChanged();
        }

        /// <summary>
        /// Called when a value changes. Override in partial class to implement custom behavior.
        /// </summary>
        partial void OnValueChanged();
#endif

        public Enum GetEnumValue(DialogueSystem.FieldSO fieldSO)
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
