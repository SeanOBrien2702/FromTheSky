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
            controller.gridController.TravelToTarget(controller.mover, controller.AttackRange, controller.newEnemyPosition);
        }
    }
}
