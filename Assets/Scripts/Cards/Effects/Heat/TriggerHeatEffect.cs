using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Heat/TriggerHeat", fileName = "TriggerHeatEffect.asset")]
    public class TriggerHeatEffect : Effect
    {
        [SerializeField] int numTimes;
        public override void ActivateEffect(Character target)
        {
            target.GetComponent<Heat>().TriggerHeat(numTimes);
        }

        public override string GetEffectText()
        {
            string effectText = "Activate <link=heat><color=\"red\">heat</color></link> instantly";

            if (numTimes > 1)
            {
                effectText += " " + numTimes + " number of times";
            }
            return effectText;
        }
    }
}
