using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace VisualNovelEngine
{
    public class EventSystemManager : MonoBehaviour
    {
        void Awake()
        {
            EventSystem eventSystem = FindFirstObjectByType<EventSystem>();
            if (eventSystem == null)
            {
#if ENABLE_INPUT_SYSTEM
            GameObject newEventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            DontDestroyOnLoad(newEventSystem);
#else
                GameObject oldEventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                DontDestroyOnLoad(oldEventSystem);
#endif
            }
            else
            {
                DontDestroyOnLoad(eventSystem.gameObject);
            }
        }
    }
}
