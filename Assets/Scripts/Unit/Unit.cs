#region Using Statements
using FTS.Cards;
using FTS.Grid;
using FTS.Turns;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

#endregion

namespace FTS.Characters
{
    public abstract class Unit : MonoBehaviour//, IComparable
    {
        //[SerializeField] Animator animator;
        //[SerializeField] CharacterClass characterClass;
        //[SerializeField] CharacterStats stats;
        [SerializeField] protected UnitUI unitUI;
        [SerializeField] Sprite portrait;
        [SerializeField] GameObject barrier;
        protected UnitController unitController;

        public MMFeedbacks damageFeedback;

        string fullName;
        //bool busy = false;

        int health = 0;
        protected int maxHealth = 0;
        //int initiative = 0;
        //int maxInitiative = 20;
        //int energy = 4;
        //int maxEnergy = 4;
        int armour = 0;
        bool hasBarrier = false;
        protected HexCell location;

        #region Properties
        public string Names   // property
        {
            get { return fullName; }   // get method
            set { fullName = value; }  // set method
        }
        //public int Initiative   // property
        //{
        //    get { return initiative; }   // get method
        //    set { initiative = value; }  // set method
        //}
        //public bool Busy   // property
        //{
        //    get { return busy; }   // get method
        //    set { busy = value; }  // set method
        //}

        //public int Energy   // property
        //{
        //    get { return energy; }   // get method
        //    set { energy = value; }  // set method
        //}

        //public int MaxEnergy   // property
        //{
        //    get { return maxEnergy; }   // get method
        //    set { maxEnergy = value; }  // set method
        //}

        //public CharacterStats Stats  // property
        //{
        //    get { return stats; }   // get method
        //}

        public Sprite Portrait  // property
        {
            get { return portrait; }   // get method
        }

        //public CharacterClass CharacterClass  // property
        //{
        //    get { return characterClass; }   // get method
        //}

        public int Health   // property
        {
            get { return health; }   // get method
            set
            {
                health = value;
                TakeDamage();
            }  // set method
        }
        public int MaxHealth   // property
        {
            get { return maxHealth; }   // get method
            set
            {
                maxHealth = value;
                unitUI.UpdateHealth(health, maxHealth);
            }  // set method
        }
        public int Armour   // property
        {
            get { return armour; }   // get method
            set
            {
                armour = value;
                unitUI.UpdateArmour(armour);
            }  // set method
        }

        public bool HasBarrier   // property
        {
            get { return hasBarrier; }   // get method
            set
            {
                hasBarrier = value;
                UpdateBarrier(value);
            }  // set method
        }
        public virtual HexCell Location
        {
            get { return location; }
            set
            {
                //if (location)
                //{
                //    location.Unit = null;
                //}
                location = value;
                //value.Unit = this;
                //transform.localPosition = value.transform.localPosition;
            }
        }

        #endregion

        #region MonoBehaviour Callbacks
        protected virtual void Awake()
        {
            Debug.Log("start unit");
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            
            //health = maxHealth = stats.GetStat(Stat.Health, characterClass);
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
        }

        //private void Start()
        //{
        //    fullName = characterClass.ToString();
        //    //Debug.Log("character placed");
        //    //unitUI.UpdateHealth(health, maxHealth);
        //    //Debug.Log("hello?");
        //    //MaxHealth = health;
        //}

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
        }
        #endregion

        #region Private Methods
        private void TakeDamage()
        {
            unitUI.UpdateHealth(health);
            if (health <= 0)
            {
                Die();
            }

        }

        private void UpdateBarrier(bool enable)
        {
            barrier.SetActive(enable);
            hasBarrier = enable;
        }
        #endregion

        #region Public Methods
        public void CalculateDamageTaken(int damage)
        {
            Debug.Log("incoming dmaage " + damage +" "+ health + " " + Health);
            damageFeedback?.PlayFeedbacks(transform.position, damage); //damage text animation
            if (!hasBarrier)
            {
                if (armour > 0)
                {
                    if (damage <= armour)
                    {
                        Armour -= damage;
                        damage = 0;
                    }
                    else
                    {
                        damage -= armour;
                        Armour = 0;
                    }
                }
                Health -= damage;
            }
            else
            {
                UpdateBarrier(false);
            }
        }
        //public int GetStat(Stat stat)
        //{
        //    return Stats.GetStat(stat, this.characterClass);
        //}

        public void Stun()
        {
            Debug.Log(name + " stunned");
            GetComponent<Mover>().CanMove = false;
        }

        //internal void RollInitive()
        //{
        //    initiative = UnityEngine.Random.Range(0, maxInitiative) + stats.GetStat(Stat.Movement, characterClass);
        //}

        public virtual void Die()
        {
            Debug.Log("Die unit");
            unitController.RemoveUnit(this);
            
            //StartCoroutine(DeathAnimation());
            Destroy(gameObject);
        }

        internal virtual void StartRound()
        {
            //Debug.Log("Character turn");
        }

        //public int CompareTo(object obj)
        //{
        //    return Initiative.CompareTo(obj);
        //}

        public virtual Sprite GetBorder()
        {
            return null;
        }

        public virtual void AddToDeck(Card card)
        {
            Debug.Log("card not added to deck");
        }

        internal void CreateHeathBar()
        {
            MaxHealth = health;
            unitUI.UpdateHealth(health, maxHealth);
        }
        #endregion

        //#region Coroutines
        //IEnumerator DeathAnimation()
        //{
        //    animator.SetTrigger("Die");
        //    yield return new WaitForSeconds(1.5f);
        //    Destroy(gameObject);
        //}
        //#endregion

        #region Events
        protected virtual void TurnController_OnNewTurn()
        {
            //energy = maxEnergy;
            //armour = 0;
        }
        #endregion
    }
}
