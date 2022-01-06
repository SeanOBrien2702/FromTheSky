using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SP.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/PlayerInRange")]
    public class PlayerInRangeDecision : Decision
    {
        public override bool Decide(StateMachine controller)
        {
            return IsInRange(controller);
        }

        private bool IsInRange(StateMachine controller)
        {
            //Check if in attack range
            bool isInRange = true;
            isInRange = controller.gridController.PlayerInRange(controller.enemy);
            return isInRange;
        }
    }
}
