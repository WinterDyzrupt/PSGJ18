using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ScenePlaymat.Data.Agents;
using ScenePlaymat.Utils;

namespace ScenePlaymat.MonoBehaviours
{
    public class PanelAgentInfo : MonoBehaviour
    {
        private Agent _agent;
        private bool _isPanelInfoOn = true;

        [Header("Panel Components")] [SerializeField]
        private TMP_Text agentName;

        [SerializeField] private Image mugShotImage;
        [SerializeField] private Transform[] barBaseStats;
        [SerializeField] private Transform[] barTotalStats;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Transform statusProgress;
        
        [Header("Active Agent Reference")]
        [SerializeField] private AgentReference selectedAgent;


        private void Awake()
        {
            Debug.Assert(agentName != null, "AgentName Text is missing in inspector!");
            Debug.Assert(mugShotImage != null, "MugShot Image is missing in inspector!");
            Debug.Assert(barBaseStats?.Length > 0, "Bar Base Stats is missing in inspector!");
            Debug.Assert(barTotalStats?.Length > 0, "Bar TotalStats is missing in inspector!");
            Debug.Assert(statusText != null, "StatusText is missing in inspector!");
            Debug.Assert(statusProgress != null, "StatusProgress is missing in inspector!");
            
            Debug.Assert(selectedAgent != null, "SelectedAgent is missing in inspector!");

            selectedAgent.AgentHasChanged += ChooseAgent;
            
            InitializePanel();
        }

        private void Update()
        {
            if (_isPanelInfoOn && _agent && (_agent.Status != AgentStatus.Idle || statusProgress.localScale.x != 0))
            {
                UpdateStatusBar();
            }
        }

        public void ChooseAgent(Agent newAgent)
        {
            if (_agent != null)
            {
                _agent.ChangeInStatus -= UpdateStatusText;
            }

            _agent = newAgent;

            if (_agent != null)
            {
                _agent.ChangeInStatus += UpdateStatusText;
            }

            InitializePanel();
        }

        private void InitializePanel()
        {
            if (_agent == null)
            {
                if (_isPanelInfoOn) TogglePanelVisibility();
            }
            else
            {
                if (!_isPanelInfoOn) TogglePanelVisibility();

                agentName.text = _agent.DisplayName;
                mugShotImage.sprite = _agent.mugshot;

                for (int i = 0; i < barBaseStats.Length; i++)
                {
                    barBaseStats[i].localScale = new(0.1f * _agent.attributes.AttributesBase[i], 1, 1);
                    barTotalStats[i].localScale = new(0.1f * _agent.attributes.AttributesTotal[i], 1, 1);
                }

                UpdateStatusText(_agent.Status);
                if (_agent.Status != AgentStatus.Idle) UpdateStatusBar();
            }
        }

        private void UpdateStatusBar()
        {
            statusProgress.localScale = new((float)_agent.CompletionOfCurrentStatus, 0, 0);
        }

        private void UpdateStatusText(AgentStatus status)
        {
            statusText.text = status.ToString();
        }

        private void TogglePanelVisibility()
        {
            _isPanelInfoOn = !_isPanelInfoOn;

            agentName.gameObject.SetActive(_isPanelInfoOn);
            mugShotImage.gameObject.SetActive(_isPanelInfoOn);
            barBaseStats[0].parent.parent.gameObject.SetActive(_isPanelInfoOn);
            statusText.transform.parent.gameObject.SetActive(_isPanelInfoOn);
        }
    }
}