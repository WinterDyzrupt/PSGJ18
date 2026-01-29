using ScenePlaymat.Data.Missions;
using ScenePlaymat.Utils;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionFrame : MonoBehaviour
    {
        [SerializeField] private Mission mission;
        public Mission Mission => mission;

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
            if (!mission)
            {
                Debug.LogError($"{name} was never assigned a mission!");
                return;
            }
            
            if (mission.Status is MissionStatus.Posted or MissionStatus.InProgress)
            {
                UpdateProgressBars();
            }
        }
        
        /// <summary>
        /// Called by a GameEventListener to pause the mission in this frame.
        /// </summary>
        public void OnPause()
        {
            mission.OnPause();
        }

        /// <summary>
        /// Called by a GameEventListner to resume the mission in this frame.
        /// </summary>
        public void OnResume()
        {
            mission.OnResume();
        }

        public void PostMission(Mission missionToPost)
        {
            Debug.Assert(missionToPost != null, $"MissionFrame: {nameof(missionToPost)} was null.");
            
            mission = missionToPost;
            mission.Completed += MissionHasExpiredOrFinished;
            StartCoroutine(mission.PostAsync());
        }
        
        private void MissionHasExpiredOrFinished(Mission _)
        {
            // TODO: Have the mission remove itself after confirmation window? I'm not sure.
        }

        private void UpdateProgressBars()
        {
            switch (mission.Status)
            {
                case MissionStatus.Posted: // Didn't expire yet, update bar
                    var postedScale = (float)mission.ExpirationDecimalPercentage;
                    UpdateProgressBarUI(expirationBar,postedScale,postedScale);
                    break;
                case MissionStatus.InProgress: // Didn't complete mission yet, update bar
                    var progressScale = 1f -  (float)mission.CompletionDecimalPercentage;
                    UpdateProgressBarUI(completionBar, 1f, progressScale);
                    break;
                case MissionStatus.Successful: // Mission JUST completed, set bar to 100%
                case MissionStatus.Expired:
                case MissionStatus.Failed:
                    UpdateProgressBarUI(completionBar,1f, 1f);
                    break;
                case MissionStatus.Inactive:
                case MissionStatus.Assigned:
                default:
                    Debug.LogError($"{name} tried to update progress bars but {mission.Status} is invalid!");
                    break;
            }
        }

        private void UpdateProgressBarUI(Transform barTransform, float xScale, float yScale)
        {
            barTransform.localScale = new(xScale, yScale, 1f);
        }

        public void FrameClicked()
        {
            if (selectedMissionWrapper.Mission == mission)
            {
                //Debug.Log("Selected currently selected mission again; unselecting mission.");
                selectedMissionWrapper.Reset();
            }
            else
            {
                selectedMissionWrapper.Set(mission);
            }
        }
    }
}