using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.Grid;
using System;

namespace FTS.Cards
{
    public abstract class Effect : ScriptableObject
    {
        protected HexGrid grid;
        protected HexGridController gridController;
        protected CardController cardController;
        [HideInInspector] public bool effectFoldout;

        public void Initialize()
        {
            grid = FindObjectOfType<HexGrid>().GetComponent<HexGrid>();
            gridController = FindObjectOfType<HexGridController>().GetComponent<HexGridController>();
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
        }

        public virtual void ActivateEffect()
        {
            Debug.Log("Base effect class");
        }

        public virtual void ActivateEffect(Unit target)
        {
            Debug.Log("Base effect class with target");
        }

        public virtual void ActivateEffect(HexCell target)
        {
            Debug.Log("Base effect class with ground target");
        }

        public virtual void ActivateEffect(Unit player, HexCell target)
        {
            Debug.Log("Base effect class with ground target");
        }

        public virtual string GetEffectText(Player player)
        {
            return "no effect";
        }

        public virtual string GetEffectText()
        {
            return "no effect";
        }
    }
}
