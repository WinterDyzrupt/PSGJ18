using System;
using System.Diagnostics;
using UnityEngine;
using ScenePlaymat.Scripts;
using PersistentData.Missions;
using Debug = UnityEngine.Debug;

namespace Variables
{
    [CreateAssetMenu(fileName = "Agent", menuName = "Persistent Data/Agent")]
    public class Agent : ScriptableObject
    {
        [Header("Agent Information")]
        [SerializeField] private string agentName;
        public string DisplayName => agentName;
        
        public Sprite portrait;
        public Sprite mugshot;

        public Attributes attributes;

        [Header("Mission Information")]
        public AgentStatus Status { get; private set; } = AgentStatus.Idle;

        private Mission _currentMission;
        private TimeSpan _currentStatusDuration;
        private readonly Stopwatch _currentStatusStopwatch = new();
        private readonly Stopwatch _totalMissionStopwatch = new();

        public double CompletionOfCurrentStatus => Math.Clamp(
            _currentStatusStopwatch.Elapsed / _currentStatusDuration, 0, 1);
        public double CompletionOfMission => Math.Clamp(
            _totalMissionStopwatch.Elapsed / _currentMission.data.TotalDuration, 0, 1);

        /// <summary>
        /// Initialization Function since this is a Scriptable Object
        /// SO's save information between run sessions. This is protection
        /// so that the mod doesn't carry over between runs. 
        /// </summary>
        public void InitializeAgent()
        {
            attributes.muscleMod = 0;
            attributes.auraMod = 0;
            attributes.improvisationMod = 0;
            attributes.resilienceMod = 0;
            attributes.swiftnessMod = 0;
        }

        public void AcceptMission(Mission mission)
        {
            Debug.Assert(mission != null, $"Expected {nameof(mission)} to be populated.");
            Debug.Assert(mission.data != null, $"Expected {nameof(mission)}.{nameof(mission.data)} to be populated.");

            _currentMission = mission;
            Status = AgentStatus.Deploying;
            _currentStatusDuration = mission.data.DurationToTravelTo;

            _currentStatusStopwatch.Restart();
            _totalMissionStopwatch.Restart();
            
            // Debug.Log($"Agent {AgentName} accepted the mission: {_currentMission.missionName}");
        }

        public void AdvanceMission()
        {
            if (_currentStatusStopwatch.Elapsed >= _currentStatusDuration)
            {
                switch (Status)
                {
                    case AgentStatus.Deploying:
                        Status = AgentStatus.AttemptingMission;
                        _currentStatusDuration = _currentMission.data.DurationToPerform;
                        break;
                    case AgentStatus.AttemptingMission:
                        Status = AgentStatus.Returning;
                        _currentStatusDuration = _currentMission.data.DurationToReturnFrom;
                        break;
                    case AgentStatus.Returning:
                        Status = AgentStatus.Resting;
                        _currentStatusDuration = _currentMission.data.DurationToRest;
                        break;
                    case AgentStatus.Resting:
                        Status = AgentStatus.Idle;
                        _totalMissionStopwatch.Stop();
                        _currentStatusStopwatch.Stop();
                        // Debug.Log($"{name} completed their missions!");
                        // Debug.Log($"Status is 100%: {(CompletionOfCurrentStatus == 1f).ToString()}");
                        // Debug.Log($"Mission is 100%: {(CompletionOfMission == 1f).ToString()}");
                        return;
                    case AgentStatus.Idle:
                    default:
                        Debug.LogError($"Agent {DisplayName} had impossible status of {Status}.");
                        break;
                }

                // Debug.Log($"{name}'s status has moved to {Status}.");
                _currentStatusStopwatch.Restart();
            }
        }

        public override string ToString() => agentName;
    }
}