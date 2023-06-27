using FTS.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FTS.UI
{
    public class CrossSceneUI : MonoBehaviour
    {
        [SerializeField] GameObject settingsUI;
        
        void Start()
        {
            settingsUI.SetActive(false);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(settingsUI.activeSelf)
                    ToggleSettings();      
            }
        }

        public void ToggleSettings()
        {
            settingsUI.SetActive(!settingsUI.activeSelf);
        }
    }
}
