using UnityEngine;

namespace VisualNovelEngine
{
    /// <summary>
    /// A serializable dictionary that can be used in Unity Inspector
    /// </summary>
    [System.Serializable]
    public class DictionarySerializable<K, V> : System.Collections.Generic.Dictionary<K, V>
    {
        // This allows the dictionary to be serialized by Unity
    }
}
