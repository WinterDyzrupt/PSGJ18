namespace ScenePlaymat.MonoBehaviours
{
    using UnityEngine;
    using Data.Missions;

    // TODO: Rename, Rahl probably already made a ManagerMission 
    public class MissionSchedulerHaver : MonoBehaviour
    {
        private MissionScheduler _missionScheduler;

        private void Awake()
        { 
            _missionScheduler = GetComponent<MissionScheduler>();
            _missionScheduler.NewMissionAdded += OnNewMission;
        }

        private void Start()
        {
            // TODO: Start after some delay or prompt?
            _missionScheduler.StartOrResume();
        }

        private void OnNewMission(Mission newMission)
        {
            Debug.Log("Adding new mission: " + newMission);
        }
    }
}