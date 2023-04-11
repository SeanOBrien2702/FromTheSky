using FTS.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class ObjectiveUI : MonoBehaviour
    {
        [SerializeField] Toggle toggle;
        [SerializeField] TextMeshProUGUI objectiveText;

        public void UpdateObjective(Objective objective)
        {
            objectiveText.text = objective.SetDescription();
            if (objective.IsOptional)
            {
                objectiveText.text += "(Optional)";
            }
            toggle.isOn = objective.IsComplete;
        }
    }
}
