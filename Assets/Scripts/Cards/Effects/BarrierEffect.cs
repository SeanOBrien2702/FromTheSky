using FTS.Characters;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Barrier", fileName = "BarrierEffect.asset")]
    public class BarrierEffect : Effect
    {
        public override void ActivateEffect(Unit target)
        {
            target.HasBarrier = true;
            Debug.Log("barrier effect played");
        }

        public override string GetEffectText()
        {
            return "Gain a <link=barrier><color=\"red\">barrier</color></link>";
        }
    }
}
