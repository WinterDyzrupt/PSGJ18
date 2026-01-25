using System;
using System.Diagnostics;
using UnityEngine;
using ScenePlaymat.Utils;
using ScenePlaymat.Data.Missions;
using Debug = UnityEngine.Debug;

namespace ScenePlaymat.Data.Agents
{
    [CreateAssetMenu(fileName = "Agent", menuName = "Data/Agent/Agent")]
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
        
        private TimeSpan _deployingDuration;
        private TimeSpan _returningDuration;
        private TimeSpan _restingDuration;
        private readonly Stopwatch _deployingStopWatch = new();
        private readonly Stopwatch _returningStopwatch = new();
        private readonly Stopwatch _restingStopwatch = new();

        public double CompletionOfDeploying => Math.Clamp(
            _deployingStopWatch.Elapsed / _deployingDuration, 0, 1);
        public double CompletionOfReturning => Math.Clamp(
            _returningStopwatch.Elapsed / _returningDuration, 0, 1);
        public double CompletionOfResting => Math.Clamp(
            _restingStopwatch.Elapsed / _restingDuration, 0, 1);

        public event Action<AgentStatus> ChangeInStatus;
        
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
            _currentMission.AdvanceMission(); // Agent accepted mission, advance phase to Assigned
            _currentMission.HasBeenCompleted += MissionHasCompleted;
            ChangeStatus(AgentStatus.Deploying);
            
            _deployingDuration = mission.data.DurationToTravelTo;
            _returningDuration = mission.data.DurationToReturnFrom;
            _restingDuration = mission.data.DurationToRest;

            _deployingStopWatch.Reset();
            _returningStopwatch.Reset();
            _restingStopwatch.Reset();
            
            _deployingStopWatch.Start();
            
            // Debug.Log($"Agent {AgentName} accepted the mission: {_currentMission.missionName}");
        }

        private void MissionHasCompleted(Mission _)
        {
            ChangeStatus(AgentStatus.Returning);
            _returningStopwatch.Start();
        }

        public void CheckAgentTimers()
        {
            switch (Status)
            {
                case AgentStatus.Idle: // Do nothing
                    break;
                case AgentStatus.Deploying: // Check to see if agent arrived
                    if (CompletionOfDeploying >= 1)
                    {
                        _deployingStopWatch.Stop();
                        _currentMission.AdvanceMission(); // Agent is starting mission
                        ChangeStatus(AgentStatus.AttemptingMission);
                    }
                    break;
                case AgentStatus.AttemptingMission: // Do nothing
                    break;
                case AgentStatus.Returning: // Check to see if agent returned
                    if (CompletionOfReturning >= 1)
                    {
                        _returningStopwatch.Stop();
                        ChangeStatus(AgentStatus.Resting);
                        _restingStopwatch.Start();
                    }
                    break;
                case AgentStatus.Resting: // check to see if agent rested
                    if (CompletionOfResting >= 1)
                    {
                        _returningStopwatch.Stop();
                        ChangeStatus(AgentStatus.Idle);
                    }
                    break;
                default:
                    Debug.LogError($"{DisplayName} had impossible status of {Status}.");
                    break;
            }
        }

        private void ChangeStatus(AgentStatus newStatus)
        {
            Status = newStatus;
            ChangeInStatus?.Invoke(Status);
        }

        public override string ToString() => agentName;
    }
}