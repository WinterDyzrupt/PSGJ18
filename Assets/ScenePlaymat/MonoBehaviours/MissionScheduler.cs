namespace ScenePlaymat.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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

        public event Action<Mission> NewMissionAdded; 

        private bool _isInitialized;
        private bool _isRunning;

        private List<Mission>.Enumerator _missionEnumerator;

        private void Awake()
        {
            _missionEnumerator = allMissions.missions.GetEnumerator();
        }

        public void StartScheduler()
        {
            Debug.Log($"MissionScheduler: Starting: {nameof(_isInitialized)}: {_isInitialized}, {nameof(_isRunning)}: {_isRunning}.");

            if (!_isInitialized)
            {
                Debug.Log("MissionScheduler: Initializing.");
                _isInitialized = true;
                _isRunning = true;
                StartCoroutine(RaiseNewMissionAtInterval());
            }
            else
            {
                Debug.Log("MissionScheduler: Resuming.");
                _isRunning = true;
            }
        }

        public void StopScheduler()
        {
            _isRunning = false;
        }

        private IEnumerator RaiseNewMissionAtInterval()
        {
            while (_isInitialized && _isRunning)
            {
                Debug.Log($"MissionHandler: Waiting: {durationBetweenNewMissionsInSeconds} seconds.");
                yield return new WaitForSeconds(durationBetweenNewMissionsInSeconds);

                if (_missionEnumerator.MoveNext())
                {
                    var newMission = _missionEnumerator.Current;
                    if (newMission != null && NewMissionAdded != null)
                    {
                        NewMissionAdded.Invoke(newMission);
                    }
                    else if (newMission == null)
                    {
                        Debug.LogError("MissionHandler: Tried to raise a null mission; check the list.");
                    }
                    else
                    {
                        Debug.LogError($"MissionHandler: {nameof(NewMissionAdded)} is null.");
                    }
                }
                else
                {
                    Debug.LogWarning("MissionScheduler: Ran out of missions.");
                }
            }
        }

        public void OnDestroy()
        {
            _isInitialized = false;
            StopScheduler();
        }
    }
}
