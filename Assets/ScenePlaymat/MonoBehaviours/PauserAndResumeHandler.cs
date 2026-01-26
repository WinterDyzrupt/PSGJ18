namespace ScenePlaymat.MonoBehaviours
{
    using UnityEngine;
    
    public class PauseAndResumeHandler : MonoBehaviour
    {
        /// <summary>
        /// This is currently called via GameEventListener listening for a GameEvent of type Pause.
        /// </summary>
        public void Pause()
        {
            Debug.Log("PauseAndResumeHandler: Pausing");
            Time.timeScale = 0;
        }

        /// <summary>
        /// This is currently called via GameEventListener listening for a GameEvent of type Resume.
        /// </summary>
        public void Resume()
        {
            Debug.Log("PauseAndResumeHandler: Resuming");
            Time.timeScale = 1;
        }
    }
}