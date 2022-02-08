using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.Grid;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/AreaAttack", fileName = "AreasAttackEffect.asset")]
    public class AreaAttackEffect : Effect
    {
        //public string effectText;
        [Header("Combat")]
        [SerializeField] int numAttacks = 1;
        [SerializeField] int radius = 2;
        [SerializeField] bool friendlyFire = false;

        HexGrid hexGrid;
        void Start()
        {
            //mover = GetComponent<Mover>();
            hexGrid = FindObjectOfType<HexGrid>().GetComponent<HexGrid>();
        }


        int damage = 5;
        public override void ActivateEffect(HexCell target)
        {
            List<HexCell> area = hexGrid.GetArea(target, radius);
            Debug.Log("area size " + area.Count);
            foreach (var cell in area)
            {
                for (int i = 0; i < numAttacks; i++)
                {
                    cell.Unit.CalculateDamageTaken(damage);
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
            effectText += " in a " + radius + " hex radius";
            return effectText;
        }

    }
}
