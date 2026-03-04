using UnityEngine;

public class TheEndMenu : MonoBehaviour
{
    private SceneTransitionManager sceneTransitionManager;

    private void Start()
    {
        sceneTransitionManager = SceneTransitionManager.Instance;
    }

    /// <summary>
    /// Loads the specified scene (the MainMenu) when clicked.
    /// </summary>
    public void OnMainMenuClicked(string sceneName)
    {
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.LoadScene(sceneName);
        }
    }
}
