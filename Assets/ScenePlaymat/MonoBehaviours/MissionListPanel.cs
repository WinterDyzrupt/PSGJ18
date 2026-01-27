using ScenePlaymat.Data.Missions;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionListPanel : MonoBehaviour
    {
        private MissionScheduler _scheduler;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject missionFrame;
        [SerializeField] private Transform content;

        private void Awake()
        {
            Debug.Assert(missionFrame != null, $"{name} doesn't have the Mission Frame prefab assigned!");
            Debug.Assert(content != null, $"{name} doesn't have the Content object assigned!");
            
            _scheduler = GetComponentInChildren<MissionScheduler>(); 
            Debug.Assert(_scheduler != null, $"{name}: doesn't have a {nameof(MissionScheduler)} component!");
        }

        private void Start()
        {
            _scheduler.NewMissionAdded += AddMission;
            _scheduler.StartScheduler();
        }

        private void AddMission(Mission mission)
        {
            Debug.Assert(mission != null, $"{name}: was given a null mission!");

            Debug.Log($"MissionListPanel: Adding new mission: {mission.data.displayName}");
            var newMissionFrame = Instantiate(missionFrame, content);
            var missionFrameMonoBehaviour = newMissionFrame.GetComponentInChildren<MissionFrame>();
            missionFrameMonoBehaviour.PostMission(mission);
        }

        private void OnDestroy()
        {
            if (_scheduler != null)
            {
                _scheduler.NewMissionAdded -= AddMission;
                _scheduler.StopScheduler();
            }
        }
    }
}
