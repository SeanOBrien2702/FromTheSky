#region Using Statements
using FTS.Cards;
using FTS.Grid;
using FTS.Turns;
using MoreMountains.Feedbacks;
using System;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    public abstract class Unit : MonoBehaviour
    {
        public static event Action<Unit> OnHover = delegate { };
        public static event Action<Unit> OnHoverExit = delegate { };
        [SerializeField] protected UnitUI unitUI;
        [SerializeField] Sprite portrait;
        [SerializeField] GameObject barrier;

        [SerializeField] string description;
        [SerializeField] SFXObject hoverSound;
        protected UnitController unitController;

        public MMFeedbacks damageFeedback;

        string fullName;
        int health = 0;
        protected int maxHealth = 0;
        int armour = 0;
        bool hasBarrier = false;
        bool isFriendly = true;
        protected HexCell location;

        #region Properties
        public string Names   // property
        {
            get { return fullName; }   // get method
            set { fullName = value; }  // set method
        }

        public Sprite Portrait  // property
        {
            get { return portrait; }   // get method
        }

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

        public bool IsFriendly
        {
            get { return isFriendly; }
            set { isFriendly = value; }
        }

        public virtual HexCell Location
        {
            get { return location; }
            set
            {
                location = value;
            }
        }

        public string Description { get => description; }
        #endregion

        #region MonoBehaviour Callbacks
        protected virtual void Awake()
        {
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
        }
        protected virtual void OnMouseEnter()
        {
            SFXManager.Main.Play(hoverSound);
            OnHover?.Invoke(this);
        }

        protected virtual void OnMouseExit()
        {
            OnHoverExit?.Invoke(this);
        }  
        #endregion

        #region Private Methods
        protected virtual void TakeDamage()
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
        public virtual void CalculateDamageTaken(int damage)
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
                unitController.DamageTaken(this, damage);
            }
            else
            {
                UpdateBarrier(false);
            }
        }


        internal void ShowDamage(int damage)
        {
            if (damage == 0)
            {
                unitUI.HideDamage();
            }
            else
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
                unitUI.ShowDamage(health, maxHealth, damage);
            }
        }


        public void Stun()
        {
            Debug.Log(name + " stunned");
            GetComponent<Mover>().CanMove = false;
        }

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
