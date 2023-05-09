using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Attack/Attack", fileName = "AttackEffect.asset")]
    public class AttackEffect : Effect, IDamageEffect
    {
        [SerializeField] int numAttacks = 1;
        //[SerializeField] int range = 3;
        //[SerializeField] int area = 1;
        [SerializeField] int damage = 2;

        public override void ActivateEffect(Unit target)
        {
            for (int i = 0; i < numAttacks; i++)
            {
                target.CalculateDamageTaken(damage);
            }
        }

        public override void ActivateEffect(HexCell target)
        {
            if (target.Unit)
            {
                for (int i = 0; i < numAttacks; i++)
                {
                    target.Unit.CalculateDamageTaken(damage);
                }
            }          
        }

        //public override void ActivateEffect(HexCell target)
        //{
        //    //TODO: allow characters to be moved different distances
        //    Debug.Log("UNIT CONTROLLER???? "+ unitController.CurrentPlayer);
        //    HexDirection direction = grid.GetDirection(unitController.CurrentPlayer.Location, target);
        //    List<HexCell> line = grid.GetLine(unitController.CurrentPlayer.Location, direction, projectileRange, true);
        //    if (line.Last().Unit && line.Last().Unit is Character)
        //    {
        //        for (int i = 0; i < numAttacks; i++)
        //        {
        //            line.Last().Unit.CalculateDamageTaken(damage);
        //        }
        //    }
        //}

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

        public int GetTotalDamage(HexCell target)
        {
            return numAttacks * damage;
        }
    }
}
