using System;
using UnityEngine;

namespace Common.Data
{
    [CreateAssetMenu(fileName = "Float Variable", menuName = "Data/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        public float FloatValue { get; private set; }
        [SerializeField] private float defaultValue;

        public event Action<float> FloatChanged;

        public void Awake()
        {
            FloatValue = defaultValue;
        }

        public void SetFloat(float value)
        {
            if (FloatValue == value) return;
            FloatValue = value;
            FloatChanged?.Invoke(FloatValue);
        }

        public static implicit operator float(FloatVariable value)
        {
            return value.FloatValue;
        }
    }
}
