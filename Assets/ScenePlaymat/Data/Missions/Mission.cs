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

        public MissionPhase Phase { get; private set; } = MissionPhase.Posting;
        
        private TimeSpan _currentPhaseDuration;
        private readonly Stopwatch _expiringStopwatch = new();
        private readonly Stopwatch _missionProgressStopwatch = new();
        
        public event Action<Mission> HasExpired;
        public event Action<Mission> HasBeenCompleted;
        
        public double ExpirationPercentage => Math.Clamp(
            _expiringStopwatch.Elapsed / data.DurationToExpiration, 0, 1);
        public double CompletionPercentage => Math.Clamp(
            _missionProgressStopwatch.Elapsed / data.DurationToPerform, 0, 1);

        public void AdvanceMissionPhase()
        {
            switch (Phase)
            {
                case MissionPhase.Posting: // Start Expiration Timer, From Mission Frame
                    Phase = MissionPhase.Posted;
                    _expiringStopwatch.Start();
                    break;
                case MissionPhase.Posted: // Mission was Assigned to an Agent, From Agent
                    Phase = MissionPhase.Assigned;
                    break;
                case MissionPhase.Assigned:// Agent Arrived, Start Performing, From Agent
                    _expiringStopwatch.Stop();
                    _expiringStopwatch.Reset();
                    Phase = MissionPhase.Performing;
                    _missionProgressStopwatch.Start();
                    break;
                case MissionPhase.Performing:
                case MissionPhase.Expired:
                case MissionPhase.Completed:
                default:
                    Debug.LogWarning($"{name} was told to advance but was in the {Phase} phase and isn't allowed.");
                    break;
            }
        }
        
        public void CheckMissionTimers()
        {
            if (Phase == MissionPhase.Posted && ExpirationPercentage >= 1) MissionExpired();
            else if (Phase == MissionPhase.Performing && CompletionPercentage >= 1) MissionFinished();
        }

        private void MissionExpired()
        {
            _expiringStopwatch.Stop();
            
            HasExpired?.Invoke(this);

            Phase = MissionPhase.Expired;
        }

        private void MissionFinished()
        {
            _missionProgressStopwatch.Stop();
            
            HasBeenCompleted?.Invoke(this);
            
            Phase = MissionPhase.Completed;
        }

        public override string ToString() => data.displayName;
    }
}