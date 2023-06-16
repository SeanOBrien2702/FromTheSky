using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/CardTypeAttack", fileName = "CardTypeAttackEffect.asset")]
    public class CardTypeAttackEffect : Effect, IDamageEffect
    {
        [SerializeField] CardType cardType;
        public override void ActivateEffect(Unit target)
        {
            target.CalculateDamageTaken(cardController.GetCardTypes(cardType));
        }

        public override void ActivateEffect(HexCell target)
        {
            if (target.Unit)
            {
                target.Unit.CalculateDamageTaken(cardController.GetCardTypes(cardType));               
            }
        }

        public override string GetEffectText()
        {
            return "Deal " + cardController.GetCardTypes(cardType) + " damage for each " + cardType.ToString() + "card type";
        }

        public int GetTotalDamage(HexCell target)
        {
            return cardController.GetCardTypes(cardType);
        }
    }
}
