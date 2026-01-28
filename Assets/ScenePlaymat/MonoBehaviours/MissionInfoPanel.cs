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

        private void Update()
        {
            // TODO: Remove if the game is paused while the panel is open; updating the button during Update is only
            // useful for when a mission expires while the panel is open. 
            UpdateAcceptMissionButton(selectedMission.Mission, selectedAgent.Agent);
        }

        private void OnDestroy()
        {
            selectedAgent.Changed -= UpdateAcceptMissionButton;
            selectedMission.Changed -= UpdateMissionPanel;
        }

        private void UpdateMissionPanel(Mission mission)
        {
            if (mission == null)
            {
                Debug.LogWarning($"{name} was told to update with a null mission!");
                return;
            }
            
            nameText.text = mission.data.displayName;
            descriptionText.text = mission.data.description;
            var missionAttributes = mission.data.missionAttributes.AttributesTotal;
            for (var i = 0; i < statBars.Length; i++)
            {
                statBars[i].localScale = new(0.1f * missionAttributes[i], 1f, 1f);
            }
            UpdateAcceptMissionButton(mission, selectedAgent.Agent);
            infoPanel.SetActive(true);
        }

        public void AcceptMissionButtonPressed()
        {
            // The button gets disabled in UpdateAcceptMissionButton too, but it's a good idea to disable it ASAP to
            // reduce the likelihood of it being clicked twice before being disabled.
            acceptMissionButton.interactable = false;
            selectedAgent.Agent.AcceptMission(selectedMission.Mission);
            UpdateAcceptMissionButton(selectedMission.Mission, selectedAgent.Agent);
            missionAssignedEvent.Invoke();
        }

        /// <summary>
        /// GameEventListener calls this while listening for the MissionAssigned event.
        /// </summary>
        public void OnMissionAssigned()
        {
            HidePanel();
        }

        public void HidePanel()
        {
            infoPanel.SetActive(false);
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
                switch (mission.Status)
                {
                    case MissionStatus.Posted:
                        if (agent?.Status == AgentStatus.Idle)
                        {
                            acceptMissionButton.interactable = true;
                            acceptMissionButtonText.text = "Assign Agent";
                        }
                        else if (agent)
                        {
                            acceptMissionButton.interactable = false;
                            acceptMissionButtonText.text = "Agent Busy";
                        }
                        else
                        {
                            acceptMissionButton.interactable = false;
                            acceptMissionButtonText.text = "Select Agent";
                        }

                        break;
                    case MissionStatus.Claimed:
                    case MissionStatus.InProgress:
                        acceptMissionButton.interactable = false;
                        acceptMissionButtonText.text = "Assigned to " + mission.AssignedAgent.DisplayName;
                        break;
                    case MissionStatus.Completed:
                        acceptMissionButton.interactable = false;
                        acceptMissionButtonText.text = "Completed";
                        break;
                    case MissionStatus.Expired:
                        acceptMissionButton.interactable = false;
                        acceptMissionButtonText.text = "Expired";
                        break;
                    case MissionStatus.Inactive:
                    default:
                        Debug.LogError("Unexpected mission status for accept mission button: " + mission.Status);
                        acceptMissionButton.interactable = false;
                        acceptMissionButtonText.text = "It's a mystery";
                        break;
                }
            }
        }
    }
}