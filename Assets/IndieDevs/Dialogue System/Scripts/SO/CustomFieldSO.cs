using System;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Holds custom field values for dialogue nodes (Emotion, Pose, Position, Time, Audio)
    /// </summary>
    public partial class CustomFieldSO : ScriptableObject
    {
        public T GetCustomFieldValue<T>(FieldSO field) where T : class
        {
            if (field == null) return null;

            // TODO: Implement field value retrieval
            return null;
        }

        public object GetCustomFieldValue(FieldSO field)
        {
            if (field == null) return null;

            // TODO: Implement generic field value retrieval
            return null;
        }
    }
}
