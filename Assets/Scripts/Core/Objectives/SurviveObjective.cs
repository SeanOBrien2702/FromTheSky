using UnityEngine;
using FTS.Turns;

namespace FTS.Core
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/SurviveObjective", fileName = "SurviveObjective.asset")]
    public class SurviveObjective : Objective
    {
        [SerializeField] int turnsToSurvive;
        TurnController turnController;

        public override void EnableObjective()
        {
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
        }

        public override void UpdateObjective()
        {
            if(turnController.Turn >= turnsToSurvive)
            {
                isComplete = true;
            }
        }

        public override string SetDescription(bool isEncounter = false)
        {
            string description = "Survive for " + turnsToSurvive + " turns";
            if (!isEncounter)
            {
                description += " (" + turnController.Turn + "/" + turnsToSurvive + ")";
            }
            return description;
        }
    }
}

