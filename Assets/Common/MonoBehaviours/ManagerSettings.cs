using UnityEngine;
using Common.Data;

namespace Common.MonoBehaviours
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private BoolVariable settingBool; 
        
        [SerializeField] private GameObject settingsPrefab;

        private void Start()
        {
            Debug.Assert(settingBool != null, "SettingBool is null! Assign in inspector!");
            
            Debug.Assert(settingsPrefab != null, "SettingsPrefab is null! Assign in inspector!.");

            settingBool.BoolChanged += ToggleSettingsPanel;
        }
        
        private void ToggleSettingsPanel()
        {
            settingsPrefab.SetActive(!settingsPrefab.activeSelf);
        }
        
        // TODO: Create Control options here. Probably Sound, Music, Controls, etc
    }
}
