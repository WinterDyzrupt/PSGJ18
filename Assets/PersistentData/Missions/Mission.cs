namespace PersistentData.Missions
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Mission", menuName = "Persistent Data/Missions/Mission")]
    public class Mission : ScriptableObject
    {
        public int defaultDurationToDisplayBeforeExpirationInSeconds = 10;
        public TimeSpan DurationToDisplayBeforeExpiration => TimeSpan.FromSeconds(defaultDurationToDisplayBeforeExpirationInSeconds);
        public MissionData data;
    }
}