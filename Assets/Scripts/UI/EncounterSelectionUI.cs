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

    void Start()
    {
        encountersController = FindObjectOfType<EncountersController>().GetComponent<EncountersController>();
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
            
            ui.Button.onClick.AddListener(() => {
                SelectEncounter(encounter);
            });
        }
    }

    private void SelectEncounter(Encounter encounter)
    {
        SceneController.Instance.LoadScene(encounter);
    }
}
