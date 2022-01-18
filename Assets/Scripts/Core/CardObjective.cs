using UnityEngine;
using FTS.Grid;
using FTS.Cards;
using FTS.Characters;

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
        
        //HexCoordinates location;

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

        public override string SetDescription()
        {
            return "Play " + cardsToPlay + " " + cardType.ToString() + " cards (" + currentPlayed + "/" + cardsToPlay + ")";
        }
    }
}

