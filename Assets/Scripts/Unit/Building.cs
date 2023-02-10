#region Using Statements
using FTS.Cards;
using FTS.Grid;
using FTS.Turns;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace FTS.Characters
{
    public class Building : Unit
    {
        //[SerializeField] UnitUI unitUI;
        //[SerializeField] GameObject barrier;
        [SerializeField] int startingHealth = 5;
        //UnitController unitController;
        //HexCell location;
        //public MMFeedbacks damageFeedback;

        //string fullName;
        //bool busy = false;

        //int health = 0;
        //int maxHealth = 0;
        //int initiative = 0;
        // int maxInitiative = 20;
        //int energy = 4;
        //int maxEnergy = 4;
        //int armour = 0;
        //bool hasBarrier = false;


        #region Properties
        //public int Health   // property
        //{
        //    get { return health; }   // get method
        //    set
        //    {
        //        health = value;
        //        TakeDamage();
        //    }  // set method
        //}
        //public int MaxHealth   // property
        //{
        //    get { return maxHealth; }   // get method
        //    set
        //    {
        //        maxHealth = value;
        //        unitUI.UpdateHealth(health, maxHealth);
        //    }  // set method
        //}
        //public int Armour   // property
        //{
        //    get { return armour; }   // get method
        //    set
        //    {
        //        armour = value;
        //        unitUI.UpdateArmour(armour);
        //    }  // set method
        //}

        //public bool HasBarrier   // property
        //{
        //    get { return hasBarrier; }   // get method
        //    set
        //    {
        //        hasBarrier = value;
        //        UpdateBarrier(value);
        //    }  // set method
        //}
        public override HexCell Location
        {
            get { return location; }
            set
            {
                if (location)
                {
                    location.Unit = null;
                }
                location = value;
                value.Unit = this;
                transform.localPosition = value.transform.localPosition;
            }
        }
        #endregion

        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            Debug.Log("start character");
            base.Awake();
            //unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            Health = maxHealth = startingHealth;

            Debug.Log("health " + Health);
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
        }

        private void Start()
        {
            //CreateHeathBar();
            //fullName = characterClass.ToString();
            //Debug.Log("character placed");
            unitUI.UpdateHealth(Health, maxHealth);
            //Debug.Log("hello?");
            //MaxHealth = health;
        }
        private void OnDestroy()
        {
            //TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
        }
        #endregion

        //#region Private Methods
        //private void TakeDamage()
        //{
        //    unitUI.UpdateHealth(health);
        //    if (health <= 0)
        //    {
        //        Die();
        //    }
        //}

        //private void UpdateBarrier(bool enable)
        //{
        //    barrier.SetActive(enable);
        //    hasBarrier = enable;
        //}
        //#endregion

        #region Public Methods
        //public void CalculateDamageTaken(int damage)
        //{
        //    damageFeedback?.PlayFeedbacks(transform.position, damage); //damage text animation
        //    if (!hasBarrier)
        //    {
        //        if (armour > 0)
        //        {
        //            if (damage <= armour)
        //            {
        //                Armour -= damage;
        //                damage = 0;
        //            }
        //            else
        //            {
        //                damage -= armour;
        //                Armour = 0;
        //            }
        //        }
        //        Health -= damage;
        //    }
        //    else
        //    {
        //        UpdateBarrier(false);
        //    }
        //}
  
        //internal void CreateHeathBar()
        //{
        //   // MaxHealth = health;
        //    //unitUI.UpdateHealth(health, maxHealth);
        //}
        #endregion

        //#region Coroutines
        //IEnumerator DeathAnimation()
        //{
        //    animator.SetTrigger("Die");
        //    yield return new WaitForSeconds(1.5f);
        //    Destroy(gameObject);
        //}
        //#endregion


    }
}
