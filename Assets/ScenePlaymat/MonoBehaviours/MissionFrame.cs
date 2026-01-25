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
            _mission.AdvanceMissionPhase(); // Mission has been posted

            _mission.HasExpired += MissionHasExpiredOrFinished;
            _mission.HasBeenCompleted += MissionHasExpiredOrFinished;
        }

        private void UpdateProgressBars()
        {
            switch (_mission.Phase)
            {
                case MissionPhase.Posted: // Didn't expire yet, update bar
                    UpdateProgressBarUI(completionBar,(float)_mission.ExpirationPercentage);
                    break;
                case MissionPhase.Expired: // Posting JUST expired, set bar to 100%
                    UpdateProgressBarUI(completionBar,1f);
                    break;
                case MissionPhase.Performing: // Didn't complete mission yet, update bar
                    if(expirationBar.localScale.y != 0) UpdateProgressBarUI(expirationBar,0);
                    UpdateProgressBarUI(completionBar, (float)_mission.CompletionPercentage);
                    break;
                case MissionPhase.Completed: // Mission JUST completed, set bar to 100%
                    UpdateProgressBarUI(completionBar,1f);
                    break;
                case MissionPhase.Posting:
                case MissionPhase.Assigned:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateProgressBarUI(Transform barTransform, float decimalPercentage)
        {
            barTransform.localScale = new(1f, decimalPercentage, 1f);
        }

        public void FrameClicked()
        {
            selectedMission.ChangeMission(_mission);
        }
    }
}