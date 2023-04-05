using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Core
{
    public class ObjectiveDatabase : MonoBehaviour
    {
        [SerializeField] Objective[] objectivesList;
        [SerializeField] Objective[] optionalObjectivesList;
        List<Objective> currentObjectives = new List<Objective>();
        [SerializeField] int minNumObjectives = 3;

        Objective GetRandomObjective(bool isOptional = false)
        {
            Objective objective;

            int randomNum = Random.Range(0, isOptional ? optionalObjectivesList.Count() : objectivesList.Count());

            if (isOptional)
            {
                objective = optionalObjectivesList[randomNum];
            }
            else
            {
                objective = objectivesList[randomNum];
            }

            return objective;
        }

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

        internal void SetObjectives(List<Objective> objectives)
        {
            currentObjectives.Clear();
            currentObjectives.AddRange(objectives);
        }

        internal List<Objective> GenerateObjectives()
        {
            List<Objective> objectives = new List<Objective>();

            objectives.Add(GetRandomObjective(false));
            objectives.Add(GetRandomObjective(true));

            return objectives;
        }
    }
}
