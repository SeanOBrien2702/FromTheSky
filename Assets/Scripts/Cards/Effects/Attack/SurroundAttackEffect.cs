using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/SurroundAttack", fileName = "SurroundAttackEffect.asset")]
    public class SurroundAttackEffect : Effect, IDamageEffect
    {
        int damage;

        public override void ActivateEffect(Unit target)
        {
            Debug.Log("card played effect active????????!!!");
            target.CalculateDamageTaken(GetTotalDamage(target.Location));
        }

        public override string GetEffectText()
        {
            return "Deal 1 damage for each friendly unit beside target";
        }

        public int GetTotalDamage(HexCell target)
        {
            damage = 0;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = target.GetNeighbor(d);
                if (neighbor.IsFrendlyUnit())
                {
                    damage += 1;
                }
            }
            Debug.Log("get damage " + damage);
            return damage;
        }
    }
}
