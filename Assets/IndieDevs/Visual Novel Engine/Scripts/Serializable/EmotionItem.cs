using DialogueSystem;
using UnityEngine;

namespace VisualNovelEngine
{
    [System.Serializable]
    public class EmotionItem
    {
        [ReadOnly]
        public string emotionName;
        public GameObject emotionObject;
    }
}
