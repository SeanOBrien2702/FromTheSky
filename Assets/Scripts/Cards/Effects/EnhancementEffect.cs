using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP.Cards
{
    public class EnhancementEffect : Effect
    {
        [Header("Enhancement")]
        [SerializeField] int armour = 5;
        public override void ActivateEffect()
        {
            Debug.Log("Enhancement effect");
        }
    }
}
