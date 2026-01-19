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
            portrait.sprite = agent.portrait;
        }

        private void Update()
        {
            if (agent.Status != AgentStatus.Ready)
            {
                completionBar.localScale = new(0, 1f - agent.CompletionOfCurrentStatus, 0);
            }
        }
    }
}