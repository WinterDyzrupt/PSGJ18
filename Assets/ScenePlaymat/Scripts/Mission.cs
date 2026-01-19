using System;
using UnityEngine.Serialization;

namespace ScenePlaymat.Scripts
{
    [Serializable]
    public class Mission
    {
        public string guid;

        public string name;
        public string description;
        public string completionText;
        public string failureText;

        public float timeToTravelInSeconds;
        public float timeToCompleteMissionInSeconds;
        public float timeForRestingAfterMissionInSeconds;

        public TimeSpan MissionTotalTime => TimeSpan.FromSeconds(
            timeToCompleteMissionInSeconds
            + 2 * timeToTravelInSeconds
            + timeForRestingAfterMissionInSeconds);

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