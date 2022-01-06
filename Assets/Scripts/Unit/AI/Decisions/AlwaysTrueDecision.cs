using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SP.Characters
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/AlwaysTrue")]
    public class AlwaysTrueDecision : Decision
    {
        public override bool Decide(StateMachine controller)
        {
            return true;
        }
    }
}
