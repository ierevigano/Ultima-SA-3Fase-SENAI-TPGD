using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Base class for dialogue UI systems. Override methods to implement custom UI behavior.
    /// </summary>
    public class DialogueUIManager : MonoBehaviour
    {
        protected DialogueManager dialogueManager;

        protected virtual void OnEnable()
        {
            FindDialogueManager();
        }

        /// <summary>
        /// Finds and caches the DialogueManager in the scene.
        /// </summary>
        protected void FindDialogueManager()
        {
            if (dialogueManager == null)
            {
                dialogueManager = FindObjectOfType<DialogueManager>();
            }
        }

        /// <summary>
        /// Called when a dialogue node is encountered.
        /// Override this in derived classes to handle dialogue display.
        /// </summary>
        public virtual void OnDialogueNode(DialogueNode dialogueNode)
        {
            // Override in derived classes
        }

        /// <summary>
        /// Called when a choice node is encountered.
        /// Override this in derived classes to handle choice display.
        /// </summary>
        public virtual void OnChoiceNode(ChoiceNode choiceNode)
        {
            // Override in derived classes
        }

        /// <summary>
        /// Called when an event node is encountered.
        /// </summary>
        public virtual void OnEventNode(EventNode eventNode, DialogueEventManager dialogueEventManager)
        {
            // Override in derived classes
        }

        /// <summary>
        /// Called when an if node is encountered.
        /// </summary>
        public virtual void OnIfNode(IfNode ifNode)
        {
            // Override in derived classes
        }

        /// <summary>
        /// Called when an end node is encountered.
        /// </summary>
        public virtual void OnEndNode(EndNode endNode)
        {
            // Override in derived classes
        }
    }
}
