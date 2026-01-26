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
    
    /// <summary>
    /// Unity serialization doesn't support SOs nested within SOs, so this is a tiny duplicate-ish class for
    /// serialization.
    /// </summary>
    [Serializable]
    public class MissionSerializationHelper
    {
        public MissionData data;
    }
}