using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Heat", fileName = "HeatEffect.asset")]
    public class HeaTEffect : Effect
    {
        [SerializeField] int heatAmount;
        public override void ActivateEffect(Character target)
        {
            target.GetComponent<Heat>().HeatLevel += heatAmount;
            Debug.Log("Heal effect");
        }

        public override string GetEffectText(Player player)
        {
            return "Gain " + heatAmount + " <link=heat><color=\"red\">energy</color></link>"; 
        }

    }
}
