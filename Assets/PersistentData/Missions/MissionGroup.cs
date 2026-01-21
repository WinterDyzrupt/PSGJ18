namespace PersistentData.Missions
{
    using System.Collections.Generic; 
    using UnityEngine;

    [CreateAssetMenu(fileName = "Missions", menuName = "Persistent Data/Missions/MissionGroup")]
    public class MissionGroup : ScriptableObject
    {
        public List<Mission> missions = new();
    }
}