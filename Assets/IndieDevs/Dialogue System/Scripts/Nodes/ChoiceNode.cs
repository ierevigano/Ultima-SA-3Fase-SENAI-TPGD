using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Base interface for ChoiceNode
    /// </summary>
    public partial interface ChoiceNode
    {
        string nodeID { get; }
        Character Speaker { get; }
        List<Character> Listeners { get; }
        AudioClip Audio { get; }
        float Time { get; }
        List<Choice> Choices { get; }
        string GetMessage(Language language);
    }

    /// <summary>
    /// Implementation of ChoiceNode interface
    /// </summary>
    public partial class ChoiceNodeInternal : ChoiceNode
    {
        public string nodeID;
        public Character speaker;
        public List<Character> listeners = new List<Character>();
        public List<Choice> choices = new List<Choice>();
        public CustomFieldSO customFieldSO;

        public Character Speaker => speaker;
        public List<Character> Listeners => listeners;
        public List<Choice> Choices => choices;

        public string GetMessage(Language language)
        {
            // TODO: Implement message retrieval based on language
            return "";
        }
    }
}
