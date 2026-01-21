using UnityEngine;
using UnityEngine.UI;
using ScenePlaymat.Data.Agents;
using ScenePlaymat.Utils;

namespace ScenePlaymat.MonoBehaviours
{
    public class AgentFrame : MonoBehaviour
    {
        public Agent actingAgent;

        [SerializeField] private Transform completionBar;

        [SerializeField] private Image portrait;

        private void Start()
        {
            Debug.Assert(actingAgent != null, $"{name} doesn't have an agent assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have an completion Bar assigned in the Inspector.");
            Debug.Assert(portrait != null, $"{name} doesn't have a portrait assigned in the Inspector.");

            portrait.sprite = actingAgent.portrait;

            // TODO: Uncomment when we're doing polish animation
            // actingAgent.ChangeInStatus += AgentFinishedMission;
        }

        private void Update()
        {
            if (actingAgent.Status != AgentStatus.Idle || completionBar.localScale.y != 0)
            {
                completionBar.localScale = new(0, 1f - (float)actingAgent.CompletionOfMission, 0);
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