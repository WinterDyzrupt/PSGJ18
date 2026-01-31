using System.Collections;
using UnityEngine;

namespace Common.MonoBehaviours
{
    /// <summary>
    /// Causes the game object that this is attached to to pulse over time.
    /// </summary>
    public class PulseGameObject : MonoBehaviour
    {
        public bool pulseOnEnable = true; 
        public bool isPulsing;
        public float maxPulseRatio;
        public float minPulseRatio;
        public float currentPulseRatio;
        public float defaultPulseRatio;
        public float pulseSpeed;

        private void OnEnable()
        {
            // pulse on enable (pause buttons)
            // conditionally pulse on enable
            // pulse on/off while enabled
            if (pulseOnEnable)
            {
                StartPulsing();
            }
        }
        
        public void StartPulsing()
        {
            if (isActiveAndEnabled && !isPulsing)
            {
                isPulsing = true;
                StartCoroutine(PulseAsync(gameObject));
            }
            else
            {
                pulseOnEnable = true;
            }
        }

        private void OnDisable()
        {
            isPulsing = false;
        }

        public void StopPulsing()
        {
            if (isActiveAndEnabled)
            {
                isPulsing = false;
            }
            else
            {
                pulseOnEnable = false;
            }
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
                yield return null;
            }
        }
    }
}