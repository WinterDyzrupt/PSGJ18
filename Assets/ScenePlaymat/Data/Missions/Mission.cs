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

        public MissionStatus Status { get; private set; }
        
        private TimeSpan _currentStatusDuration;
        private Stopwatch _expiringStopwatch;
        private Stopwatch _missionProgressStopwatch;
        
        public event Action<Mission> Expired;
        public event Action<Mission> Completed;
        
        public double ExpirationDecimalPercentage => Math.Clamp(
            _expiringStopwatch.Elapsed / data.DurationToExpire, 0, 1);
        public double CompletionDecimalPercentage => Math.Clamp(
            _missionProgressStopwatch.Elapsed / data.DurationToPerform, 0, 1);

        /// <summary>
        /// Initialization since this is a Scriptable Object
        /// SO's save information between run sessions. This is protection
        /// so that the values don't carry over between runs. 
        /// </summary>
        // TODO: Check To verify these are necessary / need more in other places
        public void Awake()
        {
            Status = MissionStatus.Inactive;
            
            _expiringStopwatch = new();
            _missionProgressStopwatch = new();
            
            Expired = null;
            Completed = null;
        }

        public void Post()
        {
            Debug.Assert(Status == MissionStatus.Inactive,
                $"{data.displayName} was told to Post but its status is {Status} and is not allowed!");

            Status = MissionStatus.Posted;
            _expiringStopwatch.Start();
        }

        public void Claim()
        {
            Debug.Assert(Status == MissionStatus.Posted,
                $"{data.displayName} was told to Claim but its status is {Status} and is not allowed!");
            
            Status = MissionStatus.Claimed;
        }
        
        public void StartMission()
        {
            Debug.Assert(Status == MissionStatus.Claimed,
                $"{data.displayName} was told to StartMission but its status is {Status} and is not allowed!");
            
             _expiringStopwatch.Stop();
             _expiringStopwatch.Reset();
             Status = MissionStatus.InProgress;
             _missionProgressStopwatch.Start();
        }
        
        public MissionStatus FetchCurrentStatus()
        {
            if (Status == MissionStatus.Posted && ExpirationDecimalPercentage >= 1) MissionExpired();
            else if (Status == MissionStatus.InProgress && CompletionDecimalPercentage >= 1) MissionFinished();
            return Status;
        }

        private void MissionExpired()
        {
            _expiringStopwatch.Stop();
            
            Expired?.Invoke(this);

            Status = MissionStatus.Expired;
        }

        private void MissionFinished()
        {
            _missionProgressStopwatch.Stop();
            
            Completed?.Invoke(this);
            
            Status = MissionStatus.Completed;
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