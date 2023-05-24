using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Create/CreateUnit", fileName = "CreatUnitEffect.asset")]
    public class CreateUnitEffect : Effect
    {
        [SerializeField] Player unit;

        public override void ActivateEffect(HexCell target)
        {
            unitController.CreateUnit(unit, target);
            indicatorController.UpdateIndicators(null, target);
        }

        public override string GetEffectText()
        {
            string effectText = "Movement: " + unit.Stats.GetStat(Stat.Movement, unit.CharacterClass) + 
                                "\nHealth: " + unit.Stats.GetStat(Stat.Health, unit.CharacterClass) +
                                "\nEnergy: " + unit.Stats.GetStat(Stat.SupportRange, unit.CharacterClass);
            
            return effectText;
        }
    }
}
