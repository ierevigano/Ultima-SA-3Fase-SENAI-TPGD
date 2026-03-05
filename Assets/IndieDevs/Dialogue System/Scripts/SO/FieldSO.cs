using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Represents a custom field in the dialogue system (Emotion, Pose, Position, Time, Audio).
    /// </summary>
    [CreateAssetMenu(fileName = "New Field", menuName = "Dialogue System/Field")]
    public class FieldSO : ScriptableObject
    {
        public string label;
        public string description;
    }
}
