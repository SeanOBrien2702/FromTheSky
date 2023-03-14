using AMPInternal.Utilities;
using FTS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        List<Encounter> encounterList = new List<Encounter>();

        void Start()
        {
            encounterList.Add(combatEncounter);
            encounterList.Add(eliteEncounter);
            encounterList.Add(eventEncounter);
            encounterList.Add(shopEncounter);
            encounterList.Add(lootEncounter);
        }

        internal List<Encounter> GetEncounters()
        {
            List<Encounter> encounters = new List<Encounter>();
            if (runController.Day > 1)
            {
                encounterList.Randomize();
                encounters = encounterList.Take(encounterNumber).ToList();
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
