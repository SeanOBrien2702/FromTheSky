using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Create Card", fileName = "CreateCardEffect.asset")]
    public class CreateCardEffect : Effect
    {
        [Header("CardDraw")]
        [SerializeField] Card card;
        [SerializeField] bool isTemporary = true;
        [SerializeField] CardLocation cardLocation = CardLocation.Hand;
        [SerializeField] int numCopies = 1;

        public override void ActivateEffect(Character target)
        {
            if(numCopies <= 0)           
                numCopies = 1;
            
            for (int i = 0; i < numCopies; i++)
            {
                cardController.AddCard(card, isTemporary, cardLocation);
            }
        }

        public override string GetEffectText()
        {
            string effectText = "Add ";
            if(numCopies == 1)
            {
                effectText += "a ";
            }
            else
            {
                effectText += numCopies + " ";
            }
            if(isTemporary)
            {
                effectText += "<link=temporary><color=\"red\">temporary</color></link> ";
            }
            effectText += card.CardName + " to your ";
            if(cardLocation == CardLocation.Hand)
            {
                effectText += "hand.";
            }
            else
            {
                effectText += "deck.";
            }
            return effectText;
        }
    }
}
