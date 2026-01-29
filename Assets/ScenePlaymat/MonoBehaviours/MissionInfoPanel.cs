using ScenePlaymat.Data.Agents;
using ScenePlaymat.Data.Missions;
using ScenePlaymat.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionInfoPanel : MonoBehaviour
    {
        public UnityEvent missionAssignedEvent;
        public UnityEvent pauseEvent;
        public UnityEvent resumeEvent;

        [Header("Mission Info Panel")]
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Transform[] statBars;
        [SerializeField] private Button acceptMissionButton;
        [SerializeField] private TMP_Text acceptMissionButtonText;
        
        [Header("Selected Wrappers")]
        [SerializeField] private AgentWrapper selectedAgent;
        [SerializeField] private MissionWrapper selectedMission;

        private void Awake()
        {
            Debug.Assert(infoPanel != null, $"{name} doesn't have a Mission Info panel assigned!");
            Debug.Assert(nameText != null, $"{name} doesn't have a Mission Name Text assigned!");
            Debug.Assert(descriptionText != null, $"{name} doesn't have a Mission Description Text assigned!");
            Debug.Assert(statBars != null, $"{name} doesn't have a Mission Stat Bars assigned!");
            Debug.Assert(acceptMissionButton != null, $"{name} doesn't have a Accept Mission button assigned!");
            Debug.Assert(selectedAgent != null, $"{name} doesn't have a Selected Agent reference assigned!");

            Debug.Assert(selectedAgent != null, $"{name} doesn't have a Selected Agent reference assigned!");
            Debug.Assert(selectedMission != null, $"{name} doesn't have a Selected Mission reference assigned!");

            infoPanel.SetActive(false);
            selectedAgent.Changed += UpdateAcceptMissionButton;
            selectedMission.Changed += UpdateMissionPanel;
        }

        private void OnDestroy()
        {
            selectedAgent.Changed -= UpdateAcceptMissionButton;
            selectedMission.Changed -= UpdateMissionPanel;
        }

        private void UpdateMissionPanel(Mission mission)
        {
            Debug.Log("New mission selected: " + mission);
            if (mission != null)
            {
                nameText.text = mission.data.displayName;
                if (mission.IsExpired || mission.IsFailed)
                {
                    descriptionText.text = mission.data.failureText;
                }
                else if (mission.IsCompletedSuccessfully)
                {
                    descriptionText.text = mission.data.completionText;
                }
                else
                {
                    descriptionText.text = mission.data.description;
                }
                
                var missionAttributes = mission.data.missionAttributes.AttributesTotal;
                for (var i = 0; i < statBars.Length; i++)
                {
                    statBars[i].localScale = new(0.1f * missionAttributes[i], 1f, 1f);
                }

                UpdateAcceptMissionButton(mission, selectedAgent.Agent);
                ShowPanel(infoPanel);
            }
            else
            {
                HidePanel(infoPanel);
            }
        }
        
        private void ShowPanel(GameObject panelToShow)
        {
            panelToShow.SetActive(true);
            pauseEvent.Invoke();
        }
        
        private void HidePanel(GameObject panelToHide)
        {
            panelToHide.SetActive(false);
            if (selectedAgent.Agent == null)
            {
                //Debug.Log("Mission panel closed and there is no selected agent; resuming.");
                resumeEvent.Invoke();
            }
            else
            {
                //Debug.Log("Mission panel closed, but an agent is selected; not resuming.");
            }
        }
        
        public void AcceptMissionButtonPressed()
        {
            // The button gets disabled in UpdateAcceptMissionButton too, but it's a good idea to disable it ASAP to
            // reduce the likelihood of it being clicked twice before being disabled.
            acceptMissionButton.interactable = false;

            if (selectedMission.Mission == null)
            {
                Debug.LogError("Accept mission button was pressed without a selected mission.");
            }
            else if (selectedMission.Mission.IsCompleted)
            {
                selectedMission.Mission.Dismiss();
                HidePanel(infoPanel);
            }
            else
            {
                StartCoroutine(selectedAgent.Agent.StartMissionAsync(selectedMission.Mission));
                UpdateAcceptMissionButton(selectedMission.Mission, selectedAgent.Agent);
                missionAssignedEvent.Invoke();
            }
        }

        /// <summary>
        /// GameEventListener calls this while listening for the MissionAssigned event.
        /// </summary>
        public void OnMissionAssigned()
        {
            selectedMission.Set(null);
            HidePanel(infoPanel);
        }
        
        private void UpdateAcceptMissionButton(Agent agent)
        {
            UpdateAcceptMissionButton(selectedMission.Mission, agent);
        }

        private void UpdateAcceptMissionButton(Mission mission, Agent agent)
        {
            if (!mission)
            {
                acceptMissionButton.interactable = false;
                acceptMissionButtonText.text = "Select a mission";
            }
            else
            {
                acceptMissionButton.interactable =
                    mission.IsCompleted ||
                    (mission.Status == MissionStatus.Posted &&
                    agent?.Status == AgentStatus.Idle);
                
                switch (mission.Status)
                {
                    case MissionStatus.Posted:
                        if (agent?.Status == AgentStatus.Idle)
                        {
                            acceptMissionButtonText.text = "Assign Agent";
                        }
                        else if (agent)
                        {
                            acceptMissionButtonText.text = "Agent Busy";
                        }
                        else
                        {
                            acceptMissionButtonText.text = "Select Agent";
                        }

                        break;
                    case MissionStatus.Assigned:
                    case MissionStatus.InProgress:
                        acceptMissionButtonText.text = "Assigned to " + mission.AssignedAgent.DisplayName;
                        break;
                    case MissionStatus.Successful:
                        acceptMissionButtonText.text = "Success; Dismiss";
                        break;
                    case MissionStatus.Expired:
                        acceptMissionButtonText.text = "Expired; Dismiss";
                        break;
                    case MissionStatus.Failed:
                        acceptMissionButtonText.text = "Failed; Dismiss";
                        break;
                    case MissionStatus.Inactive:
                    default:
                        Debug.LogError("Unexpected mission status for accept mission button: " + mission.Status);
                        acceptMissionButtonText.text = "It's a mystery";
                        break;
                }
            }
        }
    }
}