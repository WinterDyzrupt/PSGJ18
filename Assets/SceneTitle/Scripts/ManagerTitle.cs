using Generic.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Variables;

namespace SceneTitle.Scripts
{
    public class ManagerTitle : MonoBehaviour
    {
        [SerializeField] private BoolVariable startBool;
        [SerializeField] private BoolVariable manualBool;
        [SerializeField] private BoolVariable creditsBool;

        [SerializeField] private GameObject manualPanel;
        [SerializeField] private GameObject creditsPanel;

        void Start()
        {
            Debug.Assert(startBool != null, "StartBool is null! Assign in inspector!");
            Debug.Assert(manualBool != null, "ManualBool is null! Assign in inspector!");
            Debug.Assert(creditsBool != null, "CreditsBool is null! Assign in inspector!");
        
            Debug.Assert(manualPanel != null, "ManualPanel is null! Assign in inspector!");
            Debug.Assert(creditsPanel != null, "CreditsPanel is null! Assign in inspector!");

            startBool.BoolChanged += StartButton;
            manualBool.BoolChanged += ManualButton;
            creditsBool.BoolChanged += CreditsButton;
        }

        private static void StartButton()
        {
            SceneManager.LoadScene(SceneData.PlaymatSceneIndex);
        }

        private void ManualButton()
        {
            if (manualPanel.activeSelf != manualBool)
            {
                manualPanel.SetActive(manualBool);
            }
        }

        private void CreditsButton()
        {
            if (creditsPanel.activeSelf != creditsBool)
            {
                creditsPanel.SetActive(creditsBool);
            }
        }
    }
}