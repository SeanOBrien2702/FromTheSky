using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.Grid;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/LineAttack", fileName = "LineAttackEffect.asset")]
    public class LineAttackEffect : Effect
    {
        [SerializeField] int numAttacks = 1;
        [SerializeField] int length = 2;

        int damage = 5;
        public override void ActivateEffect(Character player,  HexCell target)
        {
            List<HexCell> area = FindObjectOfType<HexGrid>().GetComponent<HexGrid>().GetLine(player, target, length);
  
            foreach (var cell in area)
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


        public override string GetEffectText(Player player)
        {
            damage = player.GetStat(Stat.Damage);
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

        public override string GetEffectText()
        {
            
            string effectText;
            if (numAttacks == 1)
            {
                effectText = "Deal *Character damage* damage";
            }
            else
            {
                effectText = "Deal *Character damage* damage " + numAttacks + " times";
            }
            effectText += " " + length + " hexes forward";
            return effectText;
        }

    }
}
