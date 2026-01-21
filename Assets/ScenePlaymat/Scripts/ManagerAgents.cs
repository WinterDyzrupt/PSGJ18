using UnityEngine;
using Variables;

namespace ScenePlaymat.Scripts
{
    public class ManagerAgents : MonoBehaviour
    {
        public Agent agent1;
        public Agent agent2;
        public Agent agent3;
        public Agent agent4;
        public Agent agent5;

        private Agent[] Roster => new[]
        {
            agent1,
            agent2,
            agent3,
            agent4,
            agent5
        };

        private void Start()
        {
            Debug.Assert(agent1 != null, "Agent1 was not assigned in the inspector!");
            Debug.Assert(agent2 != null, "Agent2 was not assigned in the inspector!");
            Debug.Assert(agent3 != null, "Agent3 was not assigned in the inspector!");
            Debug.Assert(agent4 != null, "Agent4 was not assigned in the inspector!");
            Debug.Assert(agent5 != null, "Agent5 was not assigned in the inspector!");

            foreach (var agent in Roster)
            {
                agent.InitializeAgent();
            }
        }

        private void Update()
        {
            // If any agent is not Ready
            foreach (var agent in Roster)
            {
                if (agent.Status != AgentStatus.Idle)
                {
                    agent.AdvanceMission();
                }
            }
        }
    }
}