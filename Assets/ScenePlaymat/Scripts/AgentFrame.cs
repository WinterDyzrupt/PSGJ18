using UnityEngine;
using UnityEngine.UI;
using Variables;

namespace ScenePlaymat.Scripts
{
    public class AgentFrame : MonoBehaviour
    {
        [SerializeField] private Agent agent;

        [SerializeField] private Transform completionBar;

        [SerializeField] private Image portrait;

        private void Start()
        {
            Debug.Assert(agent != null, $"{name} doesn't have an agent assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have an completion Bar assigned in the Inspector.");
            Debug.Assert(portrait != null, $"{name} doesn't have a portrait assigned in the Inspector.");
            
            portrait.sprite = agent.portrait;
        }

        private void Update()
        {
            if (agent.Status != AgentStatus.Idle || completionBar.localScale.y != 0)
            {
                completionBar.localScale = new(0, 1f - (float)agent.CompletionOfMission, 0);
            }
        }
    }
}