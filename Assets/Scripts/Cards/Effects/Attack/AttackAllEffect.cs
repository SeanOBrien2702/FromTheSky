using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/AttackAll", fileName = "AttackAllEffect.asset")]
    public class AttackAllEffect : Effect, IDamageEffect
    {
        [SerializeField] int numAttacks = 1;
        [SerializeField] int damage = 1;

        public override void ActivateEffect()
        {
            foreach (var enemy in unitController.GetEnemyUnits().ToList())
            {
                enemy.CalculateDamageTaken(damage);
            }
        }

        public override string GetEffectText()
        {
            string effectText;
            if (numAttacks == 1)
            {
                effectText = "Deal " + damage + " damage";
            }
            else
            {
                effectText = "Deal " + damage + " damage " + numAttacks + " times";
            }

            effectText += " to all enemies";
            return effectText;
        }

        public int GetTotalDamage(HexCell target)
        {
            return numAttacks * damage;
        }
    }
}
