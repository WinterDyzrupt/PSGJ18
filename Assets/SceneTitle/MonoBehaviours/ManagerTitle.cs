using UnityEngine;
using UnityEngine.SceneManagement;
using Common.Data;
using Common.Utils;

namespace SceneTitle.MonoBehaviours
{
    public class ManagerTitle : MonoBehaviour
    {
        [SerializeField] private BoolWrapper isStartEnabled;
        [SerializeField] private BoolWrapper isManualEnabled;
        [SerializeField] private BoolWrapper isCreditsEnabled;

        [SerializeField] private GameObject manualPanel;
        [SerializeField] private GameObject creditsPanel;

        private void Start()
        {
            Debug.Assert(isStartEnabled != null, "StartBool is null! Assign in inspector!");
            Debug.Assert(isManualEnabled != null, "ManualBool is null! Assign in inspector!");
            Debug.Assert(isCreditsEnabled != null, "CreditsBool is null! Assign in inspector!");
        
            Debug.Assert(manualPanel != null, "ManualPanel is null! Assign in inspector!");
            Debug.Assert(creditsPanel != null, "CreditsPanel is null! Assign in inspector!");

            isStartEnabled.Changed += StartButton;
            isManualEnabled.Changed += ManualButton;
            isCreditsEnabled.Changed += CreditsButton;
        }

        private static void StartButton()
        {
            SceneManager.LoadScene(SceneData.PlaymatSceneIndex);
        }

        private void ManualButton()
        {
            if (manualPanel.activeSelf != isManualEnabled)
            {
                manualPanel.SetActive(isManualEnabled);
            }
        }

        private void CreditsButton()
        {
            if (creditsPanel.activeSelf != isCreditsEnabled)
            {
                creditsPanel.SetActive(isCreditsEnabled);
            }
        }
    }
}