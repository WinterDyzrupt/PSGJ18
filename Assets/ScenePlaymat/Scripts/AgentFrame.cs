using UnityEngine;
using UnityEngine.UI;
using Variables;

namespace ScenePlaymat.Scripts
{
    public class AgentFrame : MonoBehaviour
    {
        [SerializeField] private Agent actingAgent;
        public Agent ActingAgent => actingAgent;

        [SerializeField] private Transform completionBar;

        [SerializeField] private Image portrait;

        private void Start()
        {
            Debug.Assert(actingAgent != null, $"{name} doesn't have an agent assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have an completion Bar assigned in the Inspector.");
            Debug.Assert(portrait != null, $"{name} doesn't have a portrait assigned in the Inspector.");

            portrait.sprite = ActingAgent.portrait;

            // TODO: Uncomment when we're doing polish animation
            // actingAgent.ChangeInStatus += AgentFinishedMission;
        }

        private void Update()
        {
            if (ActingAgent.Status != AgentStatus.Idle || completionBar.localScale.y != 0)
            {
                completionBar.localScale = new(0, 1f - (float)ActingAgent.CompletionOfMission, 0);
            }
        }

        // TODO: This can be used to animate the frame a bit when the agent becomes available
        /*
        private void AgentFinishedMission(AgentStatus status)
        {
            if (status == AgentStatus.Idle)
            {
                
            }            
        }
        */
    }
}