using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Characters;

namespace SP.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Barrier", fileName = "BarrierEffect.asset")]
    public class BarrierEffect : Effect
    {
        public override void ActivateEffect(Character target)
        {
            target.HasBarrier = true;
            Debug.Log("barrier effect played");
        }

        public override string GetEffectText(Player player)
        {
            return "Gain a <link=barrier><color=\"red\">barrier</color></link>"; 
        }
    }
}
