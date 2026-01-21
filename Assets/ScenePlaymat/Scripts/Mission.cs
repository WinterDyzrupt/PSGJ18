using System;
using UnityEngine.Serialization;

namespace ScenePlaymat.Scripts
{
    [Serializable]
    public class Mission
    {
        public string guid;

        public string missionName;
        public string description;
        public string completionText;
        public string failureText;

        public float embarkInSeconds;
        public float performingMissionInSeconds;
        public float returnInSeconds;
        public float restingInSeconds;
        
        public TimeSpan EmbarkTimeSpan => TimeSpan.FromSeconds(embarkInSeconds);
        public TimeSpan PerformingMissionTimeSpan => TimeSpan.FromSeconds(performingMissionInSeconds);
        public TimeSpan ReturnTimeSpan => TimeSpan.FromSeconds(returnInSeconds);
        public TimeSpan RestingTimeSpan => TimeSpan.FromSeconds(restingInSeconds);

        public TimeSpan MissionTotalTime => TimeSpan.FromSeconds(
            embarkInSeconds +
            performingMissionInSeconds +
            returnInSeconds +
            restingInSeconds);

        /*******************
        No need for a constructor if everything is serializable
        I'm assuming we'll want to have the missions in sheets
        Which would be a much easier medium to create them.

        [Serializable] public class MissionArray { public Mission[] missions;}

        Then Load json file with

        Mission[] missions = JsonUtility.FromJson<MissionArray>(MissionsJson.text).missions;
        *******************/
    }
}