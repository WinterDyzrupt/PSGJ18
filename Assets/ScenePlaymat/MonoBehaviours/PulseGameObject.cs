using System.Collections;
using UnityEngine;

namespace ScenePlaymat.MonoBehaviours
{
    /// <summary>
    /// Causes the game object that this is attached to to pulse over time.
    /// </summary>
    public class PulseGameObject : MonoBehaviour
    {
        public bool isPulsing;
        public float maxPulseRatio;
        public float minPulseRatio;
        public float currentPulseRatio;
        public float defaultPulseRatio;
        public float pulseSpeed;
        
        private void OnEnable()
        {
            isPulsing = true;
            StartCoroutine(PulseAsync(gameObject));
        }

        private void OnDisable()
        {
            isPulsing = false;
        }
        
        private IEnumerator PulseAsync(GameObject objectToPulse)
        {
            currentPulseRatio = defaultPulseRatio; 
            var targetPulseRatio = maxPulseRatio;
            
            while (isPulsing)
            {
                if (Mathf.Approximately(currentPulseRatio, minPulseRatio))
                {
                    targetPulseRatio = maxPulseRatio;
                }
                if (Mathf.Approximately(currentPulseRatio, maxPulseRatio))
                {
                    targetPulseRatio = minPulseRatio;
                }
                
                // TODO pulseSpeed modified by Time.deltaTime?
                currentPulseRatio = Mathf.MoveTowards(currentPulseRatio, targetPulseRatio, pulseSpeed);
                objectToPulse.transform.localScale = Vector3.one * currentPulseRatio;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}