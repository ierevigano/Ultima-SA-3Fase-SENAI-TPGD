using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelEngine
{
    [CreateAssetMenu(fileName = "New Audio Registry", menuName = "Visual Novel Engine/Audio Registry")]
    public class AudioRegistrySO : ScriptableObject
    {
        public List<AudioClip> sounds;

        private Dictionary<string, AudioClip> soundsDictionary;

        /// <summary>
        /// Initializes the internal dictionary with AudioClip names as keys.
        /// </summary>
        private void Initialize()
        {
            if (soundsDictionary != null) return;

            soundsDictionary = new Dictionary<string, AudioClip>();

            foreach (AudioClip audioClip in sounds)
            {
                if (audioClip == null || string.IsNullOrEmpty(audioClip.name))
                {
                    continue;
                }

                if (soundsDictionary.ContainsKey(audioClip.name))
                {
                    Debug.LogWarning($"Warning: The Audio Registry '{name}' contains multiple references of '{audioClip.name}'. Ensure there are no duplicates in the '{name}'.");
                }
                else
                {
                    soundsDictionary.Add(audioClip.name, audioClip);
                }
            }
        }

        /// <summary>
        /// Retrieves an AudioClip by its name if it exists.
        /// </summary>
        public AudioClip GetAudioClipByName(string name)
        {
            Initialize();
            soundsDictionary.TryGetValue(name, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether a clip with the given name exists in the registry.
        /// </summary>
        public bool ContainsName(string name)
        {
            Initialize();
            return soundsDictionary.ContainsKey(name);
        }
    }
}
