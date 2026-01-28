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
            // Reset for re-running the same scene in the editor.
            mission = null;
        }

        public void Set(Mission newMission)
        {
            mission = newMission;
            // Even if newMission is the same as mission, still raise the event because the user clicked on a button;
            // so we should show them the UI if it's not already shown.
            if (Changed != null)
            {
                Changed.Invoke(mission);
            }
            else
            {
                Debug.LogWarning("MissionWrapper.Set: No listener was set.");
            }
        }

        public void Reset()
        {
            Set(null);
        }
    }
}