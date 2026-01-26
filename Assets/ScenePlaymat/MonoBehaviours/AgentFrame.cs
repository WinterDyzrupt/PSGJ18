using UnityEngine;
using UnityEngine.UI;
using ScenePlaymat.Data.Agents;
using ScenePlaymat.Utils;
using TMPro;

namespace ScenePlaymat.MonoBehaviours
{
    public class AgentFrame : MonoBehaviour
    {
        public Agent agent;

        [Header("Display Components")]
        [SerializeField] private Transform completionBar;
        [SerializeField] private Image portrait;
        [SerializeField] private TMP_Text statusText;
        
        [Header("Selected Agent Wrapper")]
        [SerializeField] private AgentWrapper selectedAgent;

        private void Awake()
        {
            Debug.Assert(agent != null, $"{name} doesn't have an agent assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have a completion Bar assigned in the Inspector.");
            Debug.Assert(portrait != null, $"{name} doesn't have a portrait assigned in the Inspector.");
            Debug.Assert(statusText != null, $"{name} doesn't have status text assigned in the Inspector.");
            Debug.Assert(selectedAgent != null, $"{name} doesn't have a Selected Agent reference assigned in the Inspector.");
        }

        private void Start()
        {
            portrait.sprite = agent.portrait;
            
            agent.StatusChanged += AgentStatusChanged;
        }

        private void Update()
        {
            switch (agent.Status)
            {
                case AgentStatus.Idle: // do nothing
                    break;
                case AgentStatus.Deploying:
                    UpdateCompletionBar((float)agent.CompletionOfDeploying);
                    agent.FetchCurrentStatus();
                    break;
                case AgentStatus.AttemptingMission: // do nothing
                    break;
                case AgentStatus.Returning:
                    UpdateCompletionBar(1f - (float)agent.CompletionOfReturning);
                    agent.FetchCurrentStatus();
                    break;
                case AgentStatus.Resting:
                    UpdateCompletionBar(1f - (float)agent.CompletionOfResting);
                    agent.FetchCurrentStatus();
                    break;
                default:
                    Debug.LogError($"{agent.DisplayName}'s status({agent.Status}) was impossible.");
                    break;
            }
        }

        private void OnDestroy()
        {
            agent.StatusChanged -= AgentStatusChanged;
        }

        private void AgentStatusChanged(AgentStatus status)
        {
            switch (status)
            {
                case AgentStatus.Idle: // from resting, reset bars to 0, empty text
                    UpdateCompletionBar(0);
                    break;
                case AgentStatus.Deploying: // from resting
                    break;
                case AgentStatus.AttemptingMission: // from deploying, set bar to full update text
                    UpdateCompletionBar(1f);
                    break;
                case AgentStatus.Returning: // from attempting
                case AgentStatus.Resting: // from returning
                    break;
                default:
                    Debug.LogError($"{name}'s agent had impossible status of {status}.");
                    break;
            }
            
            statusText.text = status == AgentStatus.Idle ? string.Empty : status.ToString();
        }

        private void UpdateCompletionBar(float decimalPercentage)
        {
            completionBar.localScale = new(1f, decimalPercentage, 1f);
        }

        public void FrameClicked()
        {
            selectedAgent.Set(agent);
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