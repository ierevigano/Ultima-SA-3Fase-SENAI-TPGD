using VisualNovelEngine;

public class MainCamera : VNEMainCamera
{
    public static MainCamera Instance
    {
        get
        {
            return InstanceInternal as MainCamera;
        }
    }
}
