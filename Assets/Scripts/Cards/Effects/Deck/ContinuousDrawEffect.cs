using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Deck/ContinuousDraw", fileName = "DrawEffect.asset")]
    public class ContinuousDrawEffect : Effect
    {
        [Header("CardDraw")]
        [SerializeField] int cardsDrawn;

        public override void ActivateEffect()
        {
            cardController.CardDrawPerTurn(cardsDrawn);
        }

        public override string GetEffectText()
        {
            string effectText = "Draw " + cardsDrawn + " extra card";

            if (cardsDrawn > 1)
            {
                effectText += "s";
            }
            effectText += " each turn";

            return effectText;
        }
    }
}
