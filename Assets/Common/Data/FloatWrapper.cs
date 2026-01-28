using System;
using UnityEngine;

namespace Common.Data
{
    [CreateAssetMenu(fileName = "FloatWrapper", menuName = "Data/Float Wrapper")]
    public class FloatWrapper : ScriptableObject
    {
        [SerializeField] private float value;
        public float Value => value; 
        
        [SerializeField] private float defaultValue;
        public float DefaultValue => defaultValue;

        public event Action<float> Changed;

        private void OnEnable()
        {
            // Reset for re-running the same scene in the editor.
            value = defaultValue;
        }

        public void Set(float newValue)
        {
            if (value == newValue) return;
            value = newValue;
            Changed?.Invoke(value);
        }

        public static implicit operator float(FloatWrapper value) => value.Value;
    }
}
