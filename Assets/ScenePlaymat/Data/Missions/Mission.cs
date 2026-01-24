namespace ScenePlaymat.Data.Missions
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Mission", menuName = "Data/Missions/Mission")]
    public class Mission : ScriptableObject
    {
        public int defaultDurationToDisplayBeforeExpirationInSeconds = 10;
        public TimeSpan DurationToDisplayBeforeExpiration => TimeSpan.FromSeconds(defaultDurationToDisplayBeforeExpirationInSeconds);
        public MissionData data;

        public override string ToString()
        {
            return data.displayName;
        }
    }
}