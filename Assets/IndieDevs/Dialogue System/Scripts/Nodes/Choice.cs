using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Represents a single choice option in a ChoiceNode
    /// </summary>
    public partial class Choice
    {
        public string choiceID;
        public int nextNodeIndex;
        public CustomFieldSO customFieldSO;

        public string GetMessage(Language language)
        {
            // TODO: Implement message retrieval based on language
            return "";
        }
    }
}
