using UnityEngine;
using UnityEngine.Events;

namespace ScenePlaymat.MonoBehaviours
{
    public class GameOverMonitor : MonoBehaviour
    {
        public UnityEvent pauseEvent;
        
        /// <summary>
        /// Called by a GameEventListener.
        /// </summary>
        public void OnVictory()
        {
            Debug.Log("Victory");
            pauseEvent.Invoke();
        }

        /// <summary>
        /// Called by a GameEventListener.
        /// </summary>
        public void OnDefeat()
        {
            Debug.Log("Defeat");
            pauseEvent.Invoke();
        }
    }
}