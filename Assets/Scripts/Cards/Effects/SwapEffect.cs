using FTS.Characters;
using FTS.Grid;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Grid/Swap", fileName = "SwapEffect.asset")]
    public class SwapEffect : Effect
    {
        public override void ActivateEffect(Unit target)
        {
            HexCell targetPos = target.Location;
            HexCell playerPos = unitController.CurrentPlayer.Location;
            unitController.CurrentPlayer.GetComponent<Mover>().Location = targetPos;
            target.GetComponent<Mover>().Location = playerPos;
            gridController.UpdateReachable();
            if(target is Enemy)
                gridController.UpdateIndicators((Enemy)target);
        }

        public override string GetEffectText()
        {
            return "Swap positions with target";
        }
    }
}
