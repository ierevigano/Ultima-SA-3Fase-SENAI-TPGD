using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelEngine
{

    [System.Serializable]
    public class PoseItem
    {
        [ReadOnly]
        public string poseName;
        public GameObject poseObject;
        public List<EmotionItem> emotionItems;
    }
}
