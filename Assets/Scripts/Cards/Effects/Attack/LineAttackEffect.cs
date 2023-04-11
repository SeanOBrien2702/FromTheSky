using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/LineAttack", fileName = "LineAttackEffect.asset")]
    public class LineAttackEffect : Effect, IDamageEffect
    {
        [SerializeField] int numAttacks = 1;
        [SerializeField] int length = 2;
        int damage = 5;

        public override void ActivateEffect(HexCell target)
        {
            HexDirection direction = grid.GetDirection(unitController.CurrentPlayer.Location, target);
            List<HexCell> line = grid.GetLine(unitController.CurrentPlayer.Location, direction, length, false);

            foreach (var cell in line)
            {
                if (cell.Unit)
                {
                    for (int i = 0; i < numAttacks; i++)
                    {
                        cell.Unit.CalculateDamageTaken(damage);
                    }
                }
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
            effectText += " " + length + " hexes forward";
            return effectText;
        }

        public int GetTotalDamage()
        {
            return numAttacks * damage;
        }
    }
}
