#region Using Statements
using UnityEngine;
using FTS.Grid;
using System;
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
        [SerializeField] GameObject projectileStart;
        [SerializeField] GameObject projectile;
        [SerializeField] EnemyTargeting targeting;
        [SerializeField] AttackTypes attackType;

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
        #endregion

        #region MonoBehaviour Callbacks
        #endregion

        #region Public Methods
        public void Attack()
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.SetParent(projectileStart.transform, false);
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
