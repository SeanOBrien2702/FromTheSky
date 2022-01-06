using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Turns;

namespace SP.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/OutsideAttackRange")]
    public class OutsideAttackRangeDecision : Decision
    {
        public override bool Decide(StateMachine controller)
        {
            //controller.playerTarget = controller.gridController.GetClosestPLayer
            //controller.newEnemyPosition = controller.gridController.GetNewEnemyPosition(controller.mover, controller.Targeting, controller.enemy);
            //Debug.Log("set new position to move to " + controller.newEnemyPosition);
            return !CanReach(controller) && !IsInRange(controller);// && IsAttackNotBlocked(controller); && IsTelegraphPhase(controller);
        }

        private bool CanReach(StateMachine controller)
        {
            //Debug.Log("enemy name " + controller.enemy.name);
            //Debug.Log("enemy target name " + controller.enemy.Target.Unit.name);

            return controller.gridController.CanReachAttackRange(controller.enemy, controller.newEnemyPosition);
        }


        private bool IsInRange(StateMachine controller)
        {
            //Check if in attack range
            bool isInRange = true;
            isInRange = controller.gridController.PlayerInRange(controller.enemy);
            return isInRange;
        }
    }
}
