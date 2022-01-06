using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/AttackTelegraph")]
    public class AttackTelegraphAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            //TelegraphAttack(machine);
            //controller.ActionDone = true;
        }

        //private void TelegraphAttack(StateMachine machine)
        //{
        //    machine.gridController.TelegraphVehicleAttack(machine.mover.Location, machine.AttackRange);
        //    machine.enemy.IsAttacking = true;
        //}
    }
}