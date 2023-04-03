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
            Attack(machine);
            controller.ActionDone = true;
        }

        private void Attack(StateMachine machine)
        {
            //AttackDamage(machine);
            machine.enemy.Attack();
            machine.enemy.IsAttacking = false;          
            if (machine.enemy.Indicator)
            {
                machine.enemy.Indicator.ToggleAim(false);
            }
            machine.gridController.Attack(machine.enemy);
        }

        //internal void AttackDamage(StateMachine machine)
        //{
        //    int damage = machine.enemy.Stats.GetStat(Stat.Damage, machine.enemy.CharacterClass);
        //    //Debug.Log("enemy damage: " + damage);
        //    //Debug.Log(machine.newEnemyPosition.Unit.name);
        //    //machine.mover.LookAtTarget(machine.enemy.Target.gameObject.transform.position);
        //    machine.enemy.Target.Unit.CalculateDamageTaken(damage);
        //    machine.enemy.Attack();
        //    //machine.newEnemyPosition.Unit.CalculateDamageTaken(damage);
        //}
    }
}