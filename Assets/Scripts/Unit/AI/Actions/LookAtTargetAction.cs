using UnityEngine;
using FTS.Grid;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/LookAtPlayer")]
    public class LookAtTargetAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            LookAt(machine);
        }

        private void LookAt(StateMachine machine)
        {
            machine.mover.LookAt(HexDirectionExtensions.Angle(machine.enemy.Direction));
        }
    }
}
