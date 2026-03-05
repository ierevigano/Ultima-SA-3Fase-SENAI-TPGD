using System;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Manages dialogue execution, node progression, and state tracking.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        private DialogueTreeSO currentDialogueTree;
        private DialogueNodeInternal currentNode;
        private bool dialogueStarted = false;

        public DialogueNodeInternal CurrentNode => currentNode;
        public bool DialogueStarted => dialogueStarted;

        /// <summary>
        /// Starts a new dialogue from a specific node.
        /// </summary>
        public void StartDialogue(DialogueTreeSO dialogueTree, string startNodeID = null)
        {
            if (dialogueTree == null)
            {
                Debug.LogError("Dialogue tree is null!");
                return;
            }

            currentDialogueTree = dialogueTree;
            dialogueStarted = true;

            string nodeID = string.IsNullOrEmpty(startNodeID) ? dialogueTree.nodes[0].nodeID : startNodeID;
            currentNode = dialogueTree.GetNodeByID(nodeID);

            if (currentNode == null)
            {
                Debug.LogError($"Node with ID '{nodeID}' not found in dialogue tree '{dialogueTree.name}'");
                dialogueStarted = false;
            }
        }

        /// <summary>
        /// Advances to the next node in the dialogue tree.
        /// </summary>
        public void NextNode(int choice = -1)
        {
            if (!dialogueStarted || currentNode == null)
            {
                return;
            }

            // TODO: Implement dialogue progression logic based on node type
            dialogueStarted = false;
        }

        /// <summary>
        /// Exits the current dialogue.
        /// </summary>
        public void ExitDialogue()
        {
            dialogueStarted = false;
            currentNode = null;
            currentDialogueTree = null;
        }
    }
}
