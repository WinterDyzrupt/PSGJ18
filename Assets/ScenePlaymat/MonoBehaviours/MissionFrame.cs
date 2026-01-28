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
        [SerializeField] private MissionWrapper selectedMissionWrapper;

        private void Awake()
        {
            Debug.Assert(expirationBar != null, $"{name} doesn't have an Expiration Bar assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have a Completion Bar assigned in the Inspector.");
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

        public void PostMission(Mission missionToPost)
        {
            Debug.Assert(missionToPost != null, $"MissionFrame: {nameof(missionToPost)} was null.");
            
            _mission = missionToPost;
            _mission.Completed += MissionHasExpiredOrFinished;
            StartCoroutine(_mission.PostAsync());
        }
        
        private void MissionHasExpiredOrFinished(Mission _)
        {
            // TODO: Have the mission remove itself after confirmation window? I'm not sure.
        }

        private void UpdateProgressBars()
        {
            switch (_mission.Status)
            {
                case MissionStatus.Posted: // Didn't expire yet, update bar
                    UpdateProgressBarUI(completionBar,(float)_mission.ExpirationDecimalPercentage);
                    break;
                case MissionStatus.InProgress: // Didn't complete mission yet, update bar
                    if(expirationBar.localScale.y != 0) UpdateProgressBarUI(expirationBar,0);
                    UpdateProgressBarUI(completionBar, (float)_mission.CompletionDecimalPercentage);
                    break;
                case MissionStatus.Successful: // Mission JUST completed, set bar to 100%
                case MissionStatus.Expired:
                case MissionStatus.Failed:
                    UpdateProgressBarUI(completionBar,1f);
                    break;
                case MissionStatus.Inactive:
                case MissionStatus.Assigned:
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
            selectedMissionWrapper.Set(_mission);
        }
    }
}