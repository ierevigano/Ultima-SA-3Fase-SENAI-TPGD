// Auto-generated code, do not modify by hand
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public partial class CustomFieldView
    {
        EnumField GetEnumField(FieldSO fieldSO) {
            if (fieldSO.label == nameof(Emotion))
            {
                EnumField enumField = new EnumField("Emotion", customFieldSO.EmotionValue);
                enumField.RegisterValueChangedCallback(evt => customFieldSO.OnEmotionChanged(evt.newValue));
                return enumField;
            }
            if (fieldSO.label == nameof(Pose))
            {
                EnumField enumField = new EnumField("Pose", customFieldSO.PoseValue);
                enumField.RegisterValueChangedCallback(evt => customFieldSO.OnPoseChanged(evt.newValue));
                return enumField;
            }
            if (fieldSO.label == nameof(Position))
            {
                EnumField enumField = new EnumField("Position", customFieldSO.PositionValue);
                enumField.RegisterValueChangedCallback(evt => customFieldSO.OnPositionChanged(evt.newValue));
                return enumField;
            }
            return null;
        }
    }
}
