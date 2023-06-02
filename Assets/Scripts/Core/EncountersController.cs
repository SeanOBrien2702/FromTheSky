using FTS.Characters;
using FTS.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Core
{
    public class EncountersController : MonoBehaviour
    {
        int encounterNumber = 3;
        int encounterColumns = 3;
        int numEncounterLayers = 3;
        [SerializeField] Encounter combatEncounter;
        [SerializeField] Encounter eliteEncounter;
        [SerializeField] Encounter bossEnccounter;
        [SerializeField] Encounter eventEncounter;
        [SerializeField] Encounter shopEncounter;
        [SerializeField] Encounter lootEncounter;

        int currentEncounter;
        int numCombatEncounters = 0;
        int numEliteEncounters = 0;
        int numShopEncounters = 0;
        int numLootEncounters = 0;
        List<Encounter> encounters = new List<Encounter>();

        private void Awake()
        {
            if (encounters.Count <= 0)
            {
                GenerateEncounters();
            }
            MainMenuUIController.OnCharacterSelect += MainMenuUIController_OnCharacterSelect;
        }

        private void Update()
        {
            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    SceneController.Instance.LoadScene(Scenes.DraftScene, true);
                }
            }
        }

        private void OnDestroy()
        {
            MainMenuUIController.OnCharacterSelect -= MainMenuUIController_OnCharacterSelect;
        }

        private Encounter GetRandomEnccounters()
        {
            Encounter newEncounter;
            int randomNumber = UnityEngine.Random.Range(0, 100);

            if (randomNumber <= 40)
            {
                numCombatEncounters++;
                if(numCombatEncounters >= encounterNumber)
                {
                    newEncounter = eliteEncounter;
                }
                else
                {
                    newEncounter = combatEncounter;
                }
            }
            else if (randomNumber > 40 && randomNumber <= 70)
            {
                numEliteEncounters++;
                if (numEliteEncounters > 3)
                {
                    newEncounter = combatEncounter;
                }
                else
                {
                    newEncounter = eliteEncounter;
                }
                
            }
            else if (randomNumber > 70 && randomNumber <= 90)
            {
                numShopEncounters++;
                if(numShopEncounters > 2)
                {
                    newEncounter = combatEncounter;
                }
                else
                {
                    newEncounter = shopEncounter;
                }
            }
            else
            {
                numLootEncounters++;
                if (numLootEncounters > 2)
                {
                    newEncounter = combatEncounter;
                }
                else
                {
                    newEncounter = lootEncounter;
                }
               
            }
            return newEncounter;
        }

        private void GenerateEncounters()
        {
            numCombatEncounters = 0;

            for (int i = 0; i < encounterColumns; i++)
            {
                encounters.Add(Instantiate(bossEnccounter));
            }

            for (int i = 0; i < encounterColumns * numEncounterLayers; i++)
            {
                encounters.Add(Instantiate(GetRandomEnccounters()));
            }

            for (int i = 0; i < encounterColumns; i++)
            {
                encounters.Add(Instantiate(combatEncounter));
            }
            int id = 0;
            foreach (var encounter in encounters)
            {
                encounter.Id = id++;
                encounter.IsSelected = false;
                encounter.IsAvailable = false;
            }
            for (int i = encounters.Count - encounterColumns; i < encounters.Count; i++)
            {
                encounters[i].IsAvailable = true;
            }
        }


        internal List<Encounter> GetEncounters()
        {      
            return encounters;
        }

        internal void SelectEncounter(int id)
        {
            currentEncounter = id;
            encounters[id].IsSelected = true;
            if (id >= encounterColumns)
            {
                encounters[id - encounterColumns].IsAvailable = true;
            }
        }

        private void MainMenuUIController_OnCharacterSelect()
        {
            if (encounters.Count <= 0)
            {
                GenerateEncounters();
            }
        }   
    }
}
