using Common.Data;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class ProgressBarFiller : MonoBehaviour
    {
        [SerializeField] private FloatVariable floatVariable;

        private void Start()
        {
            Debug.Assert(floatVariable != null, $"{name} is no assigned float variable in the inspector!");

            floatVariable.FloatChanged += UpdateBar;
            UpdateBar(floatVariable.FloatValue);
        }

        private void UpdateBar(float value)
        {
            transform.localScale = new(Mathf.Clamp01(value), 1, 1);
        }
    }
}
