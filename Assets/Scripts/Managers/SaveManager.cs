using VisualNovelEngine;

public class SaveManager : VNESaveManager
{
    public static SaveManager Instance
    {
        get
        {
            return InstanceInternal as SaveManager;
        }
    }
}
