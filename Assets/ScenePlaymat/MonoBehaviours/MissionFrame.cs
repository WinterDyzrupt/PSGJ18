using System.Collections;
using ScenePlaymat.Data.Missions;
using ScenePlaymat.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionFrame : MonoBehaviour
    {
        [SerializeField] private Mission mission;
        public Mission Mission => mission;

        [Header("Display Components")]
        [SerializeField] private Transform expirationBar;
        [SerializeField] private Transform completionBar;
        [SerializeField] private GameObject selectedMissionIndicator;
        
        [Header("Mission Wrappers")]
        [SerializeField] private MissionWrapper selectedMissionWrapper;

        [Header("Mission Complete Color Values")]
        [SerializeField] private Image background;
        [SerializeField] private Color missionSuccessColor = Color.blue;
        [SerializeField] private Color missionFailureColor = Color.red;
        [SerializeField] private float colorTransitionDuration = 0.5f;

        private void Awake()
        {
            Debug.Assert(expirationBar != null, $"{name} doesn't have an Expiration Bar assigned in the Inspector.");
            Debug.Assert(completionBar != null, $"{name} doesn't have a Completion Bar assigned in the Inspector.");
            selectedMissionWrapper.Changed += ToggleSelectedMissionIndicator;
        }

        private void OnDestroy()
        {
            selectedMissionWrapper.Changed -= ToggleSelectedMissionIndicator;
            mission.Completed -= ColorEndState;
            mission.Dismissed -= MissionDismissed;
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
        /// Called by a GameEventListener to resume the mission in this frame.
        /// </summary>
        public void OnResume()
        {
            mission.OnResume();
        }

        public void PostMission(Mission missionToPost)
        {
            Debug.Assert(missionToPost != null, $"MissionFrame: {nameof(missionToPost)} was null.");
            
            mission = missionToPost;
            mission.Completed += ColorEndState;
            mission.Dismissed += MissionDismissed;
            StartCoroutine(mission.PostAsync());
        }
        
        private void MissionDismissed(Mission _)
        {
            Debug.Log($"MissionFrame ({mission.data.displayName}): Dismissed.");
            Destroy(gameObject);
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
                case MissionStatus.Expired:
                case MissionStatus.Successful:
                case MissionStatus.Failed:
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

        private void ColorEndState(Mission _)
        {
            if (mission.Status is MissionStatus.Successful or MissionStatus.Failed)
            {
                StartCoroutine(AnimateColorEndState(mission.IsCompletedSuccessfully));
            }
        }

        private IEnumerator AnimateColorEndState(bool succeeded)
        {
            // Set Initial State
            var timeTracker = 0f;
            var initialColor = background.color;
            var finalColor = succeeded ? missionSuccessColor : missionFailureColor;

            // While Yield Null
            while (timeTracker < colorTransitionDuration)
            {
                background.color = Color.Lerp(initialColor, finalColor, timeTracker / colorTransitionDuration);
                timeTracker += Time.deltaTime;
                yield return null;
            }

            // Set Final State
            background.color = finalColor;
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

        private void ToggleSelectedMissionIndicator(Mission newMission)
        {
            selectedMissionIndicator.SetActive(mission == newMission);
        }
    }
}