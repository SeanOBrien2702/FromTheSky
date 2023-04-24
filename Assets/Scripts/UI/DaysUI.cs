using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FTS.Core;

namespace FTS.UI
{
    public class DaysUI : MonoBehaviour
    {
        TextMeshProUGUI dayText;

        void Start()
        {
            dayText = GetComponent<TextMeshProUGUI>();
            dayText.text = "Days left: " + RunController.Instance.Day.ToString();
            RunController.OnDayChanged += RunController_OnDayChanged;
        }

        private void OnDestroy()
        {
            RunController.OnDayChanged -= RunController_OnDayChanged;
        }

        private void RunController_OnDayChanged(int days)
        {
            if (days > 0)
            {
                dayText.text = "Days left: " + days.ToString();
            }
            else
            {
                dayText.text = "Cycle Complete";
            }    
        }
    }
}
