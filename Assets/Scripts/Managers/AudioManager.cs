using VisualNovelEngine;

public class AudioManager : VNEAudioManager
{
    public static AudioManager Instance
    {
        get
        {
            return InstanceInternal as AudioManager;
        }
    }
}
