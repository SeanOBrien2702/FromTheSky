using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP.Core
{
    public class ObjectiveController : MonoBehaviour
    {
        [SerializeField] Objective[] objectivesList;
        List<Objective> currentObjectives = new List<Objective>();
        List<MapObjective> mapObjectives;
        [SerializeField] int numObjectives = 3;

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
            return currentObjectives;
        }
    }
}
