// Auto-generated code, do not modify by hand
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public partial class CharacterInternal
    {
        public Emotion Emotion => customFieldSO.GetCustomFieldValue<Emotion>(DialogueFields.Emotion);
        public Pose Pose => customFieldSO.GetCustomFieldValue<Pose>(DialogueFields.Pose);
        public Position Position => customFieldSO.GetCustomFieldValue<Position>(DialogueFields.Position);
    }
}
