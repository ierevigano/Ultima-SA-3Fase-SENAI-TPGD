using System;
using UnityEngine;

namespace VisualNovelEngine
{
    public class VNEGameManager : SingletonMonobehaviour<VNEGameManager>
    {
        public Action OnNewGame;

        /// <summary>
        /// Starts a new game and invokes all subscribed functions.
        /// </summary>
        public virtual void NewGame()
        {
            OnNewGame?.Invoke();
        }

        /// <summary>
        /// Quits the game application.
        /// </summary>
        public virtual void QuitGame()
        {
            Application.Quit();
        }
    }
}
