using UnityEngine;
using FTS.Characters;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Stun", fileName = "StunEffect.asset")]
    public class StunEffect : Effect
    {
        public override void ActivateEffect(Unit target)
        {
            target.Stunned();
        }

        public override string GetEffectText()
        {
            return "Stun unit this turn";
        }

    }
}
