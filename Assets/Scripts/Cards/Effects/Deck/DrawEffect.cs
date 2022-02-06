using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Deck/Draw", fileName = "DrawEffect.asset")]
    public class DrawEffect : Effect
    {
        CardController cc;
        [Header("CardDraw")]
        [SerializeField] int cardsDrawn;
        public override void ActivateEffect()
        {
            cc = FindObjectOfType<CardController>().GetComponent<CardController>();
            Debug.Log("Effect activated");
            cc.DrawCard(cardsDrawn);
        }

        public override string GetEffectText()
        {
            string effectText = "Draw " + cardsDrawn + " card";
            if(cardsDrawn > 1)
                effectText += "s";
           
            return effectText;
        }
    }
}
