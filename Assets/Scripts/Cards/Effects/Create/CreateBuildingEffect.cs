using FTS.Characters;
using FTS.Grid;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Create/CreateBuilding", fileName = "CreatBuildingEffect.asset")]
    public class CreateBuildingEffect : Effect
    {
        [SerializeField] Building building;

        public override void ActivateEffect(HexCell target)
        {
            unitController.CreateUnit(building, target);
            indicatorController.UpdateIndicators(null, target);
        }

        public override string GetEffectText()
        {
            string effectText = "";

            return effectText;
        }
    }
}
