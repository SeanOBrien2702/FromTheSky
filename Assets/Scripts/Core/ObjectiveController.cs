using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Core
{
    public class ObjectiveController : MonoBehaviour
    {
        [SerializeField] Objective[] objectivesList;
        [SerializeField] Objective[] optionalObjectivesList;
        List<Objective> currentObjectives = new List<Objective>();
        List<MapObjective> mapObjectives;
        [SerializeField] int minNumObjectives = 3;
        //[SerializeField] int 

        //TODO: change this from adding each objecive to procedurally selecting them
        public List<Objective> GetRandomObjectives()
        {
            currentObjectives.Clear();
            foreach (var item in objectivesList)
            {
                currentObjectives.Add(item);
            }
            return currentObjectives;
        }

        internal void SetObjective(List<Objective> objectives)
        {
            currentObjectives.Clear();
            Debug.Log("objectives " + objectives);
            currentObjectives.AddRange(objectives);
        }

        internal List<Objective> GetObjectiveList()
        {
            if (currentObjectives.Count <= 0)
            {
                GetRandomObjectives();
            }
            return currentObjectives;
        }
    }
}
