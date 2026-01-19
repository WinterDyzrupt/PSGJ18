using UnityEngine;
using ScenePlaymat.Scripts;

namespace Variables
{
    [CreateAssetMenu(fileName = "Agent", menuName = "Object Variables/Agent")]
    public class Agent : ScriptableObject
    {
        [Header("Agent Specifics")]
        [SerializeField] private new string name;

        [SerializeField] private int muscleBase;
        [SerializeField] private int auraBase;
        [SerializeField] private int improvisationBase;
        [SerializeField] private int resilienceBase;
        [SerializeField] private int swiftnessBase;

        public Sprite portrait;
        
        [HideInInspector] public int muscleMod;
        [HideInInspector] public int auraMod;
        [HideInInspector] public int improvisationMod;
        [HideInInspector] public int resilienceMod;
        [HideInInspector] public int swiftnessMod;
        
        public string Name => name;
        
        public int Muscle => muscleBase + muscleMod;
        public int Aura => auraBase + auraMod;
        public int Improvisation => improvisationBase + improvisationMod;
        public int Resilience => resilienceBase + resilienceMod;
        public int Swiftness => swiftnessBase + swiftnessMod;


        public AgentStatus Status { get; private set; } = AgentStatus.Ready;

        private Mission _currentMission;
        private float _timeInStatus;
        private float _currentTargetTime;
        public float CompletionOfCurrentStatus => _timeInStatus / _currentTargetTime;

        public int[] AttributesBase => new[]
        {
            muscleBase,
            auraBase,
            improvisationBase,
            resilienceBase,
            swiftnessBase
        };
        
        public int[] AttributesMod => new[]
        {
            muscleMod,
            auraMod,
            improvisationMod,
            resilienceMod,
            swiftnessMod,
        };
        
        public int[] Attributes => new[]
        {
            Muscle,
            Aura,
            Improvisation,
            Resilience,
            Swiftness
        };

        public void InitializeAgent()
        {
            muscleMod = 0;
            auraMod = 0;
            improvisationMod = 0;
            resilienceMod = 0;
            swiftnessMod = 0;
        }

        public void AcceptMission(Mission mission)
        {
            _currentMission = mission;
            Status = AgentStatus.Traveling;
            _currentTargetTime = mission.TimeToTravel;
        }

        public void AdvanceMission(float deltaTime)
        {
            _timeInStatus += deltaTime;
            
            if (!(_timeInStatus >= _currentTargetTime)) return;
            switch (Status)
            {
                case AgentStatus.Traveling:
                    Status = AgentStatus.AttemptingMission;
                    _currentTargetTime = _currentMission.TimeToCompleteMission;
                    break;
                case AgentStatus.AttemptingMission:
                    Status =  AgentStatus.Returning;
                    _currentTargetTime = _currentMission.TimeToTravel;
                    break;
                case AgentStatus.Returning:
                    Status = AgentStatus.Resting;
                    _currentTargetTime = _currentMission.TimeForRestingAfterMission;
                    break;
                case AgentStatus.Resting:
                    Status = AgentStatus.Ready;
                    break;
                case AgentStatus.Ready:
                default:
                    Debug.LogError($"Agent {Name} had unhandleable status of {Status}.");
                    break;
            }
            _timeInStatus = 0f;
        }

        public override string ToString() => name;
    }
}