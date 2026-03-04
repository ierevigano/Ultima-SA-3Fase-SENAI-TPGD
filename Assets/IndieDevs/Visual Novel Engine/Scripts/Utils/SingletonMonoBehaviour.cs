using UnityEngine;

namespace VisualNovelEngine
{
    public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        [SerializeField]
        private bool dontDestroyOnLoad = false;

        public static T InstanceInternal
        {
            get
            {
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
