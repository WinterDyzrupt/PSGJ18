using ScenePlaymat.Data.Missions;
using UnityEngine;
using UnityEngine.UI;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionListPanel : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject missionButton;

        [Header("Content Panel")]
        [SerializeField] private Transform content;

        private void Awake()
        {
            Debug.Assert(missionButton != null, "Mission Button prefab is not assigned!");
            Debug.Assert(content != null, "Content transform is not assigned!");
        }

        public void AddMissions(Mission mission)
        {
            if (mission == null)
            {
                Debug.LogError("Null Missions are not accepted.");
                return;
            }

            var newButtonGameObject = Instantiate(missionButton, content);
        }

    }
}
