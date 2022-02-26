#region Using Statements
using UnityEngine;
using FTS.Cards;
using System.Collections.Generic;
using System;
#endregion

namespace FTS.Characters
{
    public class Player : Character
    {
        [SerializeField] Color colour;
        bool placed = false;

        #region Properties
        public Color Colour   // property
        {
            get { return colour; }   // get method
            set { colour = value; }  // set method
        }

        public bool Placed   // property
        {
            get { return placed; }   // get method
            set { placed = value; }  // set method
        }
        #endregion

        #region MonoBehaviour Callbacks
        //void Start()
        //{
        //    placed = false;
        //    Debug.Log("player placed");
        //}
        //private void Start()
        //{
        //    Debug.Log("player placed in player script");
        //    Debug.Log("hello?????");
        //    unitUI.UpdateHealth(health, maxHealth);
        //    Debug.Log("hello?");
        //}
        #endregion

        #region Public Methods
        internal override void StartRound()
        {
            //Debug.Log("player turn");
        }

        internal int GetCardRange(CardType type)
        {
            int range = 0;
            switch (type)
            {
                case CardType.Attack:
                    range = Stats.GetStat(Stat.AttackRange, CharacterClass);
                    break;
                case CardType.Support:
                    range = Stats.GetStat(Stat.SupportRange, CharacterClass);
                    break;
                case CardType.Enhancement:
                    range = Stats.GetStat(Stat.SupportRange, CharacterClass);
                    break;
                default:
                    Debug.LogError("Card type missing");
                    break;
            }
            return range;
        }
        #endregion
    }
}
