using FTS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] Toggle tutorialToggle;
    void Start()
    {
        tutorialToggle.isOn = !TutorialController.Instance.IsTutorialComplete;
        tutorialToggle.onValueChanged.AddListener(ToggleTutorial);
    }

    private void ToggleTutorial(bool isEnabled)
    {
        tutorialToggle.isOn = isEnabled;
        TutorialController.Instance.SetTutorial(!isEnabled);
    }
}
