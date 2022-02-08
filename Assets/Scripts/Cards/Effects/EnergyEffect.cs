using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.Grid;
using FTS.Cards;

namespace FTS.Cards
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


        public override string GetEffectText()
        {
            return "Gain " + energyGained + " <link=energy><color=\"red\">energy</color></link>";
        }
    }
}
