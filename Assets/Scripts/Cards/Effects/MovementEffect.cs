using FTS.Characters;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Movement", fileName = "MovementEffect.asset")]
    public class MovementEffect : Effect
    {
        [SerializeField] int movementGained;
        public override void ActivateEffect()
        {
            Character character = (Character)unitController.CurrentUnit;
            character.Mover.MovementLeft += movementGained;
            gridController.UpdateReachable();
        }

        public override string GetEffectText()
        {
            return "Gain " + movementGained + " movement";
        }
    }
}
