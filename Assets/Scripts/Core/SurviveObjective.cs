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
            Debug.Log("turn controller " + turnController);
        }

        public override void UpdateObjective()
        {
            Debug.Log("update turn objective "+ turnController.Turn + " update turn objective " + turnsToSurvive);
            if(turnController.Turn >= turnsToSurvive)
            {
                isComplete = true;
            }
        }

        public override string SetDescription()
        {
            return "Survive for " + turnsToSurvive + " turns (" + 
                turnController.Turn + "/" + turnsToSurvive + ")";
        }
    }
}

