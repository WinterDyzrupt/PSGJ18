using System;
using UnityEngine;

namespace Common.Data
{
    [CreateAssetMenu(fileName = "BoolWrapper", menuName = "Data/Bool Wrapper")]
    public class BoolWrapper : ScriptableObject
    {
        [SerializeField] private bool currentValue;
        [SerializeField] public bool defaultValue;

        public event Action BoolChanged;

        private void Awake()
        {
            currentValue = defaultValue;
        }

        public void Set(bool newBool)
        {
            if (currentValue == newBool) return;
            currentValue = newBool;
            BoolChanged?.Invoke();
        }

        public void Toggle()
        {
            currentValue = !currentValue;
            BoolChanged?.Invoke();
        }

        public static implicit operator bool(BoolWrapper value) => value.currentValue;
    }
}