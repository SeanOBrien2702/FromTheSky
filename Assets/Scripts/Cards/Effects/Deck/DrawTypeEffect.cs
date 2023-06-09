using FTS.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Deck/DrawType", fileName = "DrawTypeEffect.asset")]
    public class DrawTypeEffect : Effect
    {
        [SerializeField] List<CardType> cardTypes;

        public override void ActivateEffect()
        {
            DrawEffect();
        }

        public override void ActivateEffect(Unit target)
        {
            DrawEffect();
        }

        void DrawEffect()
        {
            foreach (var type in cardTypes)
            {
                cardController.DrawCard(type);
            }
        }

        public override string GetEffectText()
        {
            bool isFist = true;
            string effectText = "Draw a";
            foreach (var type in cardTypes)
            {
                if(isFist)
                {
                    effectText += " " + type.ToString();
                }
                else
                {
                    effectText += ", " + type.ToString();
                }
                isFist = false;
            }
            effectText += " card";
            return effectText;
        }
    }
}
