using System;
using System.Diagnostics;
using UnityEngine;
using ScenePlaymat.Scripts;
using Debug = UnityEngine.Debug;

namespace Variables
{
    [CreateAssetMenu(fileName = "Agent", menuName = "Object Variables/Agent")]
    public class Agent : ScriptableObject
    {
        [Header("Agent Specifics")] [SerializeField]
        private new string name;

        [SerializeField] private int muscleBase;
        [SerializeField] private int auraBase;
        [SerializeField] private int improvisationBase;
        [SerializeField] private int resilienceBase;
        [SerializeField] private int swiftnessBase;

        public Sprite portrait;

        [HideInInspector] public int muscleMod;
        [HideInInspector] public int auraMod;
        [HideInInspector] public int improvisationMod;
        [HideInInspector] public int resilienceMod;
        [HideInInspector] public int swiftnessMod;

        public string Name => name;

        public int Muscle => muscleBase + muscleMod;
        public int Aura => auraBase + auraMod;
        public int Improvisation => improvisationBase + improvisationMod;
        public int Resilience => resilienceBase + resilienceMod;
        public int Swiftness => swiftnessBase + swiftnessMod;


        public AgentStatus Status { get; private set; } = AgentStatus.Idle;

        private Mission _currentMission;

        public Stopwatch CurrentStatusStopwatch { get; private set; } = new();
        public Stopwatch TotalMissionStopwatch { get; private set; } = new();
        private TimeSpan _currentTargetTime;

        public double CompletionOfCurrentStatus => Math.Clamp(
            CurrentStatusStopwatch.Elapsed / _currentTargetTime, 0, 1);

        public double CompletionOfMission => Math.Clamp(
            TotalMissionStopwatch.Elapsed / _currentMission.MissionTotalTime, 0, 1);

        public int[] AttributesBase => new[]
        {
            muscleBase,
            auraBase,
            improvisationBase,
            resilienceBase,
            swiftnessBase
        };

        public int[] AttributesMod => new[]
        {
            muscleMod,
            auraMod,
            improvisationMod,
            resilienceMod,
            swiftnessMod,
        };

        public int[] Attributes => new[]
        {
            Muscle,
            Aura,
            Improvisation,
            Resilience,
            Swiftness
        };

        public void InitializeAgent()
        {
            muscleMod = 0;
            auraMod = 0;
            improvisationMod = 0;
            resilienceMod = 0;
            swiftnessMod = 0;
        }

        public void AcceptMission(Mission mission)
        {
            _currentMission = mission;
            Status = AgentStatus.Deploying;
            _currentTargetTime = TimeSpan.FromSeconds(mission.timeToTravelInSeconds);

            CurrentStatusStopwatch.Restart();
            TotalMissionStopwatch.Restart();
        }

        public void AdvanceMission()
        {
            if (CurrentStatusStopwatch.Elapsed >= _currentTargetTime)
            {
                switch (Status)
                {
                    case AgentStatus.Deploying:
                        Status = AgentStatus.AttemptingMission;
                        _currentTargetTime = TimeSpan.FromSeconds(
                            _currentMission.timeToCompleteMissionInSeconds);
                        break;
                    case AgentStatus.AttemptingMission:
                        Status = AgentStatus.Returning;
                        _currentTargetTime = TimeSpan.FromSeconds(
                            _currentMission.timeToTravelInSeconds);
                        break;
                    case AgentStatus.Returning:
                        Status = AgentStatus.Resting;
                        _currentTargetTime = TimeSpan.FromSeconds(
                            _currentMission.timeForRestingAfterMissionInSeconds);
                        break;
                    case AgentStatus.Resting:
                        Status = AgentStatus.Idle;
                        TotalMissionStopwatch.Stop();
                        CurrentStatusStopwatch.Stop();
                        // Debug.Log($"{name} completed their missions!");
                        // Debug.Log($"Status is 100%: {(CompletionOfCurrentStatus == 1f).ToString()}");
                        // Debug.Log($"Mission is 100%: {(CompletionOfMission == 1f).ToString()}");
                        return;
                    case AgentStatus.Idle:
                    default:
                        Debug.LogError($"Agent {Name} had unhandleable status of {Status}.");
                        break;
                }

                // Debug.Log($"{name}'s status has moved to {Status}.");
                CurrentStatusStopwatch.Restart();
            }
        }

        public override string ToString() => name;
    }
}