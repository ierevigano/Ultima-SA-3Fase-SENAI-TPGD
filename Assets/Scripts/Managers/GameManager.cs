using DialogueSystem;
using VisualNovelEngine;

public class GameManager : VNEGameManager
{
    public Language language = Language.English;

    public static GameManager Instance
    {
        get
        {
            return InstanceInternal as GameManager;
        }
    }
}
