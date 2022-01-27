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

        private void OnEnable()
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


        public override string SetDescription()
        {
            return "Survive for " + turnsToSurvive + " (" + turnController.Turn + "/" + turnsToSurvive + ")";
        }
    }
}

