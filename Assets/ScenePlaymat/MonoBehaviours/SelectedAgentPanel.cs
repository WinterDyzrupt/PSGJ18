using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ScenePlaymat.Data.Agents;
using ScenePlaymat.Data.Missions;
using UnityEngine.Events;

namespace ScenePlaymat.MonoBehaviours
{
    public class SelectedAgentPanel : MonoBehaviour
    {
        private Agent _agent;
        [SerializeField] private AgentWrapper selectedAgent;
        [SerializeField] private MissionWrapper selectedMission;
        public UnityEvent pauseEvent;
        public UnityEvent resumeEvent;

        [Header("Panel Components")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text agentName;
        [SerializeField] private Image mugShotImage;
        [SerializeField] private Transform[] barBaseStats;
        [SerializeField] private Transform[] barTotalStats;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Transform statusProgress;

        private void Awake()
        {
            Debug.Assert(agentName != null, "AgentName Text is missing in inspector!");
            Debug.Assert(mugShotImage != null, "MugShot Image is missing in inspector!");
            Debug.Assert(barBaseStats?.Length > 0, "Bar Base Stats is missing in inspector!");
            Debug.Assert(barTotalStats?.Length > 0, "Bar TotalStats is missing in inspector!");
            Debug.Assert(selectedAgent != null, "SelectedAgent is missing in inspector!");

            panel.SetActive(false);
            selectedAgent.Changed += NewAgentSelected;
        }

        private void OnDestroy()
        {
            selectedAgent.Changed -= NewAgentSelected;
        }

        private void NewAgentSelected(Agent newAgent)
        {
            Debug.Log("New Agent selected: " + newAgent);
            
            _agent = newAgent;

            UpdatePanel();
        }
        
        private void UpdatePanel()
        {
            if (_agent != null)
            {
                agentName.text = _agent.DisplayName;
                mugShotImage.sprite = _agent.mugshot;

                for (var i = 0; i < barBaseStats.Length; i++)
                {
                    barBaseStats[i].localScale = new(0.1f * _agent.attributes.AttributesBase[i], 1, 1);
                    barTotalStats[i].localScale = new(0.1f * _agent.attributes.AttributesTotal[i], 1, 1);
                }

                ShowPanel(panel);
            }
            else
            {
                HidePanel(panel);
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
            if (selectedMission.Mission == null)
            {
                //Debug.Log("Agent panel closed and there is no selected mission; resuming.");
                resumeEvent.Invoke();
            }
            else
            {
                //Debug.Log("Agent panel closed, but a mission is selected; not resuming.");
            }
        }
        /// <summary>
        /// GameEventListener calls this while listening for the MissionAssigned event.
        /// </summary>
        public void OnMissionAssigned()
        {
            selectedAgent.Set(null);
            HidePanel(panel);
        }
    }
}