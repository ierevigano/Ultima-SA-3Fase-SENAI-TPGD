using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Base interface for DialogueNode - Partial interface that gets extended by generated code
    /// </summary>
    public partial interface DialogueNode
    {
        string nodeID { get; }
        Character Speaker { get; }
        List<Character> Listeners { get; }
        string GetMessage(Language language);
    }
}

