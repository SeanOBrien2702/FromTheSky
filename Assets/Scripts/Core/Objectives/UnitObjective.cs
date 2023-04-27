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
        [SerializeField] int[] unitThreshholds;

        UnitController unitController;
        int unitsToControl = 1;

        public override void EnableObjective()
        {
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            unitsToControl = unitThreshholds[RunController.Instance.GetDifficultyScale()];
        }

        public override void UpdateObjective()
        {
            if (unitController.NumberOfPlayers <= unitsToControl)
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
            string description = "Control " + unitsToControl + " units";
            if (!isEncounter)
            {
                description += " (" + unitController.NumberOfPlayers + "/" + unitsToControl + ")";
            }
            return description;
        }
    }
}

