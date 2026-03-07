using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Base interface for ChoiceNode - Partial interface that gets extended by generated code
    /// </summary>
    public partial interface ChoiceNode
    {
        string nodeID { get; }
        Character Speaker { get; }
        List<Character> Listeners { get; }
        List<Choice> Choices { get; }
        string GetMessage(Language language);
    }
}

