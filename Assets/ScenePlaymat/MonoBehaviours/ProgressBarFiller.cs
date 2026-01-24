using System;
using Common.Data;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class ProgressBarFiller : MonoBehaviour
    {
        [SerializeField] private FloatWrapper floatVariable;

        private void Start()
        {
            Debug.Assert(floatVariable != null, $"{name} is no assigned float variable in the inspector!");

            UpdateBar(floatVariable);
        }

        private void OnEnable()
        {
            floatVariable.FloatChanged += UpdateBar;
        }

        private void OnDisable()
        {
            floatVariable.FloatChanged -= UpdateBar;
        }

        private void UpdateBar(float value)
        {
            transform.localScale = new(Mathf.Clamp01(value), 1, 1);
        }
    }
}
