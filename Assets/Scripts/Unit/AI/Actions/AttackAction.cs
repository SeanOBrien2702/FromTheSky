using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
    public class AttackAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            Attack(controller, machine);
            if (machine.enemy.AttackType != AttackTypes.Piercing)
            {
                controller.ActionDone = true;
            }
        }

        private void Attack(StateController controller, StateMachine machine)
        {
            machine.enemy.Attack();
            machine.enemy.IsAttacking = false;          
            if (machine.enemy.Indicator)
            {
                machine.enemy.Indicator.ToggleAim(false);
            }
            if (machine.enemy.AttackType != AttackTypes.Projectile)
            {
                machine.attackIndicator.Attack(controller, machine.enemy);            
            }
            else
            {
                machine.attackIndicator.RemoveIndicator(machine.enemy);
            }
        }
    }
}