using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Move", fileName = "MoveEffect.asset")]
    public class MoveEffect : Effect
    {
        [SerializeField] int distance = 1;
        [SerializeField] bool isPull;
        public override void ActivateEffect(Unit target)
        {
            //TODO: allow characters to be moved different distances
            if(target is Character)
                gridController.TargetPush((Character)target, distance, isPull);
        }

        public override void ActivateEffect(HexCell target)
        {
            HexDirection direction = grid.GetDirection(unitController.CurrentPlayer.Location, target);
            List<HexCell> line = grid.GetLine(unitController.CurrentPlayer.Location, direction, projectileRange, true);

            if (line.Last().Unit && line.Last().Unit is Character)
                gridController.TargetPush((Character)line.Last().Unit, distance, isPull);
        }

        public override string GetEffectText()
        {
            string effectText = "Move target ";
            if (distance != 0)
            {
                effectText += distance + " hexs";
            }

            if(isPull)
            {
                effectText += " towards you";
            }
            else
            {
                effectText += " away from you";
            }
            return effectText;
        }
    }
}
