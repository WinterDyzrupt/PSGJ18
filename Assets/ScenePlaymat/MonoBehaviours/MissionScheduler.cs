namespace ScenePlaymat.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Timers;
    using UnityEngine;
    using UnityEngine.Events;
    using Data.Missions;

    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Schedules mission to show to the player.
    /// </summary>
    public class MissionScheduler : MonoBehaviour
    {
        public float durationBetweenNewMissionsInSeconds;

        public MissionGroup allMissions;

        // TODO: Avoid re-offering previously-offered missions?

        public TimeSpan DurationBetweenNewMissions => TimeSpan.FromSeconds(durationBetweenNewMissionsInSeconds);

        public UnityEvent raiseMissionEvent; 

        private bool _isInitialized;
        private bool _isRunning;
        
        /// <summary>
        /// Note: C#'s Timer is not supported in WebGL. 
        /// </summary>
        private Stopwatch _missionTimer;

        public void Awake()
        {
            _missionTimer = Stopwatch.StartNew();
            _isInitialized = true;
            _isRunning = false;
        }

        public void Start()
        {
            // TODO: It might be rude to start in Unity's Start(); re-evaluate later
            StartOrResume();
        }

        public void StartOrResume()
        {
            Debug.Log($"Attempted to resume: {nameof(_isInitialized)}: {_isInitialized}, {nameof(_isRunning)}: {_isRunning}");
            if (_isInitialized && !_isRunning)
            {
                Debug.Log("Starting mission scheduler.");
                _missionTimer.Start();
                _isRunning = true;
                StartCoroutine(Timer());
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

        IEnumerator Timer()
        {
            while (_isInitialized)
            {
                if (_isRunning)
                {
                    if (_missionTimer.Elapsed > DurationBetweenNewMissions)
                    {
                        Debug.Log("Raising mission.");
                        raiseMissionEvent.Invoke();
                        Debug.Log("Raised mission; restarting timer, duration: " + DurationBetweenNewMissions);
                        _missionTimer.Restart();
                    }
                }

                yield return null;
            }
        }

        public void OnDestroy()
        {
            _missionTimer.Stop();
            _isRunning = false;
            _isInitialized = false;
        }

        /// <summary>
        /// TODO: Does this work with Unity?  Unity-in-the-browser?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void RaiseMission(object sender, ElapsedEventArgs eventArgs)
        {
            Debug.Log("Mission raised.");
        }
    }
}
