using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/MoveToVehicle")]
    public class MoveToVehicleAction : Action
    {
        public override void Act(StateController controller, StateMachine machine)
        {
            //Move(machine);
        }

        //private void Move(StateMachine controller)
        //{
        //    //Move to an offset of the vehicle depending on enemy range
        //    controller.gridController.TravelToTarget(controller.mover, 
        //                                            controller.AttackRange, 
        //                                            controller.playerTarget, 
        //                                            controller.unitController.GetVehiclePosition().transform.position);
        //}
    }
}
