using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Heal", fileName = "HealEffect.asset")]
    public class HealEffect : Effect
    {
        [Header("Heal")]
        [SerializeField] int healAmount;

        public override void ActivateEffect()
        {
            unitController.CurrentPlayer.Health += healAmount;
        }

        public override void ActivateEffect(Unit target)
        {
            target.Health += healAmount;
        }

        public override string GetEffectText()
        {
            return "Heal " + healAmount + " damage";
        }

    }
}
