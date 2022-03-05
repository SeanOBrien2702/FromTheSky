using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/Attack", fileName = "AttackEffect.asset")]
    public class AttackEffect : Effect
    {
        //public string effectText;
        [Header("Combat")]
        [SerializeField] int numAttacks = 1;
        [SerializeField] int range = 3;
        [SerializeField] int area = 1;

        int damage = 2;
        public override void ActivateEffect(Character target)
        {
            for (int i = 0; i < numAttacks; i++)
            {
                target.CalculateDamageTaken(damage);
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
            return effectText;
        }

    }
}
