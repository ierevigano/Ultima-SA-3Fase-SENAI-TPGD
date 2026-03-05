using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Registry that holds and provides access to multiple dialogue trees by their ID.
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue Registry", menuName = "Dialogue System/Dialogue Registry")]
    public class DialogueRegistrySO : ScriptableObject
    {
        public List<DialogueTreeSO> dialogues = new List<DialogueTreeSO>();

        private Dictionary<string, DialogueTreeSO> dialoguesDictionary;

        /// <summary>
        /// Initializes the internal dictionary with dialogue IDs as keys.
        /// </summary>
        private void Initialize()
        {
            if (dialoguesDictionary != null) return;

            dialoguesDictionary = new Dictionary<string, DialogueTreeSO>();

            foreach (DialogueTreeSO dialogue in dialogues)
            {
                if (dialogue == null || string.IsNullOrEmpty(dialogue.DialogueID))
                {
                    continue;
                }

                if (dialoguesDictionary.ContainsKey(dialogue.DialogueID))
                {
                    Debug.LogWarning($"Warning: The Dialogue Registry '{name}' contains multiple references of '{dialogue.DialogueID}'. Ensure there are no duplicates.");
                }
                else
                {
                    dialoguesDictionary.Add(dialogue.DialogueID, dialogue);
                }
            }
        }

        /// <summary>
        /// Retrieves a dialogue tree by its ID.
        /// </summary>
        public DialogueTreeSO GetDialogueByID(string dialogueID)
        {
            Initialize();
            dialoguesDictionary.TryGetValue(dialogueID, out var result);
            return result;
        }

        /// <summary>
        /// Gets all registered dialogue trees.
        /// </summary>
        public List<DialogueTreeSO> GetAllDialogues()
        {
            return new List<DialogueTreeSO>(dialogues);
        }
    }
}
