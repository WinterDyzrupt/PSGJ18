using ScenePlaymat.Data.Agents;
using ScenePlaymat.Data.Missions;
using ScenePlaymat.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionInfoPanel : MonoBehaviour
    {
        [Header("Mission Info Panel")]
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Transform[] statBars;
        [SerializeField] private Button acceptMissionButton;
        
        [Header("Selected Wrappers")]
        [SerializeField] private AgentWrapper selectedAgent;
        [SerializeField] private MissionWrapper selectedMission;
        
        private void Awake()
        {
            Debug.Assert(infoPanel != null, "Mission Info panel is not assigned!");
            Debug.Assert(nameText != null, "Mission Name Text is not assigned!");
            Debug.Assert(descriptionText != null, "Mission Description Text is not assigned!");
            Debug.Assert(statBars != null, "Mission Stat Bars are not assigned!");
            Debug.Assert(acceptMissionButton != null, "Accept Mission button is not assigned!");
            Debug.Assert(selectedAgent != null, "Selected Agent reference is not assigned!");

            infoPanel.SetActive(false);
        }

        private void OnEnable()
        {
            selectedAgent.AgentHasChanged += ToggleAcceptMissionButton;
            selectedMission.MissionHasChanged += UpdateMissionPanel;
        }

        private void OnDisable()
        {
            selectedAgent.AgentHasChanged -= ToggleAcceptMissionButton;
            selectedMission.MissionHasChanged -= UpdateMissionPanel;
        }

        private void UpdateMissionPanel(Mission mission)
        {
            if (mission == null)
            {
                Debug.LogWarning($"{name} was told to update with a null mission!");
                return;
            }
            
            infoPanel.SetActive(true);
            nameText.text = mission.data.displayName;
            descriptionText.text = mission.data.description;
            var missionAttributes = mission.data.missionAttributes.AttributesTotal;
            for (var i = 0; i < statBars.Length; i++)
            {
                statBars[i].localScale = new(0.1f * missionAttributes[i], 1f, 1f);
            }
        }

        public void AcceptMissionButtonPressed()
        {
            infoPanel.SetActive(false);
            
            selectedAgent.Agent.AcceptMission(selectedMission.Mission);
            ToggleAcceptMissionButton(null);
        }

        private void ToggleAcceptMissionButton(Agent agent)
        {
            var turnOn = agent != null && agent.Status == AgentStatus.Idle;
            acceptMissionButton.interactable = turnOn;
        }
    }
}