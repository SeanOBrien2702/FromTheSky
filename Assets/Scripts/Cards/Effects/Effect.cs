using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Characters;
using System;

namespace SP.Cards
{
    public abstract class Effect : ScriptableObject
    {
        public virtual void ActivateEffect()
        {
            Debug.Log("Base effect class");
        }

        public virtual void ActivateEffect(Character target)
        {
            Debug.Log("Base effect class with target");
        }

        public virtual string GetEffectText(Player player)
        {
            return "no effect";
        }

        public virtual string GetEffectText()
        {
            return "no effect";
        }
    }
}
