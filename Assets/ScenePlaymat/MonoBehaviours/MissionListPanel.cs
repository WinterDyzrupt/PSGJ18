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

        private void OnEnable()
        {
            newMission.MissionHasChanged += AddMission;
        }
        
        private void OnDisable()
        {
            newMission.MissionHasChanged -= AddMission;
        }

        private void AddMission(Mission mission)
        {
            if (mission == null)
            {
                Debug.LogError("Null Missions are not accepted.");
                return;
            }

            Instantiate(missionFrame, content);
        }
    }
}
