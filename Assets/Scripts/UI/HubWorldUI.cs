#region Using Statements
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FTS.Core;
using TMPro;
using System.Collections.Generic;
using System;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
#endregion

namespace FTS.UI
{
    public class HubWorldUI : MonoBehaviour
    {
        [SerializeField] GameObject endGameScreen;


        //[SerializeField] ObjectiveGenerator objective;
        //[SerializeField] PlanetCamera planetCamera;

        //[Header("Objectives")]
        //[SerializeField] TextMeshProUGUI objectiveLabel;
        //[SerializeField] TextMeshProUGUI optionalLabel;
        //[SerializeField] Transform objectivePanel;
        //List<Objective> currentObjective = new List<Objective>();
        //Dictionary<Objective, TextMeshProUGUI> textList = new Dictionary<Objective, TextMeshProUGUI>();


        #region Public Methods
        public void WonGame()
        {
            SceneController.Instance.LoadScene(Scenes.MainMenu);
        }

        public void StartGame()
        {
            SceneController.Instance.LoadScene(Scenes.GameScene);
        }
        #endregion

        void Start()
        {
            //UpdateUI(objective.CurrentObjective);
            //planetCamera.PanToObjective(objective.CurrentObjective);
            if (FindObjectOfType<RunController>().GetComponent<RunController>().Day < 0)
            {
                endGameScreen.SetActive(true);
            }
        }

        //void UpdateUI(MapObjective currentObjective)
        //{
        //    ClearUI();
        //    bool isFirstOptional = true;
        //    TextMeshProUGUI objectiveLbl = Instantiate(objectiveLabel);
        //    objectiveLbl.transform.SetParent(objectivePanel, false);
        //    for (int i = 0; i < currentObjective.Objectives.Count; i++)
        //    {             
        //        if (isFirstOptional && currentObjective.Objectives[i].IsOptional)
        //        {
        //            isFirstOptional = false;
        //            TextMeshProUGUI optional = Instantiate(optionalLabel);
        //            optional.transform.SetParent(objectivePanel, false);
        //        }
        //        TextMeshProUGUI textMesh = Instantiate(objectiveLabel);
        //        textMesh.text = currentObjective.Objectives[i].SetDescription();
        //        textMesh.transform.SetParent(objectivePanel, false);
        //        textMesh.color = Color.white;

        //        textList.Add(currentObjective.Objectives[i], textMesh);
        //    }

        //    foreach (var objective in currentObjective.Objectives)
        //    {
        //        textList[objective].text = objective.SetDescription();
        //    }
        //}

        //void ClearUI()
        //{
        //    textList.Clear();
        //    foreach (Transform child in objectivePanel)
        //    {
        //        Destroy(child.gameObject);
        //    }
        //}
    }
}
