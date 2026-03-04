using TMPro;
using UnityEngine;

namespace VisualNovelEngine
{
    public class VNEHistoryManager : SingletonMonobehaviour<VNEHistoryManager>
    {
        protected static string SAVE_HISTORY_KEY = "Save_History";

        [Header("History UI")]
        public GameObject historyObject;
        public GameObject content;
        public GameObject textPrefab;
        public GameObject dialogueUI;

        protected History history = new History();

        protected VNEGameManager gameManager;
        protected VNESaveManager saveManager;

        /// <summary>
        /// Initializes references and subscribes to game/save events.
        /// </summary>
        public void Start()
        {
            gameManager = VNEGameManager.InstanceInternal;
            saveManager = VNESaveManager.InstanceInternal;

            if (gameManager != null)
            {
                gameManager.OnNewGame += ClearHistory;
            }
            if (saveManager != null)
            {
                saveManager.OnSaveGame += Save;
                saveManager.OnLoadGame += Load;
            }
        }

        /// <summary>
        /// Unsubscribes from game/save events to prevent memory leaks.
        /// </summary>
        public void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnNewGame -= ClearHistory;
            }
            if (saveManager != null)
            {
                saveManager.OnSaveGame -= Save;
                saveManager.OnLoadGame -= Load;
            }
        }

        /// <summary>
        /// Saves current dialogue history.
        /// </summary>
        protected void Save()
        {
            string json = JsonUtility.ToJson(history);
            PlayerPrefs.SetString(SAVE_HISTORY_KEY, json);
        }

        /// <summary>
        /// Loads and initializes saved dialogue history.
        /// </summary>
        protected void Load()
        {
            ClearHistory();
            history = new History();

            string json = PlayerPrefs.GetString(SAVE_HISTORY_KEY, "");
            if (!string.IsNullOrEmpty(json))
            {
                History wrapper = JsonUtility.FromJson<History>(json);
                foreach (var entry in wrapper.list)
                {
                    history.list.Add(new HistoryItem(entry.speakerName, entry.dialogueText));

                    GameObject textObject = Instantiate(textPrefab);
                    textObject.transform.SetParent(content.transform, false);

                    TextMeshProUGUI[] texts = textObject.GetComponentsInChildren<TextMeshProUGUI>();
                    texts[0].text = entry.speakerName + ":";
                    texts[1].text = entry.dialogueText;
                }
            }
        }

        /// <summary>
        /// Displays the history panel and pauses the game.
        /// </summary>
        public void ShowHistory()
        {
            Time.timeScale = 0f;
            historyObject.SetActive(true);
            dialogueUI.SetActive(false);
        }

        /// <summary>
        /// Hides the history panel and resumes the game.
        /// </summary>
        public void HideHistory()
        {
            Time.timeScale = 1f;
            historyObject.SetActive(false);
            dialogueUI.SetActive(true);
        }

        /// <summary>
        /// Adds a dialogue entry to the history and UI.
        /// </summary>
        public void AddItemToHistory(HistoryItem item)
        {
            history.list.Add(item);

            GameObject textObject = Instantiate(textPrefab);
            textObject.transform.SetParent(content.transform, false);

            TextMeshProUGUI[] texts = textObject.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = item.speakerName + ":";
            texts[1].text = item.dialogueText;
        }

        /// <summary>
        /// Clears the history list and removes all UI entries.
        /// </summary>
        protected void ClearHistory()
        {
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
            history.list.Clear();
        }
    }
}
