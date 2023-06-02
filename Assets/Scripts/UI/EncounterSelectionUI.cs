using FTS.Cards;
using FTS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FTS.UI
{
    public class EncounterSelectionUI : MonoBehaviour
    {
        [SerializeField] GameObject buttonPrefab;
        EncountersController encountersController;
        ObjectiveDatabase objectiveDatabase;

        void Start()
        {
            encountersController = FindObjectOfType<EncountersController>().GetComponent<EncountersController>();
            objectiveDatabase = FindObjectOfType<ObjectiveDatabase>().GetComponent<ObjectiveDatabase>();
            SceneController.OnAdditiveSceneLoaded += SceneController_OnAdditiveSceneLoaded;

            FillPanel();
        }

        private void OnDestroy()
        {
            SceneController.OnAdditiveSceneLoaded -= SceneController_OnAdditiveSceneLoaded;
        }

        void FillPanel()
        {
            List<Encounter> encounters = encountersController.GetEncounters();
            foreach (Encounter encounter in encounters)
            {
                GameObject newButton = Instantiate(buttonPrefab);
                EncounterUI ui = newButton.GetComponentInChildren<EncounterUI>();
                newButton.transform.SetParent(transform, false);
                if (encounter.IsSelected)
                {
                    newButton.transform.GetChild(0).gameObject.SetActive(false);
                    continue;
                }
                ui.NameText.text = encounter.EncounterName;
                ui.GetComponent<TooltipUI>().CreateTooltips(encounter.EncounterName);          
                ui.Image.sprite = encounter.EncounterSprite;

                if (!encounter.IsAvailable)
                {
                    ui.Button.interactable = false;
                    continue;
                }

                if (encounter.NextScene == Scenes.GameScene)
                {
                    List<Objective> objectives = objectives = objectiveDatabase.GenerateObjectives();
                    foreach (Objective objective in objectives)
                    {
                        objective.EnableEncounter();
                    }
                    ui.ObjectiveText.text = objectives[0].SetDescription(true) + "\n" +
                                            objectives[1].SetDescription(true);
                    ui.Button.onClick.AddListener(() =>
                    {
                        newButton.transform.GetChild(0).gameObject.SetActive(false);
                        encountersController.SelectEncounter(encounter.Id);
                        RunController.Instance.SetCombatType(encounter.CombatType);
                        //Debug.Log(RunController.Instance.CombatType);
                        SelectEncounter(encounter, objectives);
                    });
                }
                else
                {
                    ui.Button.onClick.AddListener(() =>
                    {
                        newButton.transform.GetChild(0).gameObject.SetActive(false);
                        encountersController.SelectEncounter(encounter.Id);
                        SelectEncounter(encounter);
                    });
                }
                //LayoutRebuilder.ForceRebuildLayoutImmediate(ui.GetComponent<RectTransform>());
            }
           
        }

        private void ClearPanel()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void SelectEncounter(Encounter encounter, List<Objective> objectives = null)
        {
            if (objectives != null)
            {
                objectiveDatabase.SetObjectives(objectives);
            }
            SceneController.Instance.LoadScene(encounter);
        }

        private void SceneController_OnAdditiveSceneLoaded()
        {
            ClearPanel();
            FillPanel();
        }
    }
}
