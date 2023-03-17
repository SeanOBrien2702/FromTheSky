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
            if(machine.enemy.AttackType == AttackTypes.Trajectory)
            {
                machine.gridController.TelegraphTrajectoryAttack(machine.enemy);

            }
            else
            {
                machine.gridController.TelegraphAttack(machine.enemy);
            }
            
            machine.enemy.IsAttacking = true;
        }
    }
}