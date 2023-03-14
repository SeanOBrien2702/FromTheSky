using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FTS.Core;

namespace FTS.UI
{
    public class HeartUI : MonoBehaviour
    {
        TextMeshProUGUI hearthText;

        void Start()
        {
            hearthText = GetComponent<TextMeshProUGUI>();
            hearthText.text = FindObjectOfType<RunController>().GetComponent<RunController>().Health.ToString();
            RunController.OnHealthChanged += HeartController_OnValueChanged;
        }

        private void OnDestroy()
        {
            RunController.OnHealthChanged -= HeartController_OnValueChanged;
        }

        private void HeartController_OnValueChanged(int value)
        {
            hearthText.text = value.ToString();
        }
    }
}
