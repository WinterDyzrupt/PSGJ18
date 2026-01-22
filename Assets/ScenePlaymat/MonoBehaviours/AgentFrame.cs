using UnityEngine;
using UnityEngine.UI;
using ScenePlaymat.Data.Agents;
using ScenePlaymat.Utils;

namespace ScenePlaymat.MonoBehaviours
{
    public class AgentFrame : MonoBehaviour
    {
        public Agent frameAgent;

        [SerializeField] private Transform completionBar;
        [SerializeField] private Image portrait;
        [SerializeField] private AgentReference selectedAgent;

        private void Start()
        {
            Debug.Assert(frameAgent != null, $"{name} doesn't have an agent assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have an completion Bar assigned in the Inspector.");
            Debug.Assert(portrait != null, $"{name} doesn't have a portrait assigned in the Inspector.");
            Debug.Assert(selectedAgent != null, $"{name} doesn't have an Selected Agent reference assigned in the Inspector.");
            
            portrait.sprite = frameAgent.portrait;

            // TODO: Uncomment when we're doing polish animation
            // actingAgent.ChangeInStatus += AgentFinishedMission;
        }

        private void Update()
        {
            if (frameAgent.Status != AgentStatus.Idle || completionBar.localScale.y != 0)
            {
                completionBar.localScale = new(0, 1f - (float)frameAgent.CompletionOfMission, 0);
            }
        }

        public void PanelClicked()
        {
            selectedAgent.ChangeAgent(frameAgent);
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