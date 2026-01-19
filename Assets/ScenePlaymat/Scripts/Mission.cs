namespace ScenePlaymat.Scripts
{
    public class Mission
    {
        public string Guid;

        public string Name;
        public string Description;
        public string CompletionText;
        public string FailureText;
        
        public float TimeToTravel;
        public float TimeToCompleteMission;
        public float TimeForRestingAfterMission;

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
