using VisualNovelEngine;

public class SceneTransitionManager : VNESceneTransitionManager
{
    public static SceneTransitionManager Instance
    {
        get
        {
            return InstanceInternal as SceneTransitionManager;
        }
    }
}
