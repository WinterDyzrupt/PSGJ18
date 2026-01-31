using System;
using UnityEngine;

namespace ScenePlaymat.Utils
{
    [Serializable]
    public class Attributes
    {
        [SerializeField] private int muscleBase;
        [SerializeField] private int auraBase;
        [SerializeField] private int improvisationBase;
        [SerializeField] private int resilienceBase;
        [SerializeField] private int swiftnessBase;

        public int muscleMod;
        public int auraMod;
        public int improvisationMod;
        public int resilienceMod;
        public int swiftnessMod;

        public int Muscle => muscleBase + muscleMod;
        public int Aura => auraBase + auraMod;
        public int Improvisation => improvisationBase + improvisationMod;
        public int Resilience => resilienceBase + resilienceMod;
        public int Swiftness => swiftnessBase + swiftnessMod;
        
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

        public int[] AttributesTotal => new[]
        {
            Muscle,
            Aura,
            Improvisation,
            Resilience,
            Swiftness
        };

        /// <summary>
        /// Gets the difference in attributes, Agent - Mission.
        /// If agent attribute is greater, it will return 0.
        /// </summary>
        /// <param name="agentAttributes"></param>
        /// <param name="missionAttributes"></param>
        /// <returns>
        /// Returns a negative value which indicates how many
        /// mission attribute points are not satisfied.
        /// </returns>
        public static int GetDifferenceInAttributes(Attributes agentAttributes, Attributes missionAttributes)
        {
            Debug.Assert(agentAttributes.AttributesTotal.Length == missionAttributes.AttributesTotal.Length,
                "Expected left and right attributes to have the same length");
            
            var attributeDifference = 0;
            for (var index = 0; index < agentAttributes.AttributesTotal.Length; index++)
            {
                attributeDifference +=
                    Mathf.Min(agentAttributes.AttributesTotal[index] - missionAttributes.AttributesTotal[index], 0);
            }
            
            return attributeDifference;
        }
    }
}