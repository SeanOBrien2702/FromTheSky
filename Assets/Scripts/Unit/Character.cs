#region Using Statements
using FTS.Cards;
using FTS.Turns;
using System.Collections;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    public class Character : Unit
    {
        [SerializeField] protected Animator animator;
        [SerializeField] AnimatorOverrideController animatorOverride;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] CharacterStats stats;
        Mover mover;

        bool busy = false;
        int initiative = 0;
        int maxInitiative = 20;
        bool isHealthSet = false;

        #region Properties
        public int Initiative   // property
        {
            get { return initiative; }   // get method
            set { initiative = value; }  // set method
        }
        public bool Busy   // property
        {
            get { return busy; }   // get method
            set { busy = value; }  // set method
        }

        public CharacterStats Stats  // property
        {
            get { return stats; }   // get method
        }

        public CharacterClass CharacterClass  // property
        {
            get { return characterClass; }   // get method
        }

        public Mover Mover { get => mover; set => mover = value; }
        #endregion

        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();
            mover = GetComponent<Mover>();
            animator.enabled = false;
            Health = maxHealth = stats.GetStat(Stat.Health, characterClass);
            animator.enabled = true;
            if (animatorOverride)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            //TurnController.OnPlayerTurn += TurnController_OnNewTurn;
        }

        protected virtual void Start()
        {
            unitUI.UpdateHealth(Health, maxHealth);
        }

        private void OnDestroy()
        {
            //TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
        }
        #endregion

        #region Public Methods
        protected override void TakeDamage()
        {
            base.TakeDamage();
            Debug.Log("hello? " + animator.isActiveAndEnabled);
            if(isHealthSet)
            {
                animator.SetTrigger("Hit");
            }
            else
            {
                isHealthSet = true;
            }         
        }
        #endregion

        #region Public Methods
        public int GetStat(Stat stat)
        {
            return Stats.GetStat(stat, this.characterClass);
        }

        public void Stun()
        {
            Debug.Log(name + " stunned");
            GetComponent<Mover>().CanMove = false;
        }

        internal void RollInitive()
        {
            initiative = UnityEngine.Random.Range(0, maxInitiative) + stats.GetStat(Stat.Movement, characterClass);
        }

        public override void Die()
        {
            Debug.Log("Die character");
            
            mover.EndMovement();
            unitController.RemoveUnit(this);
            StartCoroutine(DeathAnimation());          
        }

        public override void Stunned()
        {
            mover.EndMovement();
        }

        internal virtual void StartRound()
        {
            //Debug.Log("Character turn");
        }

        public int CompareTo(object obj)
        {
            return Initiative.CompareTo(obj);
        }

        public virtual Sprite GetBorder()
        {
            return null;
        }

        public virtual void AddToDeck(Card card)
        {
            Debug.Log("card not added to deck");
        }
        #endregion

        #region Coroutines
        IEnumerator DeathAnimation()
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
        #endregion  
    }
}
