using DialogueSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VisualNovelEngine
{
    public class VNESceneTransitionManager : SingletonMonobehaviour<VNESceneTransitionManager>
    {
        protected static string SAVE_SCENE_KEY = "Save_Scene";

        public Transition transition = Transition.Fade;
        [Range(1f, 3f)]
        public float animationDuration = 1f;
        public DictionarySerializable<Transition, GameObject> transitionObjects;

        protected bool isLoading = false;
        protected Animator animator;
        protected VNESaveManager saveManager;

        /// <summary>
        /// Initializes the transition animator and subscribes to save/load events.
        /// </summary>
        public virtual void Start()
        {
            saveManager = VNESaveManager.InstanceInternal;

            if (transitionObjects.ContainsKey(transition))
            {
                GameObject transitionObject = transitionObjects[transition];
                transitionObject.SetActive(true);
                animator = transitionObject.GetComponent<Animator>();
            }

            if (saveManager != null)
            {
                saveManager.OnSaveGame += Save;
                saveManager.OnLoadGame += Load;
            }
        }

        /// <summary>
        /// Unsubscribes from save/load events to prevent memory leaks.
        /// </summary>
        public virtual void OnDisable()
        {
            if (saveManager != null)
            {
                saveManager.OnSaveGame -= Save;
                saveManager.OnLoadGame -= Load;
            }
        }

        /// <summary>
        /// Saves the name of the currently active scene.
        /// </summary>
        protected virtual void Save()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            PlayerPrefs.SetString(SAVE_SCENE_KEY, sceneName);
        }

        /// <summary>
        /// Loads the saved scene if one exists.
        /// </summary>
        protected virtual void Load()
        {
            string key = SAVE_SCENE_KEY;
            if (PlayerPrefs.HasKey(key))
            {
                string sceneName = PlayerPrefs.GetString(key);
                LoadScene(sceneName);
            }
        }

        /// <summary>
        /// Starts loading a new scene with transition animation.
        /// </summary>
        public virtual void LoadScene(string sceneName)
        {
            if (!isLoading)
            {
                StartCoroutine(LoadSceneRoutine(sceneName));
            }
        }

        /// <summary>
        /// Handles scene loading and transition animation.
        /// </summary>
        protected IEnumerator LoadSceneRoutine(string sceneName)
        {
            isLoading = true;
            if (animator != null)
            {
                animator.SetTrigger("End");
                yield return new WaitForSeconds(animationDuration);
            }

            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            if (animator != null)
            {
                animator.SetTrigger("Start");
            }
            isLoading = false;
        }
    }
}
