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
            // TODO: remove this after agent panel disappears to see if this is needed.
            if (Agent == newAgent) return;

            // Even if newAgent is the same as current agent, raise the event because the user clicked on a button, so
            // we should update the UI.
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