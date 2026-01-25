namespace ScenePlaymat.Utils
{
    public enum MissionPhase
    {
        Posting, // Default State: The mission is getting posted
        Posted, // The mission is waiting for an agent, start expiring
        Expired, // The mission was left in Posted too long and expired
        Assigned, // The mission has been accepted, waiting for agent to arrive
        Performing, // The mission is being carried out
        Completed // The mission was completed by the agent (success or failure)
    }
}