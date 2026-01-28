using System;
using System.Collections;
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

        [SerializeField] private AgentStatus status;
        public AgentStatus Status => status;

        [SerializeField] private Mission currentMission;
        public Mission CurrentMission => currentMission;
        
        private TimeSpan _deployingDuration;
        private TimeSpan _returningDuration;
        private TimeSpan _restingDuration;
        private readonly Stopwatch _deployingStopWatch = new();
        private readonly Stopwatch _returningStopwatch =  new();
        private readonly Stopwatch _restingStopwatch = new();

        public double CompletionOfDeploying => Math.Clamp(
            _deployingStopWatch.Elapsed / _deployingDuration, 0, 1);
        public double CompletionOfReturning => Math.Clamp(
            _returningStopwatch.Elapsed / _returningDuration, 0, 1);
        public double CompletionOfResting => Math.Clamp(
            _restingStopwatch.Elapsed / _restingDuration, 0, 1);

        public event Action<AgentStatus> StatusChanged;

        private void OnEnable()
        {
            // Reset state to default for restarting the scene in the editor.
            status = AgentStatus.Idle;
            currentMission = null;
        }

        public IEnumerator StartMissionAsync(Mission mission)
        {
            Debug.Assert(mission != null, $"Expected {nameof(mission)} to be populated.");
            Debug.Assert(mission.data != null, $"Expected {nameof(mission)}.{nameof(mission.data)} to be populated.");
            Debug.Assert(currentMission == null, $"{DisplayName} already has an active mission.");
            
            currentMission = mission;
            currentMission.AssignAgent(this);
            _deployingDuration = mission.data.DurationToTravelTo;
            _returningDuration = mission.data.DurationToReturnFrom;
            _restingDuration = mission.data.DurationToRest;

            _deployingStopWatch.Reset();
            _returningStopwatch.Reset();
            _restingStopwatch.Reset();

            ChangeStatus(AgentStatus.Deploying);
            _deployingStopWatch.Start();
            yield return new WaitForSeconds(mission.data.durationToTravelToInSeconds);
            _deployingStopWatch.Stop();
            
            yield return mission.PerformAsync();
            currentMission = null;
            
            ChangeStatus(AgentStatus.Returning);
            _returningStopwatch.Start();
            yield return new  WaitForSeconds(mission.data.durationToReturnFromInSeconds);
            _returningStopwatch.Stop();
            
            ChangeStatus(AgentStatus.Resting);
            _restingStopwatch.Start();
            yield return new WaitForSeconds(mission.data.durationToRestAfterInSeconds);
            _restingStopwatch.Stop();
            
            ChangeStatus(AgentStatus.Idle);
        }

        private void ChangeStatus(AgentStatus newStatus)
        {
            status = newStatus;
            StatusChanged?.Invoke(status);
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
                case AgentStatus.Deploying:
                    currentStopwatch = _deployingStopWatch;
                    break;
                case AgentStatus.Returning:
                    currentStopwatch = _returningStopwatch;
                    break;
                case AgentStatus.Resting:
                    currentStopwatch = _restingStopwatch;
                    break;
                case AgentStatus.Idle:
                case AgentStatus.AttemptingMission:
                default:
                    //Debug.Log($"Agent {DisplayName} in status: {Status}; paused, but not stopwatch to stop/start.");
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

        public override string ToString() => agentName;
        
        /*
        private float totalDurationEx = 10f;
        private float timerTime;
        
        bool timerIsRunning = true;
        
        private IEnumerator StateTimer()
        {
            // p1
            timerTime = 0;
            
            // p2
            while (timerTime <= totalDurationEx)
            {
                ProgressUpdate?.Invoke(Status, timerTime / totalDurationEx);
                timerTime += Time.deltaTime;
                
                if (timerIsRunning) yield return null;
                else
                {
                    // do what happens if timer ends early
                }
            }
            
            // p3
            switch (Status)
            {
                case AgentStatus.Idle: // no timer, coroutine would not be running
                    break;
                case AgentStatus.Deploying: // timer
                    break;
                case AgentStatus.AttemptingMission: // no timer, coroutine would not be running
                    break;
                case AgentStatus.Returning: // timer
                    break;
                case AgentStatus.Resting: // timer
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        */
    }
}