using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/MoveToPlayer")]
    public class MoveToPlayerAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            MoveToPlayer(machine);
        }

        private void MoveToPlayer(StateMachine controller)
        {
            //controller.gridController.TravelClosestToPlayer(controller.mover, controller.AttackRange);
            //controller.newEnemyPosition = controller.gridController.GetNewEnemyPosition(controller.mover, controller.Targeting, controller.enemy);
            Debug.Log("enemy position: " + controller.mover.Location.Location);
            Debug.Log("move to player target: " + controller.newEnemyPosition.Location);
            //controller.newEnemyPosition.SetDangerIndicator(true);
            controller.gridController.TravelToTarget(controller.mover, controller.AttackRange, controller.newEnemyPosition, Vector3.zero);
        }
    }
}
