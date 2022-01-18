using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FTS.Characters
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(StateMachine controller);
    }
}