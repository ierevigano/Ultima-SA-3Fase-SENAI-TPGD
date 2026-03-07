using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Base implementation of DialogueNode that works with generated partial classes
    /// </summary>
    public partial class DialogueNodeInternal : DialogueNode
    {
        public string nodeID;
        public Character speaker;
        public List<Character> listeners = new List<Character>();
        public CustomFieldSO customFieldSO;

        string DialogueNode.nodeID => nodeID;
        Character DialogueNode.Speaker => speaker;
        List<Character> DialogueNode.Listeners => listeners;

        public virtual string GetMessage(Language language)
        {
            // TODO: Implement message retrieval based on language
            return "";
        }
    }
}
