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
        private bool _isPanelInfoOn = true;

        [Header("Panel Components")] [SerializeField]
        private TMP_Text agentName;

        [SerializeField] private Image mugShotImage;
        [SerializeField] private Transform[] barBaseStats;
        [SerializeField] private Transform[] barTotalStats;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Transform statusProgress;
        
        [Header("Selected Agent")]
        [SerializeField] private AgentWrapper selectedAgent;

        private void Awake()
        {
            Debug.Assert(agentName != null, "AgentName Text is missing in inspector!");
            Debug.Assert(mugShotImage != null, "MugShot Image is missing in inspector!");
            Debug.Assert(barBaseStats?.Length > 0, "Bar Base Stats is missing in inspector!");
            Debug.Assert(barTotalStats?.Length > 0, "Bar TotalStats is missing in inspector!");
            Debug.Assert(statusText != null, "StatusText is missing in inspector!");
            Debug.Assert(statusProgress != null, "StatusProgress is missing in inspector!");
            
            Debug.Assert(selectedAgent != null, "SelectedAgent is missing in inspector!");
            
            InitializePanel();
        }

        private void Update()
        {
            if (_isPanelInfoOn && _agent && statusProgress.localScale != Vector3.zero)
            {
                UpdateStatusBar();
            }
        }

        private void OnEnable()
        {
            selectedAgent.AgentHasChanged += ChooseAgent;
        }

        private void OnDisable()
        {
            selectedAgent.AgentHasChanged -= ChooseAgent;
        }

        private void ChooseAgent(Agent newAgent)
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

                for (var i = 0; i < barBaseStats.Length; i++)
                {
                    barBaseStats[i].localScale = new(0.1f * _agent.attributes.AttributesBase[i], 1, 1);
                    barTotalStats[i].localScale = new(0.1f * _agent.attributes.AttributesTotal[i], 1, 1);
                }

                UpdateStatusText(_agent.Status);
                if (_agent.Status != AgentStatus.Idle) UpdateStatusBar();
                else statusProgress.localScale = new(1f, 0, 1f);
            }
        }

        private void UpdateStatusBar()
        {
            Vector3 newScale = new(1f, 1f - (float)_agent.CompletionOfDeploying, 1f);
            var isAgentIdle = _agent.Status == AgentStatus.Idle;
            statusProgress.localScale = isAgentIdle ? Vector3.zero : newScale;
        }

        private void UpdateStatusText(AgentStatus status)
        {
            statusText.text = status == AgentStatus.Idle ? string.Empty : status.ToString();
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