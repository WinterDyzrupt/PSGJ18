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
        
        public event Action<MissionPhase> ChangeInPhase;
        public event Action<Mission> MissionAccomplished;
        
        public double ExpirationPercentage => Math.Clamp(
            _expiringStopwatch.Elapsed / data.DurationToExpiration, 0, 1);
        public double CompletionPercentage => Math.Clamp(
            _missionProgressStopwatch.Elapsed / data.DurationToPerform, 0, 1);

        public void StartMission()
        {
            
        }
        
        public void AdvanceMissionTimers()
        {
            switch (Phase)
            {
                case MissionPhase.Posted:
                    break;
                case MissionPhase.Pending:
                    break;
                case MissionPhase.Performing:
                    break;
                case MissionPhase.Completed:
                    break;
                default:
                    Debug.LogError($"Agent {name} had impossible phase of {Phase}.");
                    break;
            }
            // Debug.Log($"{name}'s phase has moved to {Phase}.");
            // ChangeInStatus?.Invoke(Status);
            // _currentStatusStopwatch.Restart();
        }
    }
}