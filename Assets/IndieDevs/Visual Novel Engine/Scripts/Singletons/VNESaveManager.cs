using System;
using UnityEngine;

namespace VisualNovelEngine
{
    public class VNESaveManager : SingletonMonobehaviour<VNESaveManager>
    {
        public event Action OnSaveGame;
        public event Action OnLoadGame;

        /// <summary>
        /// Invokes OnSaveGame event and writes PlayerPrefs to disk.
        /// </summary>
        public virtual void SaveGame()
        {
            OnSaveGame?.Invoke();
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Invokes load event to restore saved state.
        /// </summary>
        public virtual void LoadGame()
        {
            OnLoadGame?.Invoke();
        }
    }
}
