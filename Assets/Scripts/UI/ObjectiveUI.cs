using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FTS.Core;

namespace FTS.UI
{
    public class ObjectiveUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI objectiveLabel;
        [SerializeField] TextMeshProUGUI optionalLabel;
        [SerializeField] List<Objective> objectivesBuffer;
        Dictionary<Objective, TextMeshProUGUI> textList = new Dictionary<Objective, TextMeshProUGUI>();

        public void CreateObjectiveText(List<Objective> objectives)
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                TextMeshProUGUI textMesh = Instantiate(objectiveLabel);
                textMesh.text = objectives[i].SetDescription();
                textMesh.transform.SetParent(transform, false);
                textMesh.color = Color.white;

                textList.Add(objectives[i], textMesh);
            }

            foreach (var item in objectives)
            {
                UpdateUI(item);
            }
        }
        
        public void UpdateUI(List<Objective> objectives)
        {
            foreach (var objective in objectives)
            {
                textList[objective].text = objective.SetDescription();
            }
        }

        public void UpdateUI(Objective objective)
        {
            textList[objective].text = objective.SetDescription();
            if(objective.IsOptional)
            {
                textList[objective].text += "(Optional)";
            }
        }
    }
}
