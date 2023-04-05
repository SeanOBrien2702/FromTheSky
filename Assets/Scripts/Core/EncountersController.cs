using AMPInternal.Utilities;
using FTS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace FTS.Core
{
    public class EncountersController : MonoBehaviour
    {
        [SerializeField] RunController runController;

        int encounterNumber = 3;
        [SerializeField] Encounter combatEncounter;
        [SerializeField] Encounter eliteEncounter;
        [SerializeField] Encounter bossEnccounter;
        [SerializeField] Encounter eventEncounter;
        [SerializeField] Encounter shopEncounter;
        [SerializeField] Encounter lootEncounter;

        int numCombatEncounters = 0;
        int numShopEncounters = 0;
        int numLootEncounters = 0;
        List<Encounter> encounterList = new List<Encounter>();

        void Start()
        {
            encounterList.Add(combatEncounter);
            encounterList.Add(eliteEncounter);
            encounterList.Add(eventEncounter);
            encounterList.Add(shopEncounter);
            encounterList.Add(lootEncounter);
        }

        private Encounter GetRandomEnccounters()
        {
            Encounter newEncounter;
            int randomNumber = UnityEngine.Random.Range(0, 100);

            if (randomNumber <= 65)
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
            else if (randomNumber > 65 && randomNumber <= 75)
            {
                newEncounter = eliteEncounter;
            }
            else if (randomNumber > 75 && randomNumber <= 90)
            {
                numShopEncounters++;
                if(numShopEncounters > 1)
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
                if (numLootEncounters > 1)
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

        internal List<Encounter> GetEncounters()
        {
            numCombatEncounters = 0;
            List<Encounter> encounters = new List<Encounter>();
            if (runController.Day > 1)
            {
                for (int i = 0; i < encounterNumber; i++)
                {
                    encounters.Add(GetRandomEnccounters());
                }
            }
            else if (runController.Day == 1)
            {
                for (int i = 0; i < encounterNumber; i++)
                {
                    encounters.Add(bossEnccounter);
                }
            }
            runController.Day--;
            return encounters;
        }
    }
}
