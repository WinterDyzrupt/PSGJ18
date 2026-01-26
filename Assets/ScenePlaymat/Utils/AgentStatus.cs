namespace ScenePlaymat.Utils
{
    public enum AgentStatus
    {
        Idle, // Ready for a Mission
        Deploying, // Traveling to the Mission
        AttemptingMission, // Attempting the Mission
        Returning, // Returning from the Mission
        Resting // Resting after the mission
    }
}
