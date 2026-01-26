namespace ScenePlaymat.Data.Missions
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Missions", menuName = "Data/Missions/MissionGroup")]
    public class MissionGroup : ScriptableObject
    {
        public string displayName;
        public string description;
        public List<Mission> missions = new();
    }

    /// <summary>
    /// Helper object to help Unity serialize MissionGroup, Missions, and MissionData.
    /// The problem is that Unity's json serialization doesn't support nested ScriptableObjects, so MissionGroup.
    /// missions wasn't being serialized properly; this required a non-SO Mission object, which required a differently
    /// named mission-property.  To avoid having MissionGroup.serializableMissions or something, this class exists
    /// to use the same name (missions) as MissionGroup.
    /// </summary>
    [Serializable]
    public class MissionGroupSerializationHelper
    {
        public string displayName;
        public string description;
        public List<MissionSerializationHelper> missions;
    }
}