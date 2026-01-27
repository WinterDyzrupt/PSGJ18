using System;
using System.Diagnostics;
using ScenePlaymat.Utils;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ScenePlaymat.Data.Missions
{
    [CreateAssetMenu(fileName = "Mission", menuName = "Data/Missions/Mission")]
    public class Mission : ScriptableObject
    {

        public MissionData data;

        [SerializeField] private MissionStatus status;
        public MissionStatus Status => status;
        
        private TimeSpan _currentStatusDuration;
        private readonly Stopwatch _expiringStopwatch = new();
        private readonly Stopwatch _missionProgressStopwatch = new();
        
        public event Action<Mission> Expired;
        public event Action<Mission> Completed;
        
        public double ExpirationDecimalPercentage => Math.Clamp(
            _expiringStopwatch.Elapsed / data.DurationToExpire, 0, 1);
        public double CompletionDecimalPercentage => Math.Clamp(
            _missionProgressStopwatch.Elapsed / data.DurationToPerform, 0, 1);

        /// <summary>
        /// Reset status to default for use in unity editor (otherwise values will persist if restarting the same scene).
        /// </summary>
        private void OnEnable()
        {
            status = MissionStatus.Inactive;
        }

        public void Post()
        {
            Debug.Assert(Status == MissionStatus.Inactive,
                $"{data.displayName} was told to Post but its status is {status} and is not allowed!");

            status = MissionStatus.Posted;
            _expiringStopwatch.Start();
        }

        public void Claim()
        {
            Debug.Assert(Status == MissionStatus.Posted,
                $"{data.displayName} was told to Claim but its status is {status} and is not allowed!");
            
            status = MissionStatus.Claimed;
        }
        
        public void StartMission()
        {
            Debug.Assert(Status == MissionStatus.Claimed,
                $"{data.displayName} was told to StartMission but its status is {status} and is not allowed!");
            
             _expiringStopwatch.Stop();
             _expiringStopwatch.Reset();
             status = MissionStatus.InProgress;
             _missionProgressStopwatch.Start();
        }
        
        public MissionStatus FetchCurrentStatus()
        {
            if (status == MissionStatus.Posted && ExpirationDecimalPercentage >= 1) MissionExpired();
            else if (status == MissionStatus.InProgress && CompletionDecimalPercentage >= 1) MissionFinished();
            return status;
        }

        private void MissionExpired()
        {
            _expiringStopwatch.Stop();
            
            Expired?.Invoke(this);

            status = MissionStatus.Expired;
        }

        private void MissionFinished()
        {
            _missionProgressStopwatch.Stop();
            
            Completed?.Invoke(this);
            
            status = MissionStatus.Completed;
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