using System;
using UnityEngine;

namespace Common.Data
{
    [CreateAssetMenu(fileName = "Bool Variable", menuName = "Data/BoolVariable")]
    public class BoolVariable : ScriptableObject
    {
        [SerializeField] private bool boolValue;
        // (not used)
        //public bool Bool => boolValue;
        public bool defaultState;

        public event Action BoolChanged;

        public void Awake()
        {
            // TODO: Fix once we actually use this
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