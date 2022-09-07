using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FTS.Core;
using System;
using UnityEngine.SceneManagement;

namespace FTS.Core
{
    enum EncounterTypes
    {
        Enemy,
        Elite,
        Boss,
        Shop,
        Event,
        Treasure
    }
    public class EncounterSelectionUI : MonoBehaviour
    {
        [SerializeField] Button encounterButton;
        //List<EncounterTypes> encounters = new List<EncounterTypes>();
        //creating a dictionary using collection-initializer syntax
        Dictionary<EncounterTypes, string> encounters = new Dictionary<EncounterTypes, string>(){
            {EncounterTypes.Enemy, "GameScene"},
            {EncounterTypes.Elite, "GameScene"},
            {EncounterTypes.Boss, "GameScene"},
            {EncounterTypes.Shop, "ShopScene"},
            {EncounterTypes.Event, "DraftScene"}
            };
        int numEncounters = 3;

        void Start()
        {
            for (int i = 0; i < numEncounters; i++)
            {
                Array values = Enum.GetValues(typeof(EncounterTypes));
                EncounterTypes randomEncounter = (EncounterTypes)values.GetValue(UnityEngine.Random.Range(0, values.Length));

                Button button = Instantiate(encounterButton, transform);
                button.GetComponentInChildren<TextMeshProUGUI>().text = randomEncounter.ToString();
                button.onClick.AddListener(() => OnSceneClick(encounters[randomEncounter]));
            }
        }

        private void OnSceneClick(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }


}


