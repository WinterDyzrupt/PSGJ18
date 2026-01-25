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

        private void Awake()
        {
            Debug.Assert(expirationBar != null, $"{name} doesn't have an Expiration Bar assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have a Completion Bar assigned in the Inspector.");
            
            GrabCurrentMission();
        }

        private void Update()
        {
            if (!_mission)
            {
                Debug.LogError($"{name} was never assigned a mission!");
                return;
            }
            
            if (_mission.Status is MissionStatus.Posted or MissionStatus.InProgress)
            {
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

            _mission.Expired += MissionHasExpiredOrFinished;
            _mission.Completed += MissionHasExpiredOrFinished;
            
            _mission.Post();
        }

        private void UpdateProgressBars()
        {
            switch (_mission.FetchCurrentStatus())
            {
                case MissionStatus.Posted: // Didn't expire yet, update bar
                    UpdateProgressBarUI(completionBar,(float)_mission.ExpirationDecimalPercentage);
                    break;
                case MissionStatus.Expired: // Posting JUST expired, set bar to 100%
                    UpdateProgressBarUI(completionBar,1f);
                    break;
                case MissionStatus.InProgress: // Didn't complete mission yet, update bar
                    if(expirationBar.localScale.y != 0) UpdateProgressBarUI(expirationBar,0);
                    UpdateProgressBarUI(completionBar, (float)_mission.CompletionDecimalPercentage);
                    break;
                case MissionStatus.Completed: // Mission JUST completed, set bar to 100%
                    UpdateProgressBarUI(completionBar,1f);
                    break;
                case MissionStatus.Inactive:
                case MissionStatus.Claimed:
                default:
                    Debug.LogError($"{name} tried to update progress bars but {_mission.Status} is invalid!");
                    break;
            }
        }

        private void UpdateProgressBarUI(Transform barTransform, float decimalPercentage)
        {
            barTransform.localScale = new(1f, decimalPercentage, 1f);
        }

        public void FrameClicked()
        {
            selectedMission.Set(_mission);
        }
    }
}