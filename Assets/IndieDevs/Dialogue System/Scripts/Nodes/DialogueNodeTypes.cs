using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Base interface for all node types
    /// </summary>
    public interface Node
    {
        string nodeID { get; }
    }

    /// <summary>
    /// Node that triggers a dialogue event
    /// </summary>
    public partial class EventNode : Node
    {
        public string nodeID;
        public DialogueEvent dialogueEvent;

        public DialogueEvent DialogueEvent => dialogueEvent;
    }

    /// <summary>
    /// Node for conditional branching
    /// </summary>
    public partial class IfNode : Node
    {
        public string nodeID;
        public int trueNodeIndex;
        public int falseNodeIndex;
    }

    /// <summary>
    /// Node that marks the end of a dialogue
    /// </summary>
    public partial class EndNode : Node
    {
        public string nodeID;
        public DialogueTreeSO nextDialogue;

        public DialogueTreeSO NextDialogue => nextDialogue;
    }

    /// <summary>
    /// Represents a dialogue event that can be invoked
    /// </summary>
    public class DialogueEvent
    {
        // Empty base class for dialogue events
    }

    /// <summary>
    /// Manager for dialogue events
    /// </summary>
    public class DialogueEventManager
    {
        public void Invoke(DialogueEvent dialogueEvent)
        {
            // TODO:Implement event invocation
        }
    }
}
