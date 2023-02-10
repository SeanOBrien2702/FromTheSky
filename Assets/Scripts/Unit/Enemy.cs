#region Using Statements
using UnityEngine;
using FTS.Grid;
#endregion

namespace FTS.Characters
{
    public class Enemy : Character
    {
        bool isAttacking = false;
        bool canAttack = true;
        Unit target;
        [SerializeField] bool isArchAttack = false;
        [SerializeField] GameObject projectileStart;
        [SerializeField] GameObject projectile;
        [SerializeField] EnemyTargeting targeting;

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

        
        #endregion

        #region MonoBehaviour Callbacks
        //private void Start()
        //{
        //    Debug.Log("enemy placed");
        //}
        #endregion

        #region Public Methods
        public void Attack()
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.SetParent(projectileStart.transform, false);
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
