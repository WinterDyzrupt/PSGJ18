namespace ScenePlaymat.Data.Missions
{
    using System;
    
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

        public float durationToTravelToInSeconds;
        public float durationToPerformInSeconds;
        public float durationToReturnFromInSeconds;
        public float durationToRestAfterInSeconds;
        
        public TimeSpan DurationToTravelTo => TimeSpan.FromSeconds(durationToTravelToInSeconds);
        public TimeSpan DurationToPerform => TimeSpan.FromSeconds(durationToPerformInSeconds);
        public TimeSpan DurationToReturnFrom => TimeSpan.FromSeconds(durationToReturnFromInSeconds);
        public TimeSpan DurationToRest => TimeSpan.FromSeconds(durationToRestAfterInSeconds);

        public TimeSpan TotalDuration => TimeSpan.FromSeconds(
            durationToTravelToInSeconds +
            durationToPerformInSeconds +
            durationToReturnFromInSeconds +
            durationToRestAfterInSeconds);
    }
}