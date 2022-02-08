using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Heat/HeatProof", fileName = "HeatProofEffect.asset")]
    public class HeatProofEffect : Effect
    {
        public override void ActivateEffect(Character target)
        {
            target.GetComponent<Heat>().FriendlyFire = false;
        }

        public override string GetEffectText()
        {
            return "<link=heat><color=\"red\">Heat</color></link> no longer damages allies";
        }

    }
}
