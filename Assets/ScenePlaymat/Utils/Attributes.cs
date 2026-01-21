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

        [HideInInspector] public int muscleMod;
        [HideInInspector] public int auraMod;
        [HideInInspector] public int improvisationMod;
        [HideInInspector] public int resilienceMod;
        [HideInInspector] public int swiftnessMod;

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
    }
}