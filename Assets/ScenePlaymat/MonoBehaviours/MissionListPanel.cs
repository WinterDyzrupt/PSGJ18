using ScenePlaymat.Data.Missions;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionListPanel : MonoBehaviour
    {
        public MissionScheduler scheduler;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject missionFrame;
        [SerializeField] private Transform content;

        private void Awake()
        {
            Debug.Assert(missionFrame != null, $"{name} doesn't have the Mission Frame prefab assigned!");
            Debug.Assert(content != null, $"{name} doesn't have the Content object assigned!");
            Debug.Assert(scheduler != null, $"{name}: doesn't have a {nameof(scheduler)} assigned in the editor!");
        }

        private void Start()
        {
            scheduler.NewMissionAdded += AddMission;
            scheduler.StartScheduler();
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
            if (scheduler != null)
            {
                scheduler.NewMissionAdded -= AddMission;
                scheduler.StopScheduler();
            }
        }
    }
}
