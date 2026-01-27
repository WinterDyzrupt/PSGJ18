using System;
using UnityEngine;

namespace ScenePlaymat.Data.Agents
{
    [CreateAssetMenu(fileName = "AgentWrapper", menuName = "Data/Agent/Agent Wrapper")]
    public class AgentWrapper : ScriptableObject
    {
        [SerializeField] private Agent agent;
        public Agent Agent => agent;

        public event Action<Agent> Changed;

        private void OnEnable()
        {
            // Reset to null for re-running the same scene in the editor.
            agent = null;
        }

        public void Set(Agent newAgent)
        {
            if (Agent == newAgent) return;

            agent = newAgent;
            if (Changed != null)
            {
                Changed?.Invoke(agent);
                Debug.Log("Agent.Changed invoked");
            }
            else
            {
                Debug.Log("Agent.Changed is null, but agent just changed.");
            }
        }
    }
}