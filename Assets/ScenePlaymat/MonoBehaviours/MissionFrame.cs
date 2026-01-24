using ScenePlaymat.Data.Agents;
using ScenePlaymat.Data.Missions;
using ScenePlaymat.Utils;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionFrame : MonoBehaviour
    {
        public Mission mission;

        [Header("Display Components")]
        [SerializeField] private Transform completionBar;
        
        [Header("Selected Mission Wrapper")]
        [SerializeField] private MissionWrapper selectedMission;

        private void Start()
        {
            Debug.Assert(completionBar != null, $"{name} doesn't have a completion Bar assigned in the Inspector.");
        }

        private void Update()
        {
            if (mission.Phase is not MissionPhase.Pending or MissionPhase.Completed)
            {
                mission.AdvanceMissionTimers();
            }
        }

        public void FrameClicked()
        {
            selectedMission.ChangeMission(mission);
        }

        private void MissionPhaseChanged(MissionPhase newPhase)
        {
            // TODO: UI Updates, switch from Expire bar to no bar to completion bar.
        }

        public void AcceptMission(Mission incomingMission)
        {
            mission = incomingMission;

            mission.ChangeInPhase += MissionPhaseChanged;
        }
    }
}