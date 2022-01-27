#region Using Statements
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FTS.Core;
using TMPro;
using System.Collections.Generic;
using System;
#endregion

namespace FTS.UI
{
    public class HubWorldUI : MonoBehaviour
    {
        [SerializeField] ObjectiveGenerator objective;
        [SerializeField] PlanetCamera planetCamera;

        [Header("Objectives")]
        [SerializeField] TextMeshProUGUI objectiveLabel;
        [SerializeField] TextMeshProUGUI optionalLabel;
        [SerializeField] Transform objectivePanel;
        //List<Objective> currentObjective = new List<Objective>();
        Dictionary<Objective, TextMeshProUGUI> textList = new Dictionary<Objective, TextMeshProUGUI>();


        #region Public Methods
        public void StartMisstion()
        {
            //SceneManager.LoadScene("GameScene");
        }

        public void NextMission()
        {
            //Debug.Log(objective.NextObjective.transform.position);
            planetCamera.PanToObjective(objective.NextObjective);
            UpdateUI(objective.CurrentObjective);
        }

        public void PreviousMission()
        {
            planetCamera.PanToObjective(objective.PreviousObjective);
            UpdateUI(objective.CurrentObjective);
        }
        #endregion

        void Start()
        {
            //currentObjective.AddRange(FindObjectOfType<ObjectiveController>().GetComponent<ObjectiveController>().GetObjectiveList()); // objectivesBuffer.OrderBy(item => item.IsOptional).ToList();
            UpdateUI(objective.CurrentObjective);
        }

        void UpdateUI(MapObjective currentObjective)
        {
            ClearUI();
            bool isFirstOptional = true;
            //List<Objective> currentObjective = objectivesBuffer.OrderByDescending(item => item.IsOptional).ToList();
            //currentObjective = objectivesBuffer.OrderBy(item => item.IsOptional).ToList();
            TextMeshProUGUI objectiveLbl = Instantiate(objectiveLabel);
            objectiveLbl.transform.SetParent(objectivePanel, false);
            for (int i = 0; i < currentObjective.Objectives.Count; i++)
            {             
                if (isFirstOptional && currentObjective.Objectives[i].IsOptional)
                {
                    isFirstOptional = false;
                    TextMeshProUGUI optional = Instantiate(optionalLabel);
                    optional.transform.SetParent(objectivePanel, false);
                }
                TextMeshProUGUI textMesh = Instantiate(objectiveLabel);
                textMesh.text = currentObjective.Objectives[i].SetDescription();
                textMesh.transform.SetParent(objectivePanel, false);
                textMesh.color = Color.white;

                textList.Add(currentObjective.Objectives[i], textMesh);
            }
            Debug.Log("currentObjective " + currentObjective.Objectives.Count);

            foreach (var objective in currentObjective.Objectives)
            {
                textList[objective].text = objective.SetDescription();
            }
        }

        void ClearUI()
        {
            textList.Clear();
            foreach (Transform child in objectivePanel)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
