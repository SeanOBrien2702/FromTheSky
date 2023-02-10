using FTS.Characters;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/Attack", fileName = "AttackEffect.asset")]
    public class AttackEffect : Effect
    {
        [Header("Combat")]
        [SerializeField] int numAttacks = 1;
        [SerializeField] int range = 3;
        [SerializeField] int area = 1;

        int damage = 2;
        public override void ActivateEffect(Unit target)
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
