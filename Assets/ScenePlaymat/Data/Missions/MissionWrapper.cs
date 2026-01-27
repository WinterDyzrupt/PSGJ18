using System;
using UnityEngine;

namespace ScenePlaymat.Data.Missions
{
    [CreateAssetMenu(fileName = "MissionWrapper", menuName = "Data/Missions/Mission Wrapper")]
    public class MissionWrapper : ScriptableObject
    {
        [SerializeField] private Mission mission;
        public Mission Mission => mission;

        public event Action<Mission> Changed;

        private void OnEnable()
        {
            mission = null;
        }

        public void Set(Mission newMission)
        {
            if (mission == newMission)
            {
                Debug.Log("MissionWrapper.Set: Attempted to change mission to the same mission.");
                return;
            }

            mission = newMission;
            if (Changed != null)
            {
                Changed.Invoke(mission);
            }
            else
            {
                Debug.LogWarning("MissionWrapper.Set: No listener was set.");
            }
        }
    }
}