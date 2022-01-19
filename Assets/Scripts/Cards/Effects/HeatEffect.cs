using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Heat", fileName = "HeatEffect.asset")]
    public class HeatEffect : Effect
    {
        [SerializeField] int heatAmount;
        public override void ActivateEffect(Character target)
        {
            target.GetComponent<Heat>().HeatLevel += heatAmount;
        }

        public override string GetEffectText(Player player)
        {
            return "Gain " + heatAmount + " <link=heat><color=\"red\">heat</color></link>";
        }

    }
}
