using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using SP.Characters;
using SP.Cards;
using System.Linq;

namespace SP.Core
{
    public class ObjectiveUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI objectiveLabel;
        [SerializeField] TextMeshProUGUI optionalLabel;
        [SerializeField] Transform objectivePanel;
        [SerializeField] List<Objective> objectivesBuffer;
        List<Objective> objectives = new List<Objective>();
        Dictionary<Objective, TextMeshProUGUI> textList = new Dictionary<Objective, TextMeshProUGUI>();
        int objectiveNum;
        bool isLevelComplete = true;
        // Start is called before the first frame update
        void Start()
        {
            objectives.AddRange(FindObjectOfType<ObjectiveController>().GetComponent<ObjectiveController>().GetObjectiveList()); // objectivesBuffer.OrderBy(item => item.IsOptional).ToList();
            objectiveNum = objectives.Count;
            Debug.Log("objectives " + objectives.Count);
            CreateObjectiveText();
        }

        private void CreateObjectiveText()
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
                    optional.transform.SetParent(objectivePanel, false);
                }
                TextMeshProUGUI textMesh = Instantiate(objectiveLabel);
                textMesh.text = objectives[i].SetDescription();
                textMesh.transform.SetParent(objectivePanel, false);
                textMesh.color = Color.white;
                
                textList.Add(objectives[i], textMesh);
            }
            Debug.Log("objectives " + objectives.Count);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CheckObjectives(Card card)
        {
            foreach (var objective in objectives)
            {
                //if (objective is CardObjective)
                objective.UpdateObjective(card);
            }
            ObjectiveUpdated();
        }

        internal void UpdateObjective(Character enemy)
        {
            foreach (var objective in objectives)
            {
                if (objective is KillObjective)
                    objective.UpdateObjective(enemy);

            }
            ObjectiveUpdated();
        }

        void ObjectiveUpdated()
        {
            UpdateUI();
            int objectivesComplete = 0;
            int objectivesRequired = objectives.Count(item => item.IsOptional == false);
            Debug.Log("objectivesRequired " + objectivesRequired);
            Debug.Log("objectives " + objectives.Count);
            foreach (var objective in objectives)
            {
                if(objective.IsComplete && !objective.IsOptional)
                {
                    objectivesComplete++;
                    break;
                }
            }

            if(objectivesComplete >= objectivesRequired)
            {
                SceneManager.LoadScene("DraftScene");
            }
        }

        
        void UpdateUI()
        {
            foreach (var objective in objectives)
            {
                textList[objective].text = objective.SetDescription();
            }
        }
    }
}
