using UnityEngine;
using UnityEngine.UI;
using Variables;

namespace ScenePlaymat.Scripts
{
    public class AgentFrame : MonoBehaviour
    {
        public Agent ActingAgent { get; private set; }

        [SerializeField] private Transform completionBar;

        [SerializeField] private Image portrait;

        private void Start()
        {
            Debug.Assert(ActingAgent != null, $"{name} doesn't have an agent assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have an completion Bar assigned in the Inspector.");
            Debug.Assert(portrait != null, $"{name} doesn't have a portrait assigned in the Inspector.");

            portrait.sprite = ActingAgent.portrait;
        }

        private void Update()
        {
            if (ActingAgent.Status != AgentStatus.Idle || completionBar.localScale.y != 0)
            {
                completionBar.localScale = new(0, 1f - (float)ActingAgent.CompletionOfMission, 0);
            }
        }
    }
}