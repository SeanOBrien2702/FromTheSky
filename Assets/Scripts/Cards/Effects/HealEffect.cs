using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Characters;

namespace SP.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Heal", fileName = "HealEffect.asset")]
    public class HealEffect : Effect
    {
        CardController cc;
        [Header("Heal")]
        [SerializeField] int healAmount;
        public override void ActivateEffect(Character target)
        {
            target.Health += healAmount;
            Debug.Log("Heal effect");
        }

        public override string GetEffectText(Player player)
        {
            return "Heal " + healAmount + " damage";
        }

    }
}
