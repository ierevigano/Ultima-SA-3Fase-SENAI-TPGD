using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public float timeBeforeStartDialogue = 1f;

    private VNDialogueUIManager dialogueUIManager;

    private IEnumerator Start()
    {
        dialogueUIManager = VNDialogueUIManager.Instance;

        yield return new WaitForSeconds(timeBeforeStartDialogue);

        if (dialogueUIManager != null)
        {
            dialogueUIManager.StartDialogue();
        }
        else
        {
            Debug.LogWarning("VNDialogueUIManager.Instance is null. DialogueTrigger cannot start dialogue.");
        }
    }
}
