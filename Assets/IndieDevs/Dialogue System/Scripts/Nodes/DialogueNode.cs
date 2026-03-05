using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Base interface for DialogueNode
    /// </summary>
    public partial interface DialogueNode
    {
        string nodeID { get; }
        Character Speaker { get; }
        List<Character> Listeners { get; }
        AudioClip Audio { get; }
        float Time { get; }
        string GetMessage(Language language);
    }

    /// <summary>
    /// Implementation of DialogueNode interface
    /// </summary>
    public partial class DialogueNodeInternal : DialogueNode
    {
        public string nodeID;
        public Character speaker;
        public List<Character> listeners = new List<Character>();
        public CustomFieldSO customFieldSO;

        public Character Speaker => speaker;
        public List<Character> Listeners => listeners;
        
        public string GetMessage(Language language)
        {
            // TODO: Implement message retrieval based on language
            return "";
        }
    }
}
