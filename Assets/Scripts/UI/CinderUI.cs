using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FTS.Core;

namespace FTS.UI
{
    public class CinderUI : MonoBehaviour
    {
        TextMeshProUGUI cinderText;

        void Start()
        {
            cinderText = GetComponent<TextMeshProUGUI>();
            cinderText.text = FindObjectOfType<RunController>().GetComponent<RunController>().Cinder.ToString();
            RunController.OnCinderChanged += CinderController_OnValueChanged;
        }

        private void OnDestroy()
        {
            RunController.OnCinderChanged -= CinderController_OnValueChanged;
        }

        private void CinderController_OnValueChanged(int value)
        {
            cinderText.text = value.ToString();
        }
    }
}
