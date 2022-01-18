using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Discard", fileName = "DiscardEffect.asset")]
    public class DiscardEffect : Effect
    {
        CardController cc;
        [SerializeField] int cardsDiscarded = 1;
        [SerializeField] bool random = false;
        //TODO: dont depend on passing a useless parameter when adding discard to a card with a target
        public override void ActivateEffect(Character characters)
        {
            Debug.Log("discard Effect activated");
            cc = FindObjectOfType<CardController>().GetComponent<CardController>();          
            cc.DiscardCard(cardsDiscarded, random);
        }

        public override string GetEffectText(Player player)
        {
            string effectText = "Discard " + cardsDiscarded + " card(s)";

            if(random)
            {
                effectText += " at random";
            }
            return effectText;
        }

    }
}
