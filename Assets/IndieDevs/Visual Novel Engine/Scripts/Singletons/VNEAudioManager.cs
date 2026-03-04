using System.Collections;
using UnityEngine;

namespace VisualNovelEngine
{
    public class VNEAudioManager : SingletonMonobehaviour<VNEAudioManager>
    {
        protected static string SAVE_BG_AUDIO_KEY = "Save_BG_Audio";

        public AudioRegistrySO BGAudioRegistry;
        public float fadeDuration = 1f;
        public AudioSource BGAudioSource;
        public AudioSource FXAudioSource;

        protected Coroutine fadeCoroutine;
        protected SaveManager saveManager;

        /// <summary>
        /// Subscribes to Save and Load events from the SaveManager.
        /// </summary>
        public virtual void Start()
        {
            saveManager = SaveManager.Instance;

            if (saveManager != null)
            {
                saveManager.OnSaveGame += Save;
                saveManager.OnLoadGame += Load;
            }
        }

        /// <summary>
        /// Unsubscribes from Save and Load events to prevent memory leaks.
        /// </summary>
        public virtual void OnDisable()
        {
            if (saveManager != null)
            {
                saveManager.OnSaveGame -= Save;
                saveManager.OnLoadGame -= Load;
            }
        }

        /// <summary>
        /// Saves the name of the current background audio clip.
        /// </summary>
        protected virtual void Save()
        {
            PlayerPrefs.SetString(SAVE_BG_AUDIO_KEY, BGAudioSource.clip.name);
        }

        /// <summary>
        /// Loads the saved background audio clip and plays it.
        /// </summary>
        protected virtual void Load()
        {
            if (PlayerPrefs.HasKey(SAVE_BG_AUDIO_KEY))
            {
                string audioClipName = PlayerPrefs.GetString(SAVE_BG_AUDIO_KEY);

                AudioClip audioClip = BGAudioRegistry.GetAudioClipByName(audioClipName);

                if (audioClip != null)
                {
                    PlayBGAudio(audioClip);
                }
            }
        }

        /// <summary>
        /// Plays a one-shot sound effect clip.
        /// </summary>
        public virtual void PlayFXAudio(AudioClip clip)
        {
            if (clip != null)
            {
                FXAudioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning("Passed AudioClip is null!");
            }
        }

        /// <summary>
        /// Plays a new background audio clip with fade transition.
        /// </summary>
        public virtual void PlayBGAudio(AudioClip audioClip)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeToNewClip(BGAudioSource, audioClip));
        }

        /// <summary>
        /// Fades out the current clip, switches to a new one, and fades it in.
        /// </summary>
        protected virtual IEnumerator FadeToNewClip(AudioSource audioSource, AudioClip audioClip)
        {
            float volume = audioSource.volume;

            if (audioSource.isPlaying)
            {
                // Fade out
                yield return Fade(audioSource, volume, 0f, fadeDuration);
                audioSource.Stop();
            }

            // Switch clip
            audioSource.clip = audioClip;
            if (audioClip != null)
            {
                audioSource.Play();
                // Fade In
                yield return Fade(audioSource, 0f, volume, fadeDuration);
            }
            fadeCoroutine = null;
        }

        /// <summary>
        /// Smoothly transitions an audio source's volume from one value to another.
        /// </summary>
        protected virtual IEnumerator Fade(AudioSource audioSource, float from, float to, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                audioSource.volume = Mathf.Lerp(from, to, elapsedTime / duration);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            audioSource.volume = to;
        }
    }
}
