using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Characters;

namespace SP.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Create Card", fileName = "CreateCardEffect.asset")]
    public class CreateCardEffect : Effect
    {
        CardController cc;
        [Header("CardDraw")]
        [SerializeField] Card card;
        [SerializeField] bool isTemporary = true;
        [SerializeField] CardLocation cardLocation = CardLocation.Hand;
        [SerializeField] int numCopies = 1;
        public override void ActivateEffect(Character target)
        {
            cc = FindObjectOfType<CardController>().GetComponent<CardController>();
            if(numCopies <= 0)           
                numCopies = 1;
            
            for (int i = 0; i < numCopies; i++)
            {
                cc.AddCard(card, isTemporary, cardLocation);
            }
            Debug.Log("Create card effect");
        }

        public override string GetEffectText(Player player)
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
