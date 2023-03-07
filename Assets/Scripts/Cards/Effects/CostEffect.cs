using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    public enum CostTarget
    {
        AllCopies, ThisCard, Random
    }

    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Cost", fileName = "CostEffect.asset")]
    public class CostEffect : Effect
    {
        [SerializeField] CostTarget costTarget;
        [SerializeField] int costChange = -1;

        public override void ActivateEffect(Unit target)
        {
            cardController.ReduceCost(costTarget, costChange);         
        }

        public override string GetEffectText()
        {
            string effectText = "Reduce the <link=energy><color=\"red\">energy</color></link> cost of ";
            switch (costTarget)
            {
                case CostTarget.AllCopies:
                    effectText += "all copies ";
                    break;
                case CostTarget.ThisCard:
                    effectText += "of this card ";
                    break;
                case CostTarget.Random:
                    effectText += "a random card in your hand  ";
                    break;
                default:
                    break;
            }
            effectText += "by " + -costChange;
            return effectText;
        }
    }
}
