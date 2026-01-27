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

        public AgentStatus Status { get; private set; }

        private Mission _currentMission;
        
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

        // public event Action<AgentStatus, float> ProgressUpdate;
        
        /// <summary>
        /// Initialization since this is a Scriptable Object
        /// SO's save information between run sessions. This is protection
        /// so that the values don't carry over between runs. 
        /// </summary>
        // TODO: Check to verify these are necessary / need more in other places
        // ScriptableObject.Awake() is not called when restarting the same scene in the editor,
        // so this is only for starting the game fresh, from the title scene.  OnEnable() is also executed
        // when restarting the same scene, so probably put any necessary initializations in OnEnable() instead.
        // private void Awake()
        // {
        //     Debug.Log($"{DisplayName} initialized.");
        //     Status = AgentStatus.Idle;
        //     _currentMission = null;
        //
        //     StatusChanged = null;
        //     
        //     attributes.muscleMod = 0;
        //     attributes.auraMod = 0;
        //     attributes.improvisationMod = 0;
        //     attributes.resilienceMod = 0;
        //     attributes.swiftnessMod = 0;
        // }

        public void AcceptMission(Mission mission)
        {
            Debug.Assert(mission != null, $"Expected {nameof(mission)} to be populated.");
            Debug.Assert(mission.data != null, $"Expected {nameof(mission)}.{nameof(mission.data)} to be populated.");
            Debug.Assert(_currentMission == null, $"{DisplayName} already has an active mission.");
            
            _currentMission = mission;
            _currentMission.Claim();
            _currentMission.Completed += ActiveMissionCompleted;
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

        private void ActiveMissionCompleted(Mission _)
        {
            _currentMission.Completed -= ActiveMissionCompleted;
            ChangeStatus(AgentStatus.Returning);
            _currentMission =  null;
            _returningStopwatch.Start();
        }

        public AgentStatus FetchCurrentStatus()
        {
            switch (Status)
            {
                case AgentStatus.Idle: // Do nothing
                    break;
                case AgentStatus.Deploying: // Check to see if agent arrived
                    if (CompletionOfDeploying >= 1)
                    {
                        _deployingStopWatch.Stop();
                        _currentMission.StartMission();
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

            return Status;
        }

        private void ChangeStatus(AgentStatus newStatus)
        {
            Status = newStatus;
            StatusChanged?.Invoke(Status);
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