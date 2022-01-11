using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Cards;
using SP.Characters;

namespace SP.Core
{
    public abstract class Objective : ScriptableObject
    {
        //[SerializeField] string objectiveDescription;
        [SerializeField] int goldReward;
        [SerializeField] Card cardReward;
        [SerializeField] bool isOptional;
        protected bool isComplete = false;

        #region Properties
        //public string ObjectiveDescription   // property
        //{
        //    get { return objectiveDescription; }   // get method
        //    set { objectiveDescription = value; }  // set method
        //}
        //public int GoldReward   // property
        //{
        //    get { return goldReward; }   // get method
        //    set { goldReward = value; }  // set method
        //}

        //public Card CardReward   // property
        //{
        //    get { return cardReward; }   // get method
        //    set { cardReward = value; }  // set method
        //}

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

        

        public virtual void UpdateObjective(Character enemy)
        {
            Debug.Log("check objective");
        }

        public virtual void UpdateObjective(Card card)
        {
            Debug.Log("check objective");
        }


        public virtual string GetEffectText()
        {
            return "no effect";
        }

        public virtual string SetDescription()
        {
            return "no description";
        }
    }
}

