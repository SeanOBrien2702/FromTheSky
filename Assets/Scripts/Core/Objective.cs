using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Cards;
using FTS.Characters;

namespace FTS.Core
{
    public abstract class Objective : ScriptableObject
    {
        //[SerializeField] string objectiveDescription;
        //[SerializeField] int goldReward;
        //[SerializeField] Card cardReward;
        //[SerializeField] bool isOptional;
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

        //public bool IsOptional
        //{
        //    get { return isOptional; }
        //    set { isOptional = value; }
        //}
        #endregion


        public virtual void EnableObjective()
        {
            Debug.Log("enable objective");
        }


        public virtual void UpdateObjective(Enemy enemy)
        {
            Debug.Log("check objective");
        }

        public virtual void UpdateObjective(Card card)
        {
            Debug.Log("check objective");
        }

        public virtual void UpdateObjective()
        {
            Debug.Log("check objective");
        }

        public virtual string SetDescription()
        {
            return "no description";
        }
    }
}

