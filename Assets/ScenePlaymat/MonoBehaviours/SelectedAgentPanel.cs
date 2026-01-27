using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ScenePlaymat.Data.Agents;
using ScenePlaymat.Utils;

namespace ScenePlaymat.MonoBehaviours
{
    public class SelectedAgentPanel : MonoBehaviour
    {
        private Agent _agent;
        
        [SerializeField] private AgentWrapper selectedAgent;

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
            Debug.Assert(statusText != null, "StatusText is missing in inspector!");
            Debug.Assert(statusProgress != null, "StatusProgress is missing in inspector!");
            
            Debug.Assert(selectedAgent != null, "SelectedAgent is missing in inspector!");
            
            panel.SetActive(false);
            selectedAgent.Changed += NewAgentSelected;
        }

        private void Update()
        {
            if (_agent)
            {
                UpdateStatusBar();
            }
        }

        private void OnDestroy()
        {
            selectedAgent.Changed -= NewAgentSelected;
        }

        private void NewAgentSelected(Agent newAgent)
        {
            Debug.Log("New Agent selected: " + newAgent);
            
            if (_agent)
            {
                _agent.StatusChanged -= UpdateStatusText;
            }

            _agent = newAgent;

            if (_agent)
            {
                _agent.StatusChanged += UpdateStatusText;
            }

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

                var currentStatus = _agent.FetchCurrentStatus();
                UpdateStatusText(currentStatus);
                if (currentStatus != AgentStatus.Idle) UpdateStatusBar();
                else statusProgress.localScale = new(1f, 0, 1f);
                panel.SetActive(true);
            }
        }

        private void UpdateStatusBar()
        {
            Vector3 newScale = new(1f, 1f - (float)_agent.CompletionOfDeploying, 1f);
            var isAgentIdle = _agent.FetchCurrentStatus() == AgentStatus.Idle;
            statusProgress.localScale = isAgentIdle ? Vector3.zero : newScale;
        }

        private void UpdateStatusText(AgentStatus status)
        {
            statusText.text = status == AgentStatus.Idle ? string.Empty : status.ToString();
        }
    }
}