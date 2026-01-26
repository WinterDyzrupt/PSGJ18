using ScenePlaymat.Data.Missions;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionListPanel : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject missionFrame;
        [SerializeField] private Transform content;
        
        [Header("Mission Wrappers")]
        [SerializeField] private MissionWrapper newMission;

        private void Awake()
        {
            Debug.Assert(missionFrame != null, $"{name} doesn't have the Mission Frame prefab assigned!");
            Debug.Assert(content != null, $"{name} doesn't have the Content object assigned!");
            Debug.Assert(newMission != null, $"{name} doesn't have the New Mission wrapper assigned!");
        }

        private void Start()
        {
            newMission.Changed += AddMission;
        }
        
        private void OnDestroy()
        {
            newMission.Changed -= AddMission;
        }

        private void AddMission(Mission mission)
        {
            Debug.Assert(mission != null, $"{name}: was given a null mission!");

            Instantiate(missionFrame, content);
        }
    }
}
