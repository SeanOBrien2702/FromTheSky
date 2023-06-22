using FTS.Cards;
using FTS.Characters;
using FTS.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace FTS.Grid
{
    public class AttackIndicatorController : MonoBehaviour
    {
        Dictionary<Enemy, AttackIndicator> attackIndicators = new Dictionary<Enemy, AttackIndicator>();
        HexGrid grid;
        //[SerializeField] CardController cardController;

        int projectileRange = 9999;

        public Dictionary<Enemy, AttackIndicator> AttackIndicators { get => attackIndicators; set => attackIndicators = value; }

        private void Awake()
        {
            grid = GetComponent<HexGrid>();
            Mover.OnMoved += Mover_OnMoved;
            UnitController.OnEnemyKilled += UnitController_OnEnemyKilled;
            UnitController.OnEnemyStunned += UnitController_OnEnemyStunned;
        }

        private void OnDestroy()
        {
            Mover.OnMoved -= Mover_OnMoved;
            UnitController.OnEnemyKilled -= UnitController_OnEnemyKilled;
            UnitController.OnEnemyStunned -= UnitController_OnEnemyStunned;
        }

        public void UpdateIndicators(Enemy enemy)
        {
            if (attackIndicators.ContainsKey(enemy))
            {
                foreach (AttackIndicator lines in attackIndicators[enemy].Lines)
                {
                    UpdateLine(enemy, lines);
                }              
            }
        }

        public void UpdateIndicators(HexCell oldLocation, HexCell newLocation)
        {
            //if enemy is pushed
            if(newLocation?.Unit is Enemy)
            {
                UpdateIndicators((Enemy)newLocation.Unit);
            }

            foreach (var indicator in attackIndicators)
            {
                foreach (AttackIndicator line in indicator.Value.Lines)
                {
                    if (line.Line.Contains(oldLocation) ||
                        line.Line.Contains(newLocation))
                    {
                        UpdateLine(indicator.Key, line);
                    }
                }
            }
        }

        private void UpdateLine(Enemy enemy, AttackIndicator indicator)
        {
            if (indicator.Line.Count <= 0)
            {
                return;
            }

            foreach (HexCell cell in indicator.Line)
            {
                cell.SetDangerIndicator(false);
                cell.SetDangerous(false);
            }

            if (enemy.IsPiercieing())
            {
                indicator.Line = grid.GetLine(enemy.Location, indicator.Direction, enemy.Range, CardTargeting.Piercing);
                foreach (HexCell cell in indicator.Line)
                {
                    cell.SetDangerIndicator(true);
                }
            }
            else
            {             
                indicator.Line = grid.GetLine(enemy.Location, indicator.Direction, projectileRange, CardTargeting.Projectile);
                indicator.Line.Last().SetDangerIndicator(true);               
            }
            foreach (HexCell cell in indicator.Line)
            {
                cell.SetDangerous(true);
            }
        }

        public void TelegraphTrajectoryAttack(Enemy enemy)
        {
            AttackIndicator indicator = new AttackIndicator(enemy.Target.Location, enemy.Direction);

            enemy.Target.Location.SetDangerIndicator(true);


            attackIndicators.Add(enemy, indicator);
        }

        public void TelegraphAttack(Enemy enemy)
        {
            AttackIndicator indicator;
            if (enemy.IsPiercieing())
            {
                List<AttackIndicator> indicators = new List<AttackIndicator>();
                foreach (AttackDirections direction in enemy.AttackDirections)
                {
                    HexDirection localDirection = HexDirectionExtensions.LocalDirection(enemy.Direction, direction);
                    AttackIndicator indicatorBuff = new AttackIndicator(grid.GetLine(enemy.Location, localDirection, enemy.Range, CardTargeting.Piercing), localDirection);
                    foreach (HexCell cell in indicatorBuff.Line)
                    {
                        cell.SetDangerIndicator(true);
                    }
                    indicators.Add(indicatorBuff);
                }
                indicator = new AttackIndicator(indicators, enemy.Direction);
            }
            else
            {
                List<AttackIndicator> indicators = new List<AttackIndicator>();
                foreach (AttackDirections direction in enemy.AttackDirections)
                {                  
                    HexDirection localDirection = HexDirectionExtensions.LocalDirection(enemy.Direction, direction);
                    AttackIndicator indicatorBuff = new AttackIndicator(grid.GetLine(enemy.Location, localDirection, projectileRange, CardTargeting.Projectile), localDirection);
                    if (indicatorBuff.Line.Count > 0)
                    {
                        indicatorBuff.Line.Last().SetDangerIndicator(true);
                        indicators.Add(indicatorBuff);
                    }
                }
                indicator = new AttackIndicator(indicators, enemy.Direction);
            }
            foreach (var line in indicator.Lines)
            {
                foreach (var cell in line.Line)
                {
                    cell.SetDangerous(true);
                }
            }

            attackIndicators.Add(enemy, indicator);
        }

        //TODO: make this based on animation
        public void Attack(StateController controller, Enemy enemy)
        {
            if (!attackIndicators.ContainsKey(enemy))
            {
                controller.ActionDone = true;
                return;
            }
            
            StartCoroutine(DelayAttack(controller, enemy));
        }

        private IEnumerator DelayAttack(StateController controller, Enemy enemy)
        {
            yield return new WaitForSeconds(0.75f);
            foreach (AttackIndicator lines in attackIndicators[enemy].Lines)
            {
                foreach (HexCell cell in lines.Line)
                {
                    cell.SetDangerIndicator(false);
                    if (cell.Unit)
                    {
                        int damage = enemy.Stats.GetStat(Stat.Damage, enemy.CharacterClass);
                        cell.Unit.CalculateDamageTaken(damage);
                    }
                }
            }
            attackIndicators.Remove(enemy);
            controller.ActionDone = true;
        }

        internal void RemoveIndicator(Enemy unit)
        {
            if (!attackIndicators.ContainsKey(unit))
            {
                return;
            }

            foreach (AttackIndicator lines in attackIndicators[unit].Lines)
            {
                foreach (HexCell cell in lines.Line)
                {
                    cell.SetDangerIndicator(false);
                }
            }

            attackIndicators.Remove(unit);
        }

        private void Mover_OnMoved(HexCell oldCell, HexCell newCell)
        {
            UpdateIndicators(oldCell, newCell);
        }

        private void UnitController_OnEnemyKilled(Character enemy)
        {
            Debug.Log("enemy killed? " + enemy.name);
            RemoveIndicator((Enemy)enemy);
        }

        private void UnitController_OnEnemyStunned(Enemy enemy)
        {
            RemoveIndicator(enemy);
        }       
    }
}
