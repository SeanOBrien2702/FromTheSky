using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Characters;
using SP.Grid;

namespace SP.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Move", fileName = "MoveEffect.asset")]
    public class MoveEffect : Effect
    {
        [SerializeField] int distance = 1;
        public override void ActivateEffect(Character target)
        {
            //TODO: allow characters to be moved different distances
            FindObjectOfType<HexGridController>().GetComponent<HexGridController>().TargetPush(target);
        }

        public override string GetEffectText(Player player)
        {
            string effectText;
            if (distance == 1)
            {
                effectText = "Move target " + distance + " hex away from you";
            }
            else
            {
                effectText = "Move target " + distance + " hexes away from you";
            }
            return effectText;
        }
    }
}
