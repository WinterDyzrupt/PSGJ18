namespace ScenePlaymat.Data.Missions
{
    using System.Collections.Generic; 
    using UnityEngine;

    [CreateAssetMenu(fileName = "Missions", menuName = "Data/Missions/MissionGroup")]
    public class MissionGroup : ScriptableObject
    {
        public List<Mission> missions = new();
    }
}