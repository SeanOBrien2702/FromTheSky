using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/CreateUnit", fileName = "CreatUnitEffect.asset")]
    public class CreateUnitEffect : Effect
    {
        [SerializeField] Player unit;

        public override void ActivateEffect(HexCell target)
        {
            unitController.CreateUnit(unit, target);
            gridController.UpdateIndicators(target, null); ;
        }

        public override string GetEffectText()
        {
            string effectText = "Create a " + unit.name;
            
            return effectText;
        }
    }
}
