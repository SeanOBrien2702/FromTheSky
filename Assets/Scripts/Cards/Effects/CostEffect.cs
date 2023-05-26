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

        public override void ActivateEffect()
        {
            Debug.Log("card controller " +cardController);
            cardController.ReduceCost(costTarget, costChange);         
        }

        public override string GetEffectText()
        {
            string effectText = costChange > 0 ? "+" : "-";

            effectText += costChange + " <link=energy><color=\"red\">energy</color></link> cost  ";
            switch (costTarget)
            {
                case CostTarget.AllCopies:
                    effectText += "of all copies ";
                    break;
                case CostTarget.Random:
                    effectText += "to random card";
                    break;
                default:
                    break;
            }
            //effectText += "by " + -costChange;
            return effectText;
        }
    }
}
