using FTS.Characters;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Move", fileName = "MoveEffect.asset")]
    public class MoveEffect : Effect
    {
        [SerializeField] int distance = 1;
        public override void ActivateEffect(Unit target)
        {
            //TODO: allow characters to be moved different distances
            if(target is Character)
                gridController.TargetPush((Character)target);
        }

        public override string GetEffectText()
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
