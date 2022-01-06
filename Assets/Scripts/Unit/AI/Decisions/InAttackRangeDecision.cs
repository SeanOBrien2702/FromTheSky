using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Turns;

namespace SP.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/InAttackRange")]
    public class InAttackRangeDecision : Decision
    {
        public override bool Decide(StateMachine controller)
        {
            Debug.Log("in attack range?");
            //controller.playerTarget = controller.gridController.GetClosestPLayer
            controller.newEnemyPosition = controller.gridController.GetNewEnemyPosition(controller.mover, controller.Targeting, controller.enemy);
            //Debug.Log("set new position to move to " + controller.newEnemyPosition);
            return CanReach(controller) && !IsInRange(controller);// && IsAttackNotBlocked(controller); && IsTelegraphPhase(controller);
        }

        private bool IsAttackNotBlocked(StateMachine controller)
        {
            bool isAttackNotBlocked = true;
            if(!controller.enemy.IsArchAttack)
            {
                isAttackNotBlocked = controller.gridController.IsAttackNotBlocked(controller.enemy);
            }
            return isAttackNotBlocked;
        }

        private bool CanReach(StateMachine controller)
        {
            return controller.gridController.CanReachAttackRange(controller.enemy, controller.newEnemyPosition);
        }


        private bool IsInRange(StateMachine controller)
        {
            //Check if in attack range
            bool isInRange = true;
            isInRange = controller.gridController.PlayerInRange(controller.enemy);
            return isInRange;
        }

        private bool IsTelegraphPhase(StateMachine controller)
        {
            return controller.turnController.TurnPhase == TurnPhases.EnemyTelegraph;
        }
    }
}
