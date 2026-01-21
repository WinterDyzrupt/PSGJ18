using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Variables;

namespace ScenePlaymat.Scripts
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


        private void Awake()
        {
            Debug.Assert(agentName != null, "AgentName Text is missing in inspector!");
            Debug.Assert(mugShotImage != null, "MugShot Image is missing in inspector!");
            Debug.Assert(barBaseStats?.Length > 0, "Bar Base Stats is missing in inspector!");
            Debug.Assert(barTotalStats?.Length > 0, "Bar TotalStats is missing in inspector!");
            Debug.Assert(statusText != null, "StatusText is missing in inspector!");
            Debug.Assert(statusProgress != null, "StatusProgress is missing in inspector!");

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
            _agent = newAgent;
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
                    barBaseStats[i].localScale = new(0.1f * _agent.attributes.AttributesBase[i], 0, 0);
                    barTotalStats[i].localScale = new(0.1f * _agent.attributes.AttributesTotal[i], 0, 0);
                }
                
                UpdateStatusBar();
            }
        }

        private void UpdateStatusBar()
        {
            statusText.text = _agent.Status.ToString();
            statusProgress.localScale = new(1f - (float)_agent.CompletionOfCurrentStatus, 0, 0);
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