using System;
using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(fileName = "Bool Variable", menuName = "Object Variables/Bool")]
    public class BoolVariable : ScriptableObject
    {
        [SerializeField] private bool boolValue;
        public bool Bool => boolValue;
        public bool defaultState;

        public event Action BoolChanged;

        public void Awake()
        {
            boolValue = defaultState;
        }

        public void ChangeBool(bool newBool)
        {
            if (boolValue == newBool) return;
            boolValue = newBool;
            BoolChanged?.Invoke();
        }

        public void ToggleBool()
        {
            boolValue = !boolValue;
            BoolChanged?.Invoke();
        }

        public static implicit operator bool(BoolVariable value)
        {
            return value != null && value.boolValue;
        }
    }
}