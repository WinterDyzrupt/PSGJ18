namespace ScenePlaymat.Utils
{
    public enum MissionStatus
    {
        Inactive, // Default State: The mission is in the bank
        Posted, // The mission is waiting for an agent, start expiring
        Expired, // The mission was left in Posted too long and expired
        Claimed, // The mission has been accepted, waiting for agent to arrive
        InProgress, // The mission is being carried out
        Completed // The mission was completed by the agent (success or failure)
    }
}