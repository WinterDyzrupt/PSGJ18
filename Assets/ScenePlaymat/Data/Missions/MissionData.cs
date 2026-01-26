using System;
using ScenePlaymat.Utils;

namespace ScenePlaymat.Data.Missions
{
    /// <summary>
    /// Wrapper of data related to a mission to extract from a file.
    /// </summary>
    [Serializable]
    public class MissionData
    {
        public string guid;

        public string displayName;
        public string description;
        public string completionText;
        public string failureText;

        public float durationToTravelToInSeconds = 10;
        public float durationToPerformInSeconds = 10;
        public float durationToReturnFromInSeconds = 10;
        public float durationToRestAfterInSeconds = 10;
        public float durationToExpirationInSeconds = 10;

        public Attributes missionAttributes;
        
        public TimeSpan DurationToTravelTo => TimeSpan.FromSeconds(durationToTravelToInSeconds);
        public TimeSpan DurationToPerform => TimeSpan.FromSeconds(durationToPerformInSeconds);
        public TimeSpan DurationToReturnFrom => TimeSpan.FromSeconds(durationToReturnFromInSeconds);
        public TimeSpan DurationToRest => TimeSpan.FromSeconds(durationToRestAfterInSeconds);
        public TimeSpan DurationToExpire => TimeSpan.FromSeconds(durationToExpirationInSeconds);

        public TimeSpan TotalDuration => TimeSpan.FromSeconds(
            durationToTravelToInSeconds +
            durationToPerformInSeconds +
            durationToReturnFromInSeconds +
            durationToRestAfterInSeconds);
    }
}