using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public AudioClip mainMenuBGAudio;
    public AudioClip firstDialogueBGAudio;

    private GameManager gameManager;
    private AudioManager audioManager;
    private SaveManager saveManager;
    private SceneTransitionManager sceneTransitionManager;

    /// <summary>
    /// Initializes references and plays main menu background music.
    /// </summary>
    private void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;
        saveManager = SaveManager.Instance;
        sceneTransitionManager = SceneTransitionManager.Instance;

        if (audioManager != null)
        {
            audioManager.PlayBGAudio(mainMenuBGAudio);
        }
    }

    /// <summary>
    /// Starts a new game, loads the specified scene, and plays the first dialogue BG music.
    /// </summary>
    public void OnNewGameClicked(string sceneName)
    {
        if (gameManager != null)
        {
            gameManager.NewGame();
        }
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.LoadScene(sceneName);
        }
        if (audioManager != null)
        {
            audioManager.PlayBGAudio(firstDialogueBGAudio);
        }
    }

    /// <summary>
    /// Loads the saved game state.
    /// </summary>
    public void OnLoadGameClicked()
    {
        if (saveManager != null)
        {
            saveManager.LoadGame();
        }
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public void OnQuitClicked()
    {
        if (gameManager != null)
        {
            gameManager.QuitGame();
        }
        else
        {
            Debug.LogWarning("GameManager is not initialized. Unable to quit.");
        }
    }
}