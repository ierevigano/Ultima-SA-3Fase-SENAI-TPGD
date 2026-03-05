using DialogueSystem;
using System.Collections;
using TMPro;
using UnityEngine;
using VisualNovelEngine;

public class VNDialogueUIManager : DialogueUIManager
{
    private static string SAVE_DIALOGUE_ID_KEY = "Save_Dialogue_ID";
    private static string SAVE_DIALOGUE_NODE_ID_KEY = "Save_Dialogue_Node_ID";

    public bool dontDestroyOnLoad = true;
    public DialogueTreeSO defaultDialogue;
    public DialogueRegistrySO dialogueRegistry;

    [Header("Saving parameters")]
    public GameObject savingText;

    [Header("Auto parameters")]
    public float defaultWaitTime = 3f;
    public ToggleButton autoBtn;
    private bool isAutoEnabled => autoBtn.IsToggledOn;

    [Header("Typewriter parameters")]
    public bool isTypewriterEnabled = true;
    public float typeWriterSpeed = 0.02f;

    [Header("Dialogue UI")]
    public GameObject dialogueUI;
    public GameObject leftPortrait;
    public GameObject rightPortrait;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    [Header("Choice UI")]
    public GameObject choicesContainer;
    public GameObject choicePrefab;

    public DictionarySerializable<string, GameObject> characters;

    private DialogueTreeSO currentDialogue;
    private string currentNodeID;
    private Coroutine waitCoroutine;
    private Coroutine typewriterCoroutine;

    private string leftCharacter;
    private string rightCharacter;
    private bool isShowingLeftCharacter = false;
    private bool isShowingRightCharacter = false;

    private GameManager gameManager;
    private AudioManager audioManager;
    private SaveManager saveManager;
    private SceneTransitionManager sceneTransitionManager;
    private HistoryManager historyManager;

    private int coroutinesRunning = 0;

    public static VNDialogueUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets up references and subscribes to necessary events.
    /// </summary>
    private void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;
        saveManager = SaveManager.Instance;
        sceneTransitionManager = SceneTransitionManager.Instance;
        historyManager = HistoryManager.Instance;
        
        // Initialize dialogue manager from base class
        FindDialogueManager();
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        if (gameManager != null)
        {
            gameManager.OnNewGame += ResetDialogueUI;
        }
        if (saveManager != null)
        {
            saveManager.OnSaveGame += Save;
            saveManager.OnLoadGame += Load;
        }
    }

    /// <summary>
    /// Subscribes to auto button toggle event.
    /// </summary>
    private void OnEnable()
    {
        if (autoBtn != null)
        {
            autoBtn.OnToggleChanged += OnAutoToggle;
        }
    }

    /// <summary>
    /// Unsubscribes from all events to avoid memory leaks.
    /// </summary>
    private void OnDisable()
    {
        if (autoBtn != null)
        {
            autoBtn.OnToggleChanged -= OnAutoToggle;
        }
        if (gameManager != null)
        {
            gameManager.OnNewGame -= ResetDialogueUI;
        }
        if (saveManager != null)
        {
            saveManager.OnSaveGame -= Save;
            saveManager.OnLoadGame -= Load;
        }
    }

    /// <summary>
    /// Saves the current dialogue and node ID.
    /// </summary>
    private void Save()
    {
        PlayerPrefs.SetString(SAVE_DIALOGUE_ID_KEY, currentDialogue.DialogueID);
        PlayerPrefs.SetString(SAVE_DIALOGUE_NODE_ID_KEY, dialogueManager.CurrentNode.nodeID);
    }

    /// <summary>
    /// Loads the saved dialogue and node id.
    /// </summary>
    private void Load()
    {
        string key = SAVE_DIALOGUE_ID_KEY;
        if (PlayerPrefs.HasKey(key))
        {
            string dialogueID = PlayerPrefs.GetString(key);
            DialogueTreeSO dialogue = dialogueRegistry.GetDialogueByID(dialogueID);
            if (dialogue != null)
            {
                currentDialogue = dialogue;
            }
        }

        key = SAVE_DIALOGUE_NODE_ID_KEY;
        if (PlayerPrefs.HasKey(key))
        {
            currentNodeID = PlayerPrefs.GetString(key);
        }
    }

    private void OnAutoToggle(bool toggle)
    {
        if (toggle)
        {
            StartWaitCoroutine();
        }
        else if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
        }
    }

    public void ExitDialogue(string sceneName)
    {
        ResetDialogueUI();
        dialogueManager.ExitDialogue();
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.LoadScene(sceneName);
        }
    }

    public void OnSaveClicked()
    {
        if (saveManager != null)
        {
            saveManager.SaveGame();
            StartCoroutine(ShowSavingText());
        }
    }

    public void OnNextClicked()
    {
        if (dialogueManager.CurrentNode is not ChoiceNode && coroutinesRunning == 0)
        {
            NextNode();
        }
    }

    /// <summary>
    /// Starts a new dialogue.
    /// </summary>
    public void StartDialogue()
    {
        if (!dialogueManager.DialogueStarted)
        {
            if (currentDialogue == null)
            {
                currentDialogue = defaultDialogue;
            }
            dialogueUI.SetActive(true);
            dialogueManager.StartDialogue(currentDialogue, currentNodeID);
            StartWaitCoroutine();
        }
        else
        {
            Debug.LogWarning("Dialogue is already running!");
        }
    }

    /// <summary>
    /// Advances to the next node in the dialogue tree.
    /// </summary>
    private void NextNode(int choice = -1)
    {
        if (dialogueManager.DialogueStarted)
        {
            AddToHistory(dialogueManager.CurrentNode);
            dialogueManager.NextNode(choice);
            StartWaitCoroutine();
        }
    }

    public override void OnDialogueNode(DialogueNode dialogueNode)
    {
        isShowingLeftCharacter = false;
        isShowingRightCharacter = false;

        SetupSpeaker(dialogueNode.Speaker);

        if (dialogueNode.Audio != null)
        {
            StartCoroutine(PlayFXAudio(dialogueNode.Audio));
        }
        if (gameManager != null)
        {
            string message = dialogueNode.GetMessage(gameManager.language);
            SetDialogueText(message);
        }

        foreach (Character listener in dialogueNode.Listeners)
        {
            if (listener != null)
            {
                StartCoroutine(SwapCharacterPortrait(
                    listener.CharacterName,
                    listener.Position,
                    listener.Emotion,
                    listener.Pose
                ));
            }
        }
        StartCoroutine(ClearCharacterPortraits());
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration = 0.25f)
    {
        yield return Fade(canvasGroup, 0f, 1f, duration);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, float duration = 0.25f)
    {
        yield return Fade(canvasGroup, 1f, 0f, duration);
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float from, float to, float duration = 0.25f)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    public override void OnChoiceNode(ChoiceNode choiceNode)
    {
        isShowingLeftCharacter = false;
        isShowingRightCharacter = false;

        SetupSpeaker(choiceNode.Speaker);

        if (choiceNode.Audio != null)
        {
            StartCoroutine(PlayFXAudio(choiceNode.Audio));
        }
        if (gameManager != null)
        {
            string message = choiceNode.GetMessage(gameManager.language);
            SetDialogueText(message);
        }

        foreach (Character listener in choiceNode.Listeners)
        {
            if (listener != null)
            {
                StartCoroutine(SwapCharacterPortrait(
                   listener.CharacterName,
                   listener.Position,
                   listener.Emotion,
                   listener.Pose
               ));
            }
        }

        if (gameManager != null)
        {
            for (int i = 0; i < choiceNode.Choices.Count; i++)
            {
                GameObject choice = Instantiate(choicePrefab);
                choice.transform.SetParent(choicesContainer.transform, false);

                int index = i;
                UnityEngine.UI.Button button = choice.GetComponent<UnityEngine.UI.Button>();
                button.onClick.AddListener(() => OnChoiceClick(index, choiceNode.Choices[index]));

                TextMeshProUGUI textMeshPro = choice.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.text = choiceNode.Choices[index].GetMessage(gameManager.language);
            }
        }
        StartCoroutine(ClearCharacterPortraits());
    }

    private void OnChoiceClick(int index, Choice choice)
    {
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }
        NextNode(index);
        if (historyManager != null && gameManager != null)
        {
            HistoryItem historyItem = new HistoryItem("Player", choice.GetMessage(gameManager.language));
            historyManager.AddItemToHistory(historyItem);
        }
    }

    public override void OnEventNode(EventNode eventNode, DialogueEventManager dialogueEventManager)
    {
        dialogueEventManager.Invoke(eventNode.DialogueEvent);
        NextNode();
    }

    public override void OnIfNode(IfNode ifNode)
    {
        NextNode();
    }

    public override void OnEndNode(EndNode endNode)
    {
        ResetDialogueUI();
        currentDialogue = endNode.NextDialogue;
    }

    private IEnumerator ShowSavingText()
    {
        savingText.SetActive(true);
        yield return new WaitForSeconds(1f);
        savingText.SetActive(false);
    }

    private IEnumerator PlayFXAudio(AudioClip audioClip)
    {
        if (audioManager != null)
        {
            coroutinesRunning += 1;

            audioManager.PlayFXAudio(audioClip);
            yield return new WaitForSeconds(audioClip.length);

            coroutinesRunning -= 1;
        }
    }

    private void SetDialogueText(string message)
    {
        if (isTypewriterEnabled)
        {
            if (typewriterCoroutine != null)
            {
                StopCoroutine(typewriterCoroutine);
            }
            typewriterCoroutine = StartCoroutine(TypeText(message));
        }
        else
        {
            dialogueText.text = message;
        }
    }

    private IEnumerator TypeText(string message)
    {
        dialogueText.text = "";
        foreach (char letter in message)
        {
            dialogueText.text += letter;

            float elapsedTime = 0f;
            while (elapsedTime < typeWriterSpeed)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void AddToHistory(Node currentNode)
    {
        if (historyManager != null && gameManager != null)
        {
            string speakerName = "";
            string message = "";

            DialogueNode dialogueNode = currentNode as DialogueNode;
            ChoiceNode choiceNode = currentNode as ChoiceNode;

            if (dialogueNode != null)
            {
                if (dialogueNode.Speaker != null)
                {
                    speakerName = dialogueNode.Speaker.CharacterName;
                }
                message = dialogueNode.GetMessage(gameManager.language);
            }
            else if (choiceNode != null)
            {
                if (choiceNode.Speaker != null)
                {
                    speakerName = choiceNode.Speaker.CharacterName;
                }
                message = choiceNode.GetMessage(gameManager.language);
            }
            else
            {
                return;
            }

            if (speakerName == "")
                speakerName = "Narrator";
            HistoryItem historyItem = new HistoryItem(speakerName, message);
            historyManager.AddItemToHistory(historyItem);
        }
    }

    /// <summary>
    /// Sets up speaker name and portrait for dialogue or choice nodes.
    /// </summary>
    private void SetupSpeaker(Character speaker)
    {
        if (speaker == null)
        {
            speakerNameText.text = "";
        }
        else
        {
            speakerNameText.text = speaker.CharacterName;
            StartCoroutine(SwapCharacterPortrait(
                speaker.CharacterName,
                speaker.Position,
                speaker.Emotion,
                speaker.Pose
            ));
        }
    }

    private void StartWaitCoroutine()
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
        }
        if (isAutoEnabled && dialogueManager.CurrentNode is DialogueNode)
        {
            DialogueNode dialogueNode = dialogueManager.CurrentNode as DialogueNode;
            float time = (dialogueNode.Time == 0) ? defaultWaitTime : dialogueNode.Time;
            waitCoroutine = StartCoroutine(Wait(time));
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);

        NextNode();
    }

    private IEnumerator SwapCharacterPortrait(string characterName, Position position, Emotion emotion, DialogueSystem.Pose pose)
    {
        coroutinesRunning += 1;
        if (position == Position.Left)
        {
            isShowingLeftCharacter = true;
            if (leftCharacter != characterName)
            {
                if (leftCharacter != null)
                {
                    CanvasGroup leftCanvasGroup = leftPortrait.GetComponent<CanvasGroup>();
                    if (leftCanvasGroup != null)
                    {
                        yield return FadeOut(leftCanvasGroup);
                    }
                }
                leftCharacter = characterName;
                foreach (Transform child in leftPortrait.transform)
                {
                    Destroy(child.gameObject);
                }
                if (characters.ContainsKey(characterName))
                {
                    GameObject characterObject = Instantiate(characters[characterName], leftPortrait.transform);
                    SwapCharacterEmotionAndPose(characterObject, emotion, pose);
                    CanvasGroup leftCanvasGroup = leftPortrait.GetComponent<CanvasGroup>();
                    if (leftCanvasGroup != null)
                    {
                        yield return FadeIn(leftCanvasGroup);
                    }
                }
            }
            else if (leftPortrait.transform.childCount > 0)
            {
                GameObject characterObject = leftPortrait.transform.GetChild(0).gameObject;
                SwapCharacterEmotionAndPose(characterObject, emotion, pose);
            }
        }
        else if (position == Position.Right)
        {
            isShowingRightCharacter = true;
            if (rightCharacter != characterName)
            {
                if (rightCharacter != null)
                {
                    CanvasGroup rightCanvasGroup = rightPortrait.GetComponent<CanvasGroup>();
                    if (rightCanvasGroup != null)
                    {
                        yield return FadeOut(rightCanvasGroup);
                    }
                }
                rightCharacter = characterName;
                foreach (Transform child in rightPortrait.transform)
                {
                    Destroy(child.gameObject);
                }
                if (characters.ContainsKey(characterName))
                {
                    GameObject characterObject = Instantiate(characters[characterName], rightPortrait.transform);
                    SwapCharacterEmotionAndPose(characterObject, emotion, pose);
                    CanvasGroup rightCanvasGroup = rightPortrait.GetComponent<CanvasGroup>();
                    if (rightCanvasGroup != null)
                    {
                        yield return FadeIn(rightCanvasGroup);
                    }
                }
            }
            else if (rightPortrait.transform.childCount > 0)
            {
                GameObject characterObject = rightPortrait.transform.GetChild(0).gameObject;
                SwapCharacterEmotionAndPose(characterObject, emotion, pose);
            }
        }
        yield return null;
        coroutinesRunning -= 1;
    }

    private void SwapCharacterEmotionAndPose(GameObject characterObject, Emotion emotion, DialogueSystem.Pose pose)
    {
        CharacterPortrait characterPortrait = characterObject.GetComponent<CharacterPortrait>();
        if (characterPortrait != null)
        {
            characterPortrait.ShowEmotion(emotion.ToString());
        }
        CharacterPortraitWithPoses characterPortraitWithPoses = characterObject.GetComponent<CharacterPortraitWithPoses>();
        if (characterPortraitWithPoses != null)
        {
            characterPortraitWithPoses.ShowPoseAndEmotion(pose.ToString(), emotion.ToString());
        }
    }

    private IEnumerator ClearCharacterPortraits()
    {
        coroutinesRunning += 1;
        if (!isShowingLeftCharacter)
        {
            if (leftCharacter != null)
            {
                CanvasGroup leftCanvasGroup = leftPortrait.GetComponent<CanvasGroup>();
                if (leftCanvasGroup != null)
                {
                    yield return FadeOut(leftCanvasGroup);
                }
            }
            leftCharacter = null;
            foreach (Transform child in leftPortrait.transform)
            {
                Destroy(child.gameObject);
            }
        }
        if (!isShowingRightCharacter)
        {
            if (rightCharacter != null)
            {
                CanvasGroup rightCanvasGroup = rightPortrait.GetComponent<CanvasGroup>();
                if (rightCanvasGroup != null)
                {
                    yield return FadeOut(rightCanvasGroup);
                }
            }
            rightCharacter = null;
            foreach (Transform child in rightPortrait.transform)
            {
                Destroy(child.gameObject);
            }
        }
        yield return null;
        coroutinesRunning -= 1;
    }

    private void ResetDialogueUI()
    {
        currentDialogue = null;
        currentNodeID = null;
        leftCharacter = null;
        rightCharacter = null;
        dialogueUI.SetActive(false);

        StopAllCoroutines();
        coroutinesRunning = 0;

        foreach (Transform child in leftPortrait.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rightPortrait.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
