using UnityEngine;
using UnityEngine.UI;
using ScenePlaymat.Data.Agents;
using ScenePlaymat.Utils;

namespace ScenePlaymat.MonoBehaviours
{
    public class AgentFrame : MonoBehaviour
    {
        public Agent agent;

        [Header("Display Components")]
        [SerializeField] private Transform completionBar;
        [SerializeField] private Image portrait;
        
        [Header("Selected Agent Wrapper")]
        [SerializeField] private AgentWrapper selectedAgent;

        private void Start()
        {
            Debug.Assert(agent != null, $"{name} doesn't have an agent assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have a completion Bar assigned in the Inspector.");
            Debug.Assert(portrait != null, $"{name} doesn't have a portrait assigned in the Inspector.");
            Debug.Assert(selectedAgent != null, $"{name} doesn't have a Selected Agent reference assigned in the Inspector.");
            
            portrait.sprite = agent.portrait;
            
            agent.InitializeAgent();

            // TODO: Uncomment when we're doing polish animation
            // actingAgent.ChangeInStatus += AgentFinishedMission;
            // put this in OnEnable/OnDisable
        }

        private void Update()
        {
            if (agent.Status != AgentStatus.Idle) agent.AdvanceAgentTimers();
            
            if (agent.Status != AgentStatus.Idle || completionBar.localScale.y != 0)
            {
                completionBar.localScale = new(0, 1f - (float)agent.CompletionOfMission, 0);
            }
        }

        public void FrameClicked()
        {
            selectedAgent.ChangeAgent(agent);
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