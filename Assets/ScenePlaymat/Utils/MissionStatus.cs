namespace ScenePlaymat.Utils
{
    public enum MissionStatus
    {
        /// <summary>
        /// Default State: The mission has not yet been made available.
        /// </summary>
        Inactive,
        
        /// <summary>
        /// The mission is waiting for an agent to be assigned
        /// </summary>
        Posted,
        
        /// <summary>
        /// The mission has been assigned to an agent, waiting for agent to perform the mission
        /// </summary>
        Assigned,
        
        /// <summary>
        /// The mission is being carried out
        /// </summary>
        InProgress,
        
        /// <summary>
        /// The mission was completed by the agent successfully
        /// </summary>
        Successful,
        
        /// <summary>
        /// The mission lingered in the Posted state for too long
        /// </summary>
        Expired,
        
        /// <summary>
        /// The mission was completed by the agent successfully
        /// </summary>
        Failed
        
    }
}