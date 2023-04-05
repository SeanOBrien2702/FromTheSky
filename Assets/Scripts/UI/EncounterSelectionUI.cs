using FTS.Cards;
using FTS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterSelectionUI : MonoBehaviour
{
    [SerializeField] EncounterUI buttonPrefab;
    List<EncounterUI> encounterUI = new List<EncounterUI>();
    EncountersController encountersController;
    ObjectiveDatabase objectiveDatabase;
    //List<Objective> objectives = new List<Objective>();

    void Start()
    {
        encountersController = FindObjectOfType<EncountersController>().GetComponent<EncountersController>();
        objectiveDatabase = FindObjectOfType<ObjectiveDatabase>().GetComponent<ObjectiveDatabase>();
        FillPanel();
    }

    void FillPanel()
    {

        List<Encounter> encounters = encountersController.GetEncounters();

        foreach (Encounter encounter in encounters)
        {
            EncounterUI ui = Instantiate(buttonPrefab);
            ui.transform.SetParent(transform, false);
            ui.NameText.text = encounter.EncounterName;
            ui.Image.sprite = encounter.EncounterSprite;
            if(encounter.NextScene == Scenes.GameScene)
            {
                List<Objective> objectives = objectives = objectiveDatabase.GenerateObjectives();
                ui.ObjectiveText.text = objectives[0].SetDescription(true) + "\n" +
                                        objectives[1].SetDescription(true);
                ui.Button.onClick.AddListener(() => {
                    SelectEncounter(encounter, objectives);
                });
            }
            else
            {
                ui.Button.onClick.AddListener(() => {
                    SelectEncounter(encounter);
                });
            }         
        }
    }

    private void SelectEncounter(Encounter encounter, List<Objective> objectives = null)
    {
        if(objectives != null)
        {
            objectiveDatabase.SetObjectives(objectives);
        }
        SceneController.Instance.LoadScene(encounter);
    }
}
