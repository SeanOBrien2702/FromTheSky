using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/AttackTelegraph")]
    public class AttackTelegraphAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            TelegraphAttack(machine);
            controller.ActionDone = true;
        }

        private void TelegraphAttack(StateMachine machine)
        {
            machine.gridController.TelegraphAttack(machine.enemy, machine.mover.Location, machine.enemy.Target.Location);
            machine.enemy.IsAttacking = true;
        }
    }
}