using UnityEngine;
using FTS.Grid;

namespace FTS.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/MoveCamera")]
    public class MoveCameraAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            MoveCamera(machine);
        }

        private void MoveCamera(StateMachine machine)
        {
            machine.cameraController.MoveCamera(machine.enemy.Location.transform.position);
        }
    }
}
