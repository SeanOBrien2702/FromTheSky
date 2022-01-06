using UnityEngine;
using SP.Grid;
using SP.Cards;
using System.Collections.Generic;

namespace SP.Core
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/PositionObjective", fileName = "PositionObjective.asset")]
    public class PositionObjective : Objective
    {
        //[SerializeField] string objectiveDescription;
        [SerializeField] int goldReward;
        [SerializeField] Card cardReward;
        [SerializeField] bool isOptional;
        bool isComplete = false;
        List<HexCell> locations = new List<HexCell>();

        #region Properties
        //public string ObjectiveDescription   // property
        //{
        //    get { return objectiveDescription; }   // get method
        //    set { objectiveDescription = value; }  // set method
        //}
        public int GoldReward   // property
        {
            get { return goldReward; }   // get method
            set { goldReward = value; }  // set method
        }

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

        public override string SetDescription()
        {
            return "Reach the EVAC zone";
        }

    }
}
