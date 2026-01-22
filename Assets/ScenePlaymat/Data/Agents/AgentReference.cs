using System;
using UnityEngine;

namespace ScenePlaymat.Data.Agents
{
    [CreateAssetMenu(fileName = "Agent", menuName = "Data/Agent/Agent Reference")]
    public class AgentReference : ScriptableObject
    {
        public Agent Agent { get; private set; }

        public event Action<Agent> AgentHasChanged;

        public void ChangeAgent(Agent agent)
        {
            if (Agent == agent) return;
            
            Agent = agent;
            AgentHasChanged?.Invoke(agent);
        }
    }
}