using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Represents a complete dialogue tree containing dialogue nodes, choices, and events.
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue Tree", menuName = "Dialogue System/Dialogue Tree")]
    public class DialogueTreeSO : ScriptableObject
    {
        public string DialogueID;
        
        [TextArea(2, 5)]
        public string description;
        
        public List<DialogueNodeInternal> nodes = new List<DialogueNodeInternal>();
        
        public DialogueNodeInternal GetNodeByID(string nodeID)
        {
            foreach (var node in nodes)
            {
                if (node != null && node.nodeID == nodeID)
                {
                    return node;
                }
            }
            return null;
        }
    }
}
