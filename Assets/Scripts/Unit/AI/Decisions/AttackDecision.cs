using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Turns;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Attack")]
    public class AttackDecision : Decision
    {
        public override bool Decide(StateMachine controller)
        {
            return CanAttack(controller);// IsAttacking(controller) && CanAttack(controller) && IsAttackPhase(controller); 
        }

        private bool IsAttackPhase(StateMachine controller)
        {
            return controller.turnController.TurnPhase == TurnPhases.EnemyActions;
        }

        private bool CanAttack(StateMachine controller)
        {
            return controller.enemy.CanAttack;
        }

        private bool IsAttacking(StateMachine controller)
        {
            return controller.enemy.IsAttacking;
        }
    }
}
