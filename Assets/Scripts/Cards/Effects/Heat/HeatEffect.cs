using FTS.Characters;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Heat/Heat", fileName = "HeatEffect.asset")]
    public class HeatEffect : Effect
    {
        [SerializeField] int heatAmount;
        public override void ActivateEffect(Unit target)
        {
            target.GetComponent<Heat>().HeatLevel += heatAmount;
        }

        public override string GetEffectText()
        {
            return "Gain " + heatAmount + " <link=heat><color=\"red\">heat</color></link>";
        }
    }
}
