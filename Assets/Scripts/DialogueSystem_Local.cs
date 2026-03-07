using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    // ============= ENUMS =============
    public enum Language
    {
        English = 0,
        Portuguese = 1,
        Spanish = 2,
        French = 3,
        German = 4
    }

    public enum Position { Left, Center, Right }
    public enum Emotion { Neutral, Happy, Sad, Angry, Surprised }
    public enum Pose { Idle, Talking, Thinking }

    // ============= SCRIPTABLE OBJECTS =============
    [CreateAssetMenu(fileName = "New Field", menuName = "Dialogue System/Field")]
    public class FieldSO : ScriptableObject
    {
        public string label;
        public string description;
    }

    [CreateAssetMenu(fileName = "New Custom Field", menuName = "Dialogue System/Custom Field")]
    public class CustomFieldSO : ScriptableObject
    {
        public T GetCustomFieldValue<T>(FieldSO field) where T : class => null;
        public object GetCustomFieldValue(FieldSO field) => null;
    }

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
                if (node != null && node.nodeID == nodeID)
                    return node;
            return null;
        }
    }

    [CreateAssetMenu(fileName = "New Dialogue Registry", menuName = "Dialogue System/Dialogue Registry")]
    public class DialogueRegistrySO : ScriptableObject
    {
        public List<DialogueTreeSO> dialogues = new List<DialogueTreeSO>();

        public DialogueTreeSO GetDialogueByID(string dialogueID)
        {
            foreach (var d in dialogues)
                if (d != null && d.DialogueID == dialogueID)
                    return d;
            return null;
        }
    }

    // ============= INTERFACES =============
    public interface DialogueNode
    {
        string nodeID { get; }
        Character Speaker { get; }
        List<Character> Listeners { get; }
        string GetMessage(Language language);
    }

    public interface ChoiceNode
    {
        string nodeID { get; }
        Character Speaker { get; }
        List<Character> Listeners { get; }
        List<Choice> Choices { get; }
        string GetMessage(Language language);
    }

    public interface Node
    {
        string nodeID { get; }
    }

    // ============= CLASSES =============
    public class DialogueNodeInternal : DialogueNode
    {
        public string nodeID;
        public Character speaker;
        public List<Character> listeners = new List<Character>();
        public CustomFieldSO customFieldSO;
        public AudioClip Audio;
        public float Time;

        string DialogueNode.nodeID => nodeID;
        Character DialogueNode.Speaker => speaker;
        List<Character> DialogueNode.Listeners => listeners;
        public virtual string GetMessage(Language language) => "";
    }

    public class ChoiceNodeInternal : ChoiceNode
    {
        public string nodeID;
        public Character speaker;
        public List<Character> listeners = new List<Character>();
        public List<Choice> choices = new List<Choice>();
        public CustomFieldSO customFieldSO;
        public AudioClip Audio;
        public float Time;

        string ChoiceNode.nodeID => nodeID;
        Character ChoiceNode.Speaker => speaker;
        List<Character> ChoiceNode.Listeners => listeners;
        List<Choice> ChoiceNode.Choices => choices;
        public virtual string GetMessage(Language language) => "";
    }

    public class Choice
    {
        public string choiceID;
        public int nextNodeIndex;
        public CustomFieldSO customFieldSO;
        public string GetMessage(Language language) => "";
    }

    public partial class Character
    {
        public string characterID;
        public string characterName;
        public Position position;
        public Emotion emotion;
        public Pose pose;

        public string CharacterName => characterName;
        public Position Position => position;
        public Emotion Emotion => emotion;
        public Pose Pose => pose;
    }

    public class EventNode : Node
    {
        public string nodeID;
        public DialogueEvent dialogueEvent;
        public DialogueEvent DialogueEvent => dialogueEvent;
    }

    public class IfNode : Node
    {
        public string nodeID;
        public int trueNodeIndex;
        public int falseNodeIndex;
    }

    public class EndNode : Node
    {
        public string nodeID;
        public DialogueTreeSO nextDialogue;
        public DialogueTreeSO NextDialogue => nextDialogue;
    }

    public class DialogueEvent { }

    public class DialogueEventManager
    {
        public void Invoke(DialogueEvent dialogueEvent) { }
    }

    public class DialogueManager : MonoBehaviour
    {
        private DialogueTreeSO currentDialogueTree;
        private DialogueNodeInternal currentNode;
        private bool dialogueStarted = false;

        public DialogueNodeInternal CurrentNode => currentNode;
        public bool DialogueStarted => dialogueStarted;

        public void StartDialogue(DialogueTreeSO dialogueTree, string startNodeID = null)
        {
            if (dialogueTree == null) { Debug.LogError("Dialogue tree is null!"); return; }
            currentDialogueTree = dialogueTree;
            dialogueStarted = true;
            string nodeID = string.IsNullOrEmpty(startNodeID) ? (dialogueTree.nodes.Count > 0 ? dialogueTree.nodes[0].nodeID : "") : startNodeID;
            currentNode = dialogueTree.GetNodeByID(nodeID);
            if (currentNode == null) { Debug.LogError($"Node with ID '{nodeID}' not found"); dialogueStarted = false; }
        }

        public void NextNode(int choice = -1) { if (!dialogueStarted || currentNode == null) return; dialogueStarted = false; }

        public void ExitDialogue() { dialogueStarted = false; currentNode = null; currentDialogueTree = null; }
    }

    public class DialogueUIManager : MonoBehaviour
    {
        protected DialogueManager dialogueManager;

        protected virtual void OnEnable() { FindDialogueManager(); }

        protected void FindDialogueManager()
        {
            if (dialogueManager == null)
                dialogueManager = FindObjectOfType<DialogueManager>();
        }

        public virtual void OnDialogueNode(DialogueNode dialogueNode) { }
        public virtual void OnChoiceNode(ChoiceNode choiceNode) { }
        public virtual void OnEventNode(EventNode eventNode, DialogueEventManager dialogueEventManager) { }
        public virtual void OnIfNode(IfNode ifNode) { }
        public virtual void OnEndNode(EndNode endNode) { }
    }
}
