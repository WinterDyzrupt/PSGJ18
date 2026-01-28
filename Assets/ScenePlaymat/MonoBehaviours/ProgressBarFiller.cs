using System;
using Common.Data;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class ProgressBarFiller : MonoBehaviour
    {
        [SerializeField] private FloatWrapper floatVariable;

        private void Awake()
        {
            Debug.Assert(floatVariable != null, $"{name} is no assigned float variable in the inspector!");
            floatVariable.Changed += UpdateBar;

            UpdateBar(floatVariable);
        }

        private void OnDestroy()
        {
            floatVariable.Changed -= UpdateBar;
        }

        private void UpdateBar(float value)
        {
            transform.localScale = new(Mathf.Clamp01(value), 1, 1);
        }
    }
}
