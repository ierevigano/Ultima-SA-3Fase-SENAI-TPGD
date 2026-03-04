using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelEngine
{
    public class CharacterPortraitWithPoses : MonoBehaviour
    {
        public FieldSO poseField;
        public FieldSO emotionField;
        public List<PoseItem> poseItems = new List<PoseItem>();

        public void ShowPoseAndEmotion(string pose, string emotionName)
        {
            foreach (PoseItem poseItem in poseItems)
            {
                if (poseItem.poseObject != null)
                {
                    poseItem.poseObject.SetActive(poseItem.poseName == pose);
                }
                foreach (EmotionItem emotionItem in poseItem.emotionItems)
                {
                    if (emotionItem.emotionObject != null)
                    {
                        emotionItem.emotionObject.SetActive(emotionItem.emotionName == emotionName);
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (poseField != null && poseField.fieldType == CustomFieldType.Enum)
            {
                foreach (string pose in poseField.enumChoices)
                {
                    if (!poseItems.Exists(e => e.poseName == pose))
                    {
                        if (emotionField != null && emotionField.fieldType == CustomFieldType.Enum)
                        {
                            var emotionItems = new List<EmotionItem>();
                            foreach (string emotion in emotionField.enumChoices)
                            {
                                if (!emotionItems.Exists(e => e.emotionName == emotion))
                                {
                                    emotionItems.Add(new EmotionItem
                                    {
                                        emotionName = emotion,
                                        emotionObject = null
                                    });
                                }
                            }
                            emotionItems.RemoveAll(e => !emotionField.enumChoices.Contains(e.emotionName));
                            poseItems.Add(new PoseItem
                            {
                                poseName = pose,
                                emotionItems = emotionItems
                            });
                        }
                    }
                }
                poseItems.RemoveAll(e => !poseField.enumChoices.Contains(e.poseName));
            }
            else
            {
                poseItems.Clear();
            }
        }
#endif
    }
}