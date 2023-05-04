using FTS.Cards;
using FTS.Characters;
using FTS.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Grid
{
    public class DamageHighlightController : MonoBehaviour
    {
        [SerializeField] CardController cardController;
        AttackIndicatorController indicatorController;

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            indicatorController = GetComponent<AttackIndicatorController>();
            Unit.OnHover += Unit_OnHover;
            Unit.OnHoverExit += Unit_OnHoverExit;
        }

        private void OnDestroy()
        {
            Unit.OnHover -= Unit_OnHover;
            Unit.OnHoverExit -= Unit_OnHoverExit;
        }
        #endregion

        #region Private Methods
        private void ApplyHighlights(AttackIndicator lines, Enemy enemy)
        {
            foreach (HexCell cell in lines.Line)
            {
                if (cell.Unit)
                {
                    int damage = enemy.Stats.GetStat(Stat.Damage, enemy.CharacterClass);
                    cell.Unit.ShowDamage(damage);
                }
            }
        }

        private void ClearHighlights(AttackIndicator lines, Enemy enemy)
        {
            foreach (HexCell cell in lines.Line)
            {
                if (cell.Unit)
                {
                    cell.Unit.ShowDamage(0);
                }
            }
        }
        #endregion

        #region Events
        private void Unit_OnHoverExit(Unit unit)
        {
            if (!(unit is Enemy))
            {
                return;
            }
            Enemy enemy = (Enemy)unit;

            if (cardController.CardSelected)// &&
                //cardController.CardSelected.Targeting == CardTargeting.Unit)
            {
                enemy.ShowDamage(0);
            }
            else if (indicatorController.AttackIndicators.ContainsKey(enemy))
            {
                foreach (var lines in indicatorController.AttackIndicators[enemy].Lines)
                {
                    ClearHighlights(lines, enemy);                
                }
            }
        }

        private void Unit_OnHover(Unit unit)
        {
            if (!(unit is Enemy))
            {
                return;
            }

            Enemy enemy = (Enemy)unit;
            if (cardController.CardSelected)// &&
                //cardController.CardSelected.Targeting == CardTargeting.Unit)
            {
                enemy.ShowDamage(cardController.GetDamage(enemy.Location));
            }
            else if (indicatorController.AttackIndicators.ContainsKey(enemy))
            {
                foreach (var lines in indicatorController.AttackIndicators[enemy].Lines)
                {
                    ApplyHighlights(lines, enemy);                   
                }
            }        
        }        
        #endregion
    }
}
