using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Deck/Draw", fileName = "DrawEffect.asset")]
    public class DrawEffect : Effect
    {
        [Header("CardDraw")]
        [SerializeField] int cardsDrawn;

        public override void ActivateEffect()
        {
            cardController.DrawCard(cardsDrawn);
        }

        public override string GetEffectText()
        {
            string effectText = "Draw " + cardsDrawn + " card";
            if (cardsDrawn > 1)
                effectText += "s";

            if(cardsDrawn >= 10)
            {
                effectText = "Draw until hand is full";
            }

            return effectText;
        }
    }
}
