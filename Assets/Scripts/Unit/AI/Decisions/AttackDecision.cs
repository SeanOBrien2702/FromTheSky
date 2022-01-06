using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Turns;

namespace SP.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Attack")]
    public class AttackDecision : Decision
    {

        public override bool Decide(StateMachine controller)
        {
            return IsAttacking(controller) && CanAttack(controller) && IsAttackPhase(controller); 
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
