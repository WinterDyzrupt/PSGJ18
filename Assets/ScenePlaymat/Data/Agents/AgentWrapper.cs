using System;
using UnityEngine;

namespace ScenePlaymat.Data.Agents
{
    [CreateAssetMenu(fileName = "AgentWrapper", menuName = "Data/Agent/Agent Wrapper")]
    public class AgentWrapper : ScriptableObject
    {
        public Agent Agent { get; private set; }

        public event Action<Agent> Changed;

        public void Set(Agent agent)
        {
            if (Agent == agent) return;

            Agent = agent;
            Changed?.Invoke(agent);
        }
    }
}