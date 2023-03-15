using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FTS.Core;
using UnityEngine.UI;
using System;

namespace FTS.UI
{
    [RequireComponent(typeof(Button))]
    public class SettingsButton : MonoBehaviour
    {
        CrossSceneUI crossSceneUI;
        Button button;

        void Start()
        {
            crossSceneUI = FindObjectOfType<CrossSceneUI>().GetComponent<CrossSceneUI>();
            button = GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            crossSceneUI.ToggleSettings();
        }
    }
}
