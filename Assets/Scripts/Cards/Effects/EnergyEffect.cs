using FTS.Characters;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Energy", fileName = "EnergyEffect.asset")]
    public class EnergyEffect : Effect
    {
        [SerializeField] int energyGained;
        public override void ActivateEffect()
        {
            unitController.CurrentPlayer.Energy += energyGained;
        }

        public override string GetEffectText()
        {
            return "Gain " + energyGained + " <link=energy><color=\"red\">energy</color></link>";
        }
    }
}
