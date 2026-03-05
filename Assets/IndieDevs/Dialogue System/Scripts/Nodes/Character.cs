using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Represents a character in the dialogue system
    /// </summary>
    public partial class Character
    {
        public string characterID;
        public string characterName;
        public Position position;
        public Emotion emotion;
        public Pose pose;

        public string CharacterName => characterName;
        public Position Position => position;
        public Emotion Emotion => emotion;
        public Pose Pose => pose;
    }
}
