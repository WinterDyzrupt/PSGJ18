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
            Debug.Assert(infoPanel != null, $"{name} doesn't have a Mission Info panel assigned!");
            Debug.Assert(nameText != null, $"{name} doesn't have a Mission Name Text assigned!");
            Debug.Assert(descriptionText != null, $"{name} doesn't have a Mission Description Text assigned!");
            Debug.Assert(statBars != null, $"{name} doesn't have a Mission Stat Bars assigned!");
            Debug.Assert(acceptMissionButton != null, $"{name} doesn't have a Accept Mission button assigned!");
            Debug.Assert(selectedAgent != null, $"{name} doesn't have a Selected Agent reference assigned!");
            
            Debug.Assert(selectedAgent != null, $"{name} doesn't have a Selected Agent reference assigned!");
            Debug.Assert(selectedMission != null, $"{name} doesn't have a Selected Mission reference assigned!");

            infoPanel.SetActive(false);
        }

        private void OnEnable()
        {
            selectedAgent.Changed += ToggleAcceptMissionButton;
            selectedMission.Changed += UpdateMissionPanel;
        }

        private void OnDisable()
        {
            selectedAgent.Changed -= ToggleAcceptMissionButton;
            selectedMission.Changed -= UpdateMissionPanel;
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
            selectedAgent.Agent.AcceptMission(selectedMission.Mission);
            ToggleAcceptMissionButton(false);
            
            infoPanel.SetActive(false);
        }

        private void ToggleAcceptMissionButton(Agent agent)
        {
            ToggleAcceptMissionButton(agent != null && agent.FetchCurrentStatus() == AgentStatus.Idle);
        }

        private void ToggleAcceptMissionButton(bool turnOn)
        {
            acceptMissionButton.interactable = turnOn;
        }
    }
}