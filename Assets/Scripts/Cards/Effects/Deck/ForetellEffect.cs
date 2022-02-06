using FTS.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Deck/Foretell", fileName = "ForetellEffect.asset")]
    public class ForetellEffect : Effect
    {
        [Range(2,5)]
        [SerializeField] int foretellAmount;
        public override void ActivateEffect(Character target)
        {
            ForetellController foretell = FindObjectOfType<ForetellController>().GetComponent<ForetellController>();
            foretell.Foretell(foretellAmount);
        }

        public override string GetEffectText(Player player)
        {
            return "<link=foretell><color=\"red\">Foretell</color></link> " + foretellAmount;
        }
    }
}
