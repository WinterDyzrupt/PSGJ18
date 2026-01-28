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
        public event Action<Mission> Reselected;

        private void OnEnable()
        {
            // Reset for re-running the same scene in the editor.
            mission = null;
        }

        public void Set(Mission newMission)
        {
            if (mission == newMission)
            {
                if (Reselected != null)
                {
                    Reselected.Invoke(mission);
                    Debug.Log("Mission Reselected");
                }
                else
                {
                    Debug.Log("Mission.Reselected is null, but mission was just selected.");
                }
            }
            else
            {
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
}