using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Cards;
using FTS.Characters;

namespace FTS.Core
{
    public abstract class Objective : ScriptableObject
    {
        [SerializeField] int cinderReward;
        [SerializeField] bool isOptional;
        protected bool isComplete = false;

        #region Properties
        public int CinderReward   // property
        {
            get { return cinderReward; }   // get method
            set { cinderReward = value; }  // set method
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

        public virtual void UpdateObjective(int damage)
        {
            Debug.Log("check objective");
        }

        public virtual void UpdateObjective()
        {
            Debug.Log("check objective");
        }

        public virtual string SetDescription(bool isEncounter = false)
        {
            return "no description";
        }
    }
}

