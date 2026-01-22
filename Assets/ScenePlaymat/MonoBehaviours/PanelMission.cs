using ScenePlaymat.Data.Agents;
using ScenePlaymat.Data.Missions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScenePlaymat.MonoBehaviours
{
    public class PanelMission : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject missionButton;

        [Header("Agent Reference")]
        [SerializeField] private AgentReference selectedAgent;

        [Header("Mission List Panel")]
        [SerializeField] private Transform content;

        [Header("Mission Info Panel")]
        [SerializeField] private GameObject panelMissionInfo;
        [SerializeField] private TMP_Text missionName;
        [SerializeField] private TMP_Text missionDescription;
        [SerializeField] private Transform[] missionStatBars;
        [SerializeField] private Button acceptMissionButton;
        [SerializeField] private Button backButton;

        private Mission _viewingMission;
        private GameObject _viewingMissionButton;

        private void Awake()
        {
            Debug.Assert(missionButton != null, "Mission Button prefab is not assigned!");

            Debug.Assert(selectedAgent != null, "Selected Agent reference is not assigned!");

            Debug.Assert(content != null, "Content transform is not assigned!");

            Debug.Assert(panelMissionInfo != null, "Mission Info panel is not assigned!");
            Debug.Assert(missionName != null, "Mission Name text is not assigned!");
            Debug.Assert(missionDescription != null, "Mission Description text is not assigned!");
            Debug.Assert(missionStatBars != null, "Mission Stat Bars are not assigned!");
            Debug.Assert(acceptMissionButton != null, "Accept Mission button is not assigned!");
            Debug.Assert(backButton != null, "Back button is not assigned!");

            panelMissionInfo.SetActive(false);
            
            selectedAgent.AgentHasChanged += ToggleAcceptMissionButton;
        }

        public void AddMissions(Mission mission)
        {
            if (mission == null)
            {
                Debug.LogError("Null Missions are not accepted.");
                return;
            }

            GameObject newButtonGameObject = Instantiate(missionButton, content);

            Button newButton = newButtonGameObject.GetComponent<Button>();
            newButton.onClick.AddListener(() => MissionButtonPressed(mission, newButtonGameObject));
        }

        public void MissionButtonPressed(Mission mission, GameObject button)
        {
            panelMissionInfo.SetActive(true);
            missionName.text = mission.data.displayName;
            missionDescription.text = mission.data.description;
            int[] missionAttributes = mission.data.missionAttributes.AttributesTotal;
            for (int i = 0; i < missionStatBars.Length; i++)
            {
                missionStatBars[i].localScale = new(0.1f * missionAttributes[i], 1f, 1f);
            }

            _viewingMission = mission;
            _viewingMissionButton = button;
        }

        public void AcceptMissionButtonPressed()
        {
            panelMissionInfo.SetActive(false);
            selectedAgent.Agent.AcceptMission(_viewingMission);
            selectedAgent.ChangeAgent(null);
            Destroy(_viewingMissionButton.gameObject);
        }

        public void BackButtonPressed()
        {
            panelMissionInfo.SetActive(false);
        }

        private void ToggleAcceptMissionButton(Agent _)
        {
            acceptMissionButton.interactable = selectedAgent.Agent != null;
        }
    }
}