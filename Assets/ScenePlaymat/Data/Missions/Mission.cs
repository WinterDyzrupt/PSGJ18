using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using ScenePlaymat.Data.Agents;
using ScenePlaymat.Utils;
using Debug = UnityEngine.Debug;

namespace ScenePlaymat.Data.Missions
{
    [CreateAssetMenu(fileName = "Mission", menuName = "Data/Missions/Mission")]
    public class Mission : ScriptableObject
    {
        public MissionData data;

        [SerializeField] private MissionStatus status;
        public MissionStatus Status => status;

        /// <summary>
        /// Whether the mission has been completed in one of three final states (Expirerd, Successful, or Failed)
        /// </summary>
        public bool IsCompleted => Status is MissionStatus.Expired or MissionStatus.Successful or MissionStatus.Failed;
        public bool IsExpired => status == MissionStatus.Expired;
        public bool IsCompletedSuccessfully => status == MissionStatus.Successful;
        public bool IsFailed => status == MissionStatus.Failed;
        
        [SerializeField] private Agent assignedAgent;
        public Agent AssignedAgent => assignedAgent;
        
        private TimeSpan _currentStatusDuration;
        private readonly Stopwatch _expiringStopwatch = new();
        private readonly Stopwatch _missionProgressStopwatch = new();
        
        /// <summary>
        /// Event indicating that the mission has reached one of the three final states:
        /// expired, successful, or failed.
        /// </summary>
        public event Action<Mission> Completed;
        
        public double ExpirationDecimalPercentage => Math.Clamp(
            _expiringStopwatch.Elapsed / data.DurationToExpire, 0, 1);
        public double CompletionDecimalPercentage => Math.Clamp(
            _missionProgressStopwatch.Elapsed / data.DurationToPerform, 0, 1);

        private void OnEnable()
        {
            // Reset status to default for use in unity editor (these will persist if restarting the same scene).
            status = MissionStatus.Inactive;
            assignedAgent = null;
        }

        /// <summary>
        /// Asynchronously posts a mission and waits for it to expire.
        /// </summary>
        /// <returns></returns>
        public IEnumerator PostAsync()
        {
            status = MissionStatus.Posted;
            _expiringStopwatch.Start();
            
            while (status == MissionStatus.Posted && _expiringStopwatch.Elapsed < data.DurationToExpire)
            {
                yield return null;
            }
            _expiringStopwatch.Stop();

            if (status == MissionStatus.Posted)
            {
                Debug.Log($"Mission ({data.displayName}) timed out.");
                status = MissionStatus.Expired;
                MissionCompleted();
            }
        }

        public void AssignAgent(Agent agent)
        {
            Debug.Assert(Status == MissionStatus.Posted,
                $"{data.displayName} was told to Claim but its status is {status} and is not allowed!");
            
            assignedAgent = agent;
            status = MissionStatus.Assigned;
        }

        public IEnumerator PerformAsync()
        {
            Debug.Log("Performing mission: " + data.displayName);
            
            status = MissionStatus.InProgress;
            _missionProgressStopwatch.Start();
            yield return new WaitForSeconds(data.durationToPerformInSeconds);
            _missionProgressStopwatch.Stop();

            status = DetermineMissionResult();
            MissionCompleted();
        }

        private MissionStatus DetermineMissionResult()
        {
            return DetermineMissionResult(this);
        }

        private MissionStatus DetermineMissionResult(Mission mission)
        {
            Debug.Assert(mission.Status == MissionStatus.InProgress,
                $"Mission.DetermineMissionResult status: {mission.Status}, but expected InProgress");
            
            var missionResult = mission.Status;
            if (mission.Status == MissionStatus.InProgress)
            {
                var attributeDifference = Attributes.GetDifferenceInAttributes(mission.AssignedAgent.attributes, mission.data.missionAttributes);
                Debug.Log(
                    $"DetermineMissionResults (Mission: {mission.data.displayName}): attribute difference: {attributeDifference}, max difference: {mission.data.maximumAttributeDifferenceForSuccess}");

                if (attributeDifference > mission.data.maximumAttributeDifferenceForSuccess)
                {
                    missionResult = MissionStatus.Successful;
                }
                else
                {
                    missionResult = MissionStatus.Failed;
                }
            }

            return missionResult;
        }
        
        /// <summary>
        /// The mission completed with some result, possibly expired, success or failure.
        /// </summary>
        private void MissionCompleted()
        {
            MissionCompleted(this);
        }
        
        /// <summary>
        /// The mission completed with some result, possibly expired, success or failure.
        /// </summary>
        private void MissionCompleted(Mission mission)
        {
            if (Completed != null)
            {
                Debug.Log($"Mission ({mission.data.displayName}) completed with status: {mission.Status}");
                Completed.Invoke(mission);
            }
            else
            {
                Debug.LogError($"Mission ({mission.data.displayName}) completed ({mission.Status}) with no listener");
            }
        }

        public void OnPause()
        {
            PauseOrResume(true);
        }

        public void OnResume()
        {
            PauseOrResume(false);
        }

        private void PauseOrResume(bool pause)
        {
            Stopwatch currentStopwatch = null;

            switch (Status)
            {
                case MissionStatus.Posted:
                    currentStopwatch = _expiringStopwatch;
                    break;
                case MissionStatus.InProgress:
                    currentStopwatch = _missionProgressStopwatch;
                    break;
                case MissionStatus.Inactive:
                case MissionStatus.Expired:
                case MissionStatus.Assigned:
                case MissionStatus.Successful:
                case MissionStatus.Failed:
                default:
                    //Debug.Log($"Mission ({data.displayName}) in status: {Status}; paused, but no stopwatches to stop/start");
                    break;
            }

            if (currentStopwatch != null)
            {
                if (pause)
                {
                    currentStopwatch.Stop();
                }
                else
                {
                    currentStopwatch.Start();
                }
            }
        }

        public override string ToString() => data.displayName;
    }
    
    /// <summary>
    /// Unity serialization doesn't support SOs nested within SOs, so this is a tiny duplicate-ish class for
    /// serialization.
    /// </summary>
    [Serializable]
    public class MissionSerializationHelper
    {
        public MissionData data;
    }
}