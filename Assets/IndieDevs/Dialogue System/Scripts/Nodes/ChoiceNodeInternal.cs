using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Base implementation of ChoiceNode that works with generated partial classes
    /// </summary>
    public partial class ChoiceNodeInternal : ChoiceNode
    {
        public string nodeID;
        public Character speaker;
        public List<Character> listeners = new List<Character>();
        public List<Choice> choices = new List<Choice>();
        public CustomFieldSO customFieldSO;

        string ChoiceNode.nodeID => nodeID;
        Character ChoiceNode.Speaker => speaker;
        List<Character> ChoiceNode.Listeners => listeners;
        List<Choice> ChoiceNode.Choices => choices;

        public virtual string GetMessage(Language language)
        {
            // TODO: Implement message retrieval based on language
            return "";
        }
    }
}
