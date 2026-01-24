using System;
using UnityEngine;

namespace Common.Data
{
    [CreateAssetMenu(fileName = "FloatWrapper", menuName = "Data/Float Wrapper")]
    public class FloatWrapper : ScriptableObject
    {
        [SerializeField] private float currentValue;
        [SerializeField] private float defaultValue;

        public event Action<float> FloatChanged;

        private void Awake()
        {
            currentValue = defaultValue;
        }

        public void Set(float value)
        {
            if (currentValue == value) return;
            currentValue = value;
            FloatChanged?.Invoke(currentValue);
        }

        public static implicit operator float(FloatWrapper value) => value.currentValue;
    }
}
