namespace ScenePlaymat.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEngine;
    using Data.Missions;

    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Schedules new missions to show to the player.
    /// </summary>
    public class MissionScheduler : MonoBehaviour
    {
        public float durationBetweenNewMissionsInSeconds;

        public MissionGroup allMissions;

        // TODO: Do something interesting (e.g. randomization) when offering missions?
        // TODO: Reuse at least some missions (or have infinite missions) ... or...
        // ... prevent a situation where you can run out of missions before without filling a bar
        // ... (e.g. final missions give +/- 100%)

        private TimeSpan DurationBetweenNewMissions => TimeSpan.FromSeconds(durationBetweenNewMissionsInSeconds);

        public event Action<Mission> NewMissionAdded; 
        
        /// <summary>
        /// Note: C#'s Timer is not supported in WebGL.
        /// </summary>
        private Stopwatch _missionTimer;
        private bool _isInitialized;
        private bool _isRunning;

        private List<Mission>.Enumerator _missionEnumerator;

        private void Awake()
        {
            _missionTimer = Stopwatch.StartNew();
            _isInitialized = true;
            _isRunning = false;
            _missionEnumerator = allMissions.missions.GetEnumerator();
        }

        /// <summary>
        /// Starts or resumes the scheduler's timer.
        /// </summary>
        public void StartOrResume()
        {
            Debug.Log($"Attempted to resume: {nameof(_isInitialized)}: {_isInitialized}, {nameof(_isRunning)}: {_isRunning}");
            if (_isInitialized && !_isRunning)
            {
                Debug.Log("Starting mission scheduler.");
                _missionTimer.Start();
                _isRunning = true;
                StartCoroutine(RaiseNewMissionAtInterval());
            }
        }

        public void Pause()
        {
            Debug.Log($"Attempted to pause: {nameof(_isInitialized)}: {_isInitialized}, {nameof(_isRunning)}: {_isRunning}");
            if (_isInitialized && _isRunning)
            {
                Debug.Log("Pausing mission scheduler.");
                _missionTimer.Stop();
                _isRunning = false;
            }
        }

        public void Stop()
        {
            _missionTimer?.Stop();
            _isRunning = false;
        }

        IEnumerator RaiseNewMissionAtInterval()
        {
            while (_isInitialized)
            {
                if (_isRunning && _missionTimer.Elapsed >= DurationBetweenNewMissions && NewMissionAdded != null)
                {
                    var hasNewMission = _missionEnumerator.MoveNext();
                    var newMission = _missionEnumerator.Current;

                    if (hasNewMission && newMission != null)
                    {
                        Debug.Log("MissionScheduler: New mission: " + newMission);

                        // Last-minute check in case the last listener stopped listening in the middle of this block.
                        if (NewMissionAdded != null)
                        {
                            NewMissionAdded.Invoke(newMission);
                        }
                        else
                        {
                            Debug.LogWarning($"MissionHandler: {nameof(NewMissionAdded)} is null; skipping.");
                        }
                    }
                    else if (hasNewMission && newMission == null)
                    {
                        Debug.LogWarning("MissionHandler: Tried to raise a null mission; check the list.");
                    }
                    else
                    {
                        Debug.LogWarning("MissionScheduler: Time to raise a new mission, but there are no more.");
                    }

                    Debug.Log("MissionScheduler: Restarting timer, duration: " + DurationBetweenNewMissions);
                    _missionTimer.Restart();
                }
                else if (_isRunning && _missionTimer.Elapsed >= DurationBetweenNewMissions && NewMissionAdded == null)
                {
                    Debug.LogWarning("MissionScheduler: Time to raise an event, but there are no listeners; why isn't this paused? ... restarting");
                    // TODO: Pause itself when there are no listeners?
                    _missionTimer.Restart();
                }

                yield return null;
            }
        }

        public void OnDestroy()
        {
            _isInitialized = false;
            Stop();
        }
    }
}
