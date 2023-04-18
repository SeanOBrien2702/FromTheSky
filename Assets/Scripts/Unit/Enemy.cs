#region Using Statements
using UnityEngine;
using FTS.Grid;
using FTS.UI;
using WalldoffStudios.Indicators;
using AeLa.EasyFeedback.APIs;
#endregion

namespace FTS.Characters
{
    public class Enemy : Character 
    {    
        bool isAttacking = false;
        bool canAttack = true;
        HexDirection direction;
        Unit target;
        [SerializeField] bool isArchAttack = false;

        [SerializeField] EnemyTargeting targeting;
        [SerializeField] AttackTypes attackType;
        [SerializeField] TelegraphIntentUI intentUI;
        [SerializeField] IndicatorController indicator;

        [Header("Attack animation")]
        [SerializeField] GameObject projectileStart;
        [SerializeField] Projectile projectileAnimation;
        [SerializeField] GameObject piercingAnimation;
        GameObject piercingInstance;

        #region Properties
        public bool IsAttacking   // property
        {
            get { return isAttacking; }   // get method
            set { isAttacking = value; }  // set method
        }

        public bool CanAttack   // property
        {
            get { return canAttack; }   // get method
            set { canAttack = value; }  // set method
        }

        public Unit Target   // property
        {
            get { return target; }   // get method
            set { target = value; }  // set method
        }

        public bool IsArchAttack   // property
        {
            get { return isArchAttack; }   // get method
        }

        public int Range
        { 
            get { return this.Stats.GetStat(Stat.AttackRange, this.CharacterClass); }
        }
        public EnemyTargeting Targeting
        {
            get { return targeting; }
        }

        public HexDirection Direction 
        { 
            get => direction; 
            set => direction = value; 
        }

        public AttackTypes AttackType
        {
            get { return attackType; }
        }

        public TelegraphIntentUI IntentUI { get => intentUI; set => intentUI = value; }
        public IndicatorController Indicator { get => indicator; set => indicator = value; }
        #endregion

        #region MonoBehaviour Callbacks
        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();
            if (indicator)
                indicator.ShotIndicator();
        }

        protected override void OnMouseExit()
        {
            base.OnMouseExit();
            if (indicator)
                indicator.IndicatorResetFillAmount();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }
        #endregion

        #region Public Methods
        public void Attack()
        {
            animator.SetTrigger("Shoot");
            if (attackType == AttackTypes.Projectile)
            {
                Projectile proectile = Instantiate(projectileAnimation, projectileStart.transform.position, projectileStart.transform.rotation);
                proectile.Damage = GetStat(Stat.Damage);
            }
            else if(attackType == AttackTypes.Piercing)
            {
                Destroy(piercingInstance);
                piercingInstance = Instantiate(piercingAnimation, projectileStart.transform.position, projectileStart.transform.rotation);
                piercingInstance.transform.parent = transform;
                Destroy(piercingInstance, 0.5f);
            }
        }

        internal bool IsPiercieing()
        {
            return attackType == AttackTypes.Piercing ? true : false; 
        }
        #endregion
    }

    public enum EnemyTargeting
    {
        OnlyVehicle,
        OnlyPlayers,
        AllCharacters
    }
}
