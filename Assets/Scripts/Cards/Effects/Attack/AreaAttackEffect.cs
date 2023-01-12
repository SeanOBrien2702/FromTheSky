using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/AreaAttack", fileName = "AreasAttackEffect.asset")]
    public class AreaAttackEffect : Effect
    {
        [Header("Combat")]
        [SerializeField] int numAttacks = 1;
        [SerializeField] int radius = 2;
        [SerializeField] bool friendlyFire = false;
        int damage = 5;

        public override void ActivateEffect(HexCell target)
        {
            grid = FindObjectOfType<HexGrid>().GetComponent<HexGrid>();
            List<HexCell> area = grid.GetArea(target, radius);
            Debug.Log("area size " + area.Count);
            foreach (var cell in area)
            {
                for (int i = 0; i < numAttacks; i++)
                {
                    if (cell.Unit)
                        cell.Unit.CalculateDamageTaken(damage);
                }
            }
            grid.ClearArea();
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
            effectText += " in a " + radius + " hex radius";
            return effectText;
        }

    }
}
