using System;
using UnityEngine;

namespace ScenePlaymat.Data.Missions
{
    [CreateAssetMenu(fileName = "MissionWrapper", menuName = "Data/Missions/Mission Wrapper")]
    public class MissionWrapper : ScriptableObject
    {
        public Mission Mission { get; private set; }

        public event Action<Mission> Changed;

        public void Set(Mission mission)
        {
            if (Mission == mission) return;

            Mission = mission;
            Changed?.Invoke(Mission);
        }
    }
}