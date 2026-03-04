using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelEngine
{
    public class CharacterPortrait : MonoBehaviour
    {
        public FieldSO emotionField;
        public List<EmotionItem> emotionItems = new List<EmotionItem>();

        public void ShowEmotion(string emotionName)
        {
            foreach (EmotionItem emotion in emotionItems)
            {
                if (emotion.emotionObject != null)
                {
                    emotion.emotionObject.SetActive(emotion.emotionName == emotionName);
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (emotionField != null && emotionField.fieldType == CustomFieldType.Enum)
            {
                foreach (string item in emotionField.enumChoices)
                {
                    if (!emotionItems.Exists(e => e.emotionName == item))
                    {
                        emotionItems.Add(new EmotionItem
                        {
                            emotionName = item,
                            emotionObject = null
                        });
                    }
                }
                emotionItems.RemoveAll(e => !emotionField.enumChoices.Contains(e.emotionName));
            }
            else
            {
                emotionItems.Clear();
            }
        }
#endif
    }
}