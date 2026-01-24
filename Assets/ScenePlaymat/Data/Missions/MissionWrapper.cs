using System;
using UnityEngine;

namespace ScenePlaymat.Data.Missions
{
    [CreateAssetMenu(fileName = "MissionWrapper", menuName = "Data/Missions/Mission Wrapper")]
    public class MissionWrapper : ScriptableObject
    {
        public Mission Mission { get; private set; }

        public event Action<Mission> MissionHasChanged;

        public void ChangeMission(Mission mission)
        {
            if (Mission == mission) return;

            Mission = mission;
            MissionHasChanged?.Invoke(Mission);
        }
    }
}