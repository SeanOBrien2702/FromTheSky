using FTS.Characters;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Deck/Discard", fileName = "DiscardEffect.asset")]
    public class DiscardEffect : Effect
    {
        [SerializeField] int cardsDiscarded = 1;
        [SerializeField] bool random = false;

        //TODO: dont depend on passing a useless parameter when adding discard to a card with a target
        public override void ActivateEffect(Unit characters)
        {
            cardController.DiscardCard(cardsDiscarded, random);
        }

        public override string GetEffectText()
        {
            string effectText = "Discard " + cardsDiscarded + " card";
            if (cardsDiscarded > 1)
                effectText += "s";

            if (random)
            {
                effectText += " at random";
            }
            return effectText;
        }
    }
}
