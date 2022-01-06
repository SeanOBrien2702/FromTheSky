using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Characters;
using SP.Grid;
using SP.Cards;

namespace SP.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Energy", fileName = "EnergyEffect.asset")]
    public class EnergyEffect : Effect
    {
        [SerializeField] int energyGained;
        public override void ActivateEffect(Character character)
        {
            FindObjectOfType<CardController>().GetComponent<CardController>().Energy += energyGained;
        }


        public override string GetEffectText(Player player)
        {
            return "Gain " + energyGained + " <link=energy><color=\"red\">energy</color></link>";
        }
    }
}
