using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Stun")]
    public class StunAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            Stun(machine);
            controller.ActionDone = true;
        }

        private void Stun(StateMachine controller)
        {
            if (controller.newEnemyPosition.Unit)
            {
                controller.newEnemyPosition.Unit.Stun();
            }
        }
    }
}
