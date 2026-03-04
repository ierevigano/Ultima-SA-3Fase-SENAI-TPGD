using VisualNovelEngine;

public class HistoryManager : VNEHistoryManager
{
    public static HistoryManager Instance
    {
        get
        {
            return InstanceInternal as HistoryManager;
        }
    }
}
