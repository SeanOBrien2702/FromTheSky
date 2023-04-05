using UnityEngine;
using FTS.Grid;
using FTS.Cards;
using FTS.Characters;
using FTS.Turns;

namespace FTS.Core
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/UnitObjective", fileName = "UnitObjective.asset")]
    public class UnitObjective : Objective
    {
        [SerializeField] int unitThreshhold;
        UnitController unitController;

        public override void EnableObjective()
        {
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
        }

        public override void UpdateObjective()
        {
            if (unitController.NumberOfPlayers <= unitThreshhold)
            {
                isComplete = true;
            }
            else
            {
                isComplete = false;
            }
        }

        public override string SetDescription(bool isEncounter = false)
        {
            string description = "Control " + unitThreshhold + " units";
            if (!isEncounter)
            {
                description += " (" + unitController.NumberOfPlayers + "/" + unitThreshhold + ")";
            }
            return description;
        }
    }
}

