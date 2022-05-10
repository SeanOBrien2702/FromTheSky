using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using FTS.Characters;
using FTS.Cards;
using FTS.Core;
using System.Linq;

namespace FTS.UI
{
    public class ObjectiveUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI objectiveLabel;
        [SerializeField] TextMeshProUGUI optionalLabel;
        [SerializeField] List<Objective> objectivesBuffer;
        //List<Objective> objectives = new List<Objective>();
        Dictionary<Objective, TextMeshProUGUI> textList = new Dictionary<Objective, TextMeshProUGUI>();

        // Start is called before the first frame update
        //void Start()
        //{
        //    objectives.AddRange(FindObjectOfType<ObjectiveDatabase>().GetComponent<ObjectiveDatabase>().GetObjectiveList()); // objectivesBuffer.OrderBy(item => item.IsOptional).ToList();
        //    //objectiveNum = objectives.Count;
        //    Debug.Log("objectives " + objectives.Count);
        //    CreateObjectiveText();
        //}

        public void CreateObjectiveText(List<Objective> objectives)
        {
            bool isFirstOptional = true;
            //List<Objective> objectives = objectivesBuffer.OrderByDescending(item => item.IsOptional).ToList();
            //objectives = objectivesBuffer.OrderBy(item => item.IsOptional).ToList();
            for (int i = 0; i < objectives.Count; i++)
            {
                if (isFirstOptional && objectives[i].IsOptional)
                {
                    isFirstOptional = false;
                    TextMeshProUGUI optional = Instantiate(optionalLabel);
                    optional.transform.SetParent(transform, false);
                }
                TextMeshProUGUI textMesh = Instantiate(objectiveLabel);
                textMesh.text = objectives[i].SetDescription();
                textMesh.transform.SetParent(transform, false);
                textMesh.color = Color.white;
                
                textList.Add(objectives[i], textMesh);
            }
        }
        
        public void UpdateUI(List<Objective> objectives)
        {
            foreach (var objective in objectives)
            {
                textList[objective].text = objective.SetDescription();
            }
        }
    }
}
