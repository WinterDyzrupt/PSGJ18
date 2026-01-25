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
            if (Status == MissionStatus.Inactive)
            {
                Status = MissionStatus.Posted;
                _expiringStopwatch.Start();
            }
            else
            {
                Debug.LogError($"{name} was told to post but its status is {Status} and is not allowed!");
            }
        }

        public void Claim()
        {
            if (Status == MissionStatus.Posted)
            {
                Status = MissionStatus.Claimed;
            }
            else
            {
                Debug.LogError($"{name} was claimed but its status is {Status} and is not allowed!");
            }
        }
        
        public void StartMission()
        {
            if (Status == MissionStatus.Claimed)
            {
                _expiringStopwatch.Stop();
                _expiringStopwatch.Reset();
                Status = MissionStatus.InProgress;
                _missionProgressStopwatch.Start();
            }
            else
            {
                Debug.LogError($"{name} was started but its status is {Status} and is not allowed!");
            }
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
}