using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SP.Characters
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller, StateMachine machine);
    }
}