using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Flee")]
    public class FleeAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            Flee(machine);
        }

        private void Flee(StateMachine controller)
        {
            controller.mover.Flee();
            controller.gridController.Flee(controller.mover);
        }
    }
}