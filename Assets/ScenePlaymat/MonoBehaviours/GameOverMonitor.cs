using Common.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScenePlaymat.MonoBehaviours
{
    public class GameOverMonitor : MonoBehaviour
    {
        public UnityEvent pauseEvent;
        public UnityEvent resumeEvent;
        
        [Header("Components")]
        [SerializeField] private GameObject clickShieldGraphic;
        [SerializeField] private GameObject victoryGraphic;
        [SerializeField] private GameObject defeatGraphic;

        private void Awake()
        {
            Debug.Assert(pauseEvent != null, $"{name} is missing pauseEvent in inspector!");
            Debug.Assert(clickShieldGraphic != null, $"{name} is missing clickShieldGraphic in inspector!");
            Debug.Assert(victoryGraphic != null, $"{name} is missing victoryGraphic in inspector!");
            Debug.Assert(defeatGraphic != null, $"{name} is missing defeatGraphic in inspector!");
        }
        
        private void Start()
        {
            // Adding Resume invoke to handle pausing after victory/defeat
            resumeEvent.Invoke();
            
            clickShieldGraphic.SetActive(false);
            victoryGraphic.SetActive(false);
            defeatGraphic.SetActive(false);
        }
        
        public void MainMenuButton()
        {
            SceneManager.LoadScene(SceneData.TitleSceneIndex);
        }

        public void RestartButton()
        {
            SceneManager.LoadScene(SceneData.PlaymatSceneIndex);
        }
        
        /// <summary>
        /// Called by a GameEventListener.
        /// </summary>
        public void OnVictory()
        {
            Debug.Log("Victory");
            pauseEvent.Invoke();
            
            clickShieldGraphic.SetActive(true);
            victoryGraphic.SetActive(true);
        }

        /// <summary>
        /// Called by a GameEventListener.
        /// </summary>
        public void OnDefeat()
        {
            Debug.Log("Defeat");
            pauseEvent.Invoke();
            
            clickShieldGraphic.SetActive(true);
            defeatGraphic.SetActive(true);
        }
    }
}