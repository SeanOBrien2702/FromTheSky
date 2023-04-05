using UnityEngine;
using FTS.Grid;
using FTS.Cards;
using FTS.Characters;
using FTS.Turns;
using AMPInternal.Utilities;

namespace FTS.Core
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/CardObjective", fileName = "CardObjective.asset")]
    public class CardObjective : Objective
    {
        [SerializeField] int cardsToPlay;
        [SerializeField] CardType cardType;
        int currentPlayed = 0;

        public override void UpdateObjective(Card card)
        {
            if (card.Type == cardType)
            {
                currentPlayed++;
            }

            
            if (currentPlayed >= cardsToPlay)
            {
                currentPlayed = cardsToPlay;
                isComplete = true;
            }
        }

        public override string SetDescription(bool isEncounter = false)
        {
            string description = "Play " + cardsToPlay + " " + cardType.ToString() + " cards";
            if (!isEncounter)
            {
                description += " (" + currentPlayed + "/" + cardsToPlay + ")";
            }
            return description;
        }
    }
}

