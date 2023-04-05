using UnityEngine;
using FTS.Grid;
using FTS.Cards;
using System.Collections.Generic;

namespace FTS.Core
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/PositionObjective", fileName = "PositionObjective.asset")]
    public class PositionObjective : Objective
    {
        //[SerializeField] string objectiveDescription;
        //[SerializeField] int goldReward;
        [SerializeField] Card cardReward;
        [SerializeField] bool isOptional;
        private bool isComplete = false;
        List<HexCell> locations = new List<HexCell>();

        #region Properties
        //public int GoldReward   // property
        //{
        //    get { return goldReward; }   // get method
        //    set { goldReward = value; }  // set method
        //}

        public Card CardReward   // property
        {
            get { return cardReward; }   // get method
            set { cardReward = value; }  // set method
        }

        public bool IsComplete
        {
            get { return isComplete; }
            set { isComplete = value; }
        }

        public bool IsOptional
        {
            get { return isOptional; }
            set { isOptional = value; }
        }
        #endregion
        public void UpdateObjective()
        {
            throw new System.NotImplementedException();
        }

        public override string SetDescription(bool isEncounter = false)
        {
            return "Reach the EVAC zone";
        }

    }
}
