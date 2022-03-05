using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using FTS.Characters;
using FTS.Cards;
using FTS.UI;
using System.Linq;
using FTS.Turns;

namespace FTS.Core
{
    public class ObjectiveController : MonoBehaviour
    {
        //[SerializeField] TextMeshProUGUI objectiveLabel;
        //[SerializeField] TextMeshProUGUI optionalLabel;
        //[SerializeField] Transform objectivePanel;
        
        [SerializeField] List<Objective> objectivesBuffer;
        ObjectiveUI objectiveUI;
        List<Objective> objectives = new List<Objective>();
        //Dictionary<Objective, TextMeshProUGUI> textList = new Dictionary<Objective, TextMeshProUGUI>();
        int objectiveNum;
        bool isLevelComplete = true;
        // Start is called before the first frame update
        void Start()
        {
            objectiveUI = FindObjectOfType<ObjectiveUI>().GetComponent<ObjectiveUI>();
            objectives.AddRange(FindObjectOfType<ObjectiveDatabase>().GetComponent<ObjectiveDatabase>().GetObjectiveList()); // objectivesBuffer.OrderBy(item => item.IsOptional).ToList();
            objectiveNum = objectives.Count;
            Debug.Log("objectives " + objectives.Count);
            objectiveUI.CreateObjectiveText(objectives);
            TurnController.OnNewTurn += TurnController_OnNewTurn;
            CardController.OnCardPlayed += CardController_OnCardPlayed;
            UnitController.OnEnemyKilled += UnitController_OnEnemyKilled;
            foreach (var objective in objectives)
            {
                objective.IsComplete = false;
                objective.EnableObjective();
                Debug.Log("objectives " + objective.name);
            }
        }



        private void OnDestroy()
        {
            TurnController.OnNewTurn -= TurnController_OnNewTurn;
            CardController.OnCardPlayed += CardController_OnCardPlayed;
            UnitController.OnEnemyKilled -= UnitController_OnEnemyKilled;
        }


        //private void CreateObjectiveText()
        //{
        //    bool isFirstOptional = true;
        //    //List<Objective> objectives = objectivesBuffer.OrderByDescending(item => item.IsOptional).ToList();
        //    //objectives = objectivesBuffer.OrderBy(item => item.IsOptional).ToList();
        //    for (int i = 0; i < objectives.Count; i++)
        //    {
        //        if (isFirstOptional && objectives[i].IsOptional)
        //        {
        //            isFirstOptional = false;
        //            TextMeshProUGUI optional = Instantiate(optionalLabel);
        //            optional.transform.SetParent(objectivePanel, false);
        //        }
        //        TextMeshProUGUI textMesh = Instantiate(objectiveLabel);
        //        textMesh.text = objectives[i].SetDescription();
        //        textMesh.transform.SetParent(objectivePanel, false);
        //        textMesh.color = Color.white;

        //        textList.Add(objectives[i], textMesh);
        //    }
        //}

        // Update is called once per frame
        void Update()
        {

        }

        //public void CheckObjectives(Card card)
        //{
        //    foreach (var objective in objectives)
        //    {
        //        //if (objective is CardObjective)
        //        objective.UpdateObjective(card);
        //    }
        //    ObjectiveUpdated();
        //}

        internal void UpdateObjective()
        {
            Debug.Log("update objectives");
            foreach (var objective in objectives)
            {
                //if (objective is KillObjective)
                //{
                //    objective.UpdateObjective((Enemy)enemy);
                //}
                //else
                //{
                    objective.UpdateObjective();
                //}
            }
            CheckObjectives();
        }

        void CheckObjectives()
        {
            //TODO: fix object UI being null sometimes
            if (objectiveUI != null)
            {
                objectiveUI.UpdateUI(objectives);
                int objectivesComplete = 0;
                int objectivesRequired = objectives.Count(item => item.IsOptional == false);
                foreach (var objective in objectives)
                {
                    Debug.Log("objective complete: " + objective.name);
                    Debug.Log("objective complete: " + objective.IsComplete);
                    if (objective.IsComplete && !objective.IsOptional)
                    {
                        objectivesComplete++;
                    }
                }
                Debug.Log("objective complete: " + objectivesRequired);
                if (objectivesComplete >= objectivesRequired)
                {
                    SceneManager.LoadScene("DraftScene");
                }
            }
        }


        //void UpdateUI()
        //{
        //    foreach (var objective in objectives)
        //    {
        //        textList[objective].text = objective.SetDescription();
        //    }
        //}
        private void UnitController_OnEnemyKilled(Character enemy)
        {
            Debug.Log("update objectives");
            foreach (var objective in objectives.FindAll(item => item is KillObjective))
            {
                objective.UpdateObjective((Enemy)enemy);
            }
            CheckObjectives();
        }

        private void CardController_OnCardPlayed(Card playedCard)
        {
            Debug.Log("update objectives");
            foreach (var objective in objectives.FindAll(item => item is CardObjective))
            {
                objective.UpdateObjective(playedCard);
            }
            CheckObjectives();
        }

        private void TurnController_OnNewTurn()
        {
            foreach (var objective in objectives.FindAll(item => item is SurviveObjective))
            {
                objective.UpdateObjective();
            }
            CheckObjectives();
        }
    }
}
