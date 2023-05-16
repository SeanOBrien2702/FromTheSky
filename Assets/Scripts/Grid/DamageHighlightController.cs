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
        List<Unit> highlights = new List<Unit>();

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            indicatorController = GetComponent<AttackIndicatorController>();
            Unit.OnHover += Unit_OnHover;
            Unit.OnHoverExit += Unit_OnHoverExit;
            UnitController.OnEnemyKilled += UnitController_OnEnemyKilled;
            HandController.OnCardSelected += HandController_OnCardSelected;
        }

        private void OnDestroy()
        {
            Unit.OnHover -= Unit_OnHover;
            Unit.OnHoverExit -= Unit_OnHoverExit;
            UnitController.OnEnemyKilled += UnitController_OnEnemyKilled;
            HandController.OnCardSelected -= HandController_OnCardSelected;
        }
        #endregion

        #region Private Methods
        internal void UpdateHighlight(List<HexCell> targetArea, Enemy enemy = null)
        {
            ClearHighlights();
            foreach (var cell in targetArea)
            {
                if (!cell.Unit)
                {
                    continue;
                }
                if (enemy)
                {                 
                    int damage = enemy.Stats.GetStat(Stat.Damage, enemy.CharacterClass);
                    cell.Unit.ShowDamage(damage);
                }
                else
                {
                    cell.Unit.ShowDamage(cardController.GetDamage(cell));                    
                }
                highlights.Add(cell.Unit);
            }
        }

        void ClearHighlights()
        {
            foreach (Unit unit in highlights)
            {
                unit.ShowDamage(0);
            }
            highlights.Clear();
        }
        #endregion

        #region Events
        private void Unit_OnHoverExit(Unit unit)
        {
            if (!(unit is Enemy))
            {
                return;
            }
            ClearHighlights();
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
                highlights.Add(enemy);
            }
            else if (indicatorController.AttackIndicators.ContainsKey(enemy))
            {
                ClearHighlights();
                foreach (var lines in indicatorController.AttackIndicators[enemy].Lines)
                {
                    UpdateHighlight(lines.Line, enemy);                 
                }
            }        
        }

        private void HandController_OnCardSelected(string obj)
        {
            ClearHighlights();
        }

        private void UnitController_OnEnemyKilled(Character enemy)
        {
            if(highlights.Contains(enemy))
            {
                highlights.Remove(enemy);
            }
        }
        #endregion
    }
}
