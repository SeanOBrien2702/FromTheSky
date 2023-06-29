using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Create/CreateUnit", fileName = "CreatUnitEffect.asset")]
    public class CreateUnitEffect : Effect, ICreate
    {
        [SerializeField] Player unit;

        public override void ActivateEffect(HexCell target)
        {
            unitController.CreateUnit(unit, target);
            indicatorController.UpdateIndicators(null, target);
        }

        public override string GetEffectText()
        { 
            return "Energy: " + unit.Stats.GetStat(Stat.SupportRange, unit.CharacterClass);
        }

        public string GetStat(Stat stat)
        {
            return unit.GetStat(stat).ToString();
        }
    }
}
