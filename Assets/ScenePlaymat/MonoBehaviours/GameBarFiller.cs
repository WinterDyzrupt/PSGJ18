using ScenePlaymat.Data.Missions;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    /// <summary>
    /// Fills victory/defeat game bars as missions complete.
    /// </summary>
    public class GameBarFiller : MonoBehaviour
    {
        /// <summary>
        /// The scheduler to monitor for missions.
        /// </summary>
        public MissionScheduler scheduler;

        /// <summary>
        /// Primarily for viewing in the editor; the bar is not updated during Update, so manual
        /// modifications in the editor will not be reflected until a mission completes.  
        /// </summary>
        public float currentFillPercentageDecimal;
        
        /// <summary>
        /// Whether to monitor successful missions; if false, monitors failed and expired missions.
        /// </summary>
        public bool monitorSuccessfulMissions;

        private void Awake()
        {
            Debug.Assert(scheduler != null, $"{nameof(GameBarFiller)} expected {nameof(scheduler)} to be injected via inspector.");
            scheduler.NewMissionAdded += OnMissionPosted;
        }
        
        private void OnMissionPosted(Mission newMission)
        {
            newMission.Completed += OnMissionCompleted;
        }
        
        private void OnMissionCompleted(Mission mission)
        {
            var increment = 0f;
            if (monitorSuccessfulMissions)
            {
                if (mission.IsCompletedSuccessfully)
                {
                    increment = mission.data.successIncrement;
                }
            }
            else
            {
                if (mission.IsExpired)
                {
                    increment = mission.data.expirationIncrement;
                }
                else if (mission.IsFailed)
                {
                    increment = mission.data.failureIncrement;
                }
            }
            
            currentFillPercentageDecimal += increment;
            Debug.Log($"{nameof(GameBarFiller)} ({name}): Incrementing bar by: {increment}; newValue: {currentFillPercentageDecimal}.");
            UpdateBar(currentFillPercentageDecimal);
        }
        
        private void UpdateBar(float value)
        {
            transform.localScale = new(Mathf.Clamp01(value), 1, 1);
        }
    }
}