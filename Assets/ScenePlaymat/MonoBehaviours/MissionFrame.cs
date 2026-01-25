using System;
using ScenePlaymat.Data.Missions;
using ScenePlaymat.Utils;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionFrame : MonoBehaviour
    {
        private Mission _mission;

        [Header("Display Components")]
        [SerializeField] private Transform expirationBar;
        [SerializeField] private Transform completionBar;
        
        [Header("Mission Wrappers")]
        [SerializeField] private MissionWrapper selectedMission;
        [SerializeField] private MissionWrapper newMission;

        private void Start()
        {
            Debug.Assert(expirationBar != null, $"{name} doesn't have an Expiration Bar assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have a Completion Bar assigned in the Inspector.");
            
            GrabCurrentMission();
        }

        private void Update()
        {
            if (_mission.Phase is MissionPhase.Posted or MissionPhase.Performing)
            {
                _mission.CheckMissionTimers();
                UpdateProgressBars();
            }
        }

        private void MissionHasExpiredOrFinished(Mission _)
        {
            // TODO: Have the mission remove itself after confirmation window? I'm not sure.
        }

        private void GrabCurrentMission()
        {
            if (_mission != null)
            {
                Debug.LogWarning($"{name} was assigned a mission but already had one!");
                return;
            }

            if (newMission.Mission == null )
            {
                Debug.LogWarning($"{name} was created but no new active mission!");
                return;
            }
            
            _mission = newMission.Mission;
            _mission.AdvanceMission(); // Mission has been posted

            _mission.HasExpired += MissionHasExpiredOrFinished;
            _mission.HasBeenCompleted += MissionHasExpiredOrFinished;
        }

        private void UpdateProgressBars()
        {
            switch (_mission.Phase)
            {
                case MissionPhase.Posted: // Didn't expire yet, update bar
                    completionBar.localScale = (float)_mission.CompletionPercentage * Vector3.one;
                    break;
                case MissionPhase.Expired: // Posting JUST expired, set bar to 100%
                    completionBar.localScale = Vector3.one;
                    break;
                case MissionPhase.Performing: // Didn't complete mission yet, update bar
                    if(expirationBar.localScale != Vector3.zero) expirationBar.localScale = Vector3.zero;
                    completionBar.localScale = (float)_mission.CompletionPercentage * Vector3.one;
                    break;
                case MissionPhase.Completed: // Mission JUST completed, set bar to 100%
                    completionBar.localScale = Vector3.one;
                    break;
                case MissionPhase.Posting:
                case MissionPhase.Assigned:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void FrameClicked()
        {
            selectedMission.ChangeMission(_mission);
        }
    }
}