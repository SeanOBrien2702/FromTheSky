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
            if (machine.enemy.CanAttack)
            {
                TelegraphAttack(machine);
                if (machine.enemy.Indicator)
                {
                    machine.enemy.Indicator.ToggleAim(true);
                }
                machine.telegraphIntentUI.ShowAttack(machine.enemy.GetStat(Stat.Damage));
            }
            controller.ActionDone = true;
        }

        private void TelegraphAttack(StateMachine machine)
        {
            if(machine.enemy.AttackType == AttackTypes.Trajectory)
            {
                machine.attackIndicator.TelegraphTrajectoryAttack(machine.enemy);

            }
            else
            {
                machine.attackIndicator.TelegraphAttack(machine.enemy);
            }
            
            machine.enemy.IsAttacking = true;
        }
    }
}