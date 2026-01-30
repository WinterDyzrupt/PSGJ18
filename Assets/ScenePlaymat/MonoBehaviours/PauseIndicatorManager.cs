using System.Collections.Generic;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    public class PauseIndicatorManager : MonoBehaviour
    {
        public List<GameObject> pauseIndicators;

        private void Awake()
        {
            Debug.Assert(pauseIndicators?.Count > 0, $"Expected {nameof(pauseIndicators)} to be injected in the inspector.");
        }
        
        public void Paused()
        {
            pauseIndicators.ForEach(pauseIndicator => pauseIndicator.SetActive(true));
        }
        
        public void Resumed()
        {
            pauseIndicators.ForEach(pauseIndicator => pauseIndicator.SetActive(false));
        }
    }
}