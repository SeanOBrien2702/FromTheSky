using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/CreateTrap", fileName = "CreatTrapEffect.asset")]
    public class CreateTrapEffect : Effect
    {
        [SerializeField] Trap trap;

        public override void ActivateEffect(HexCell target)
        {
            Trap newTrap = Instantiate(trap);
            Debug.Log(newTrap.name + " !!!!");
            newTrap.transform.SetParent(grid.transform, false);
            // 
            newTrap.SetPosition(target);
        }

        public override string GetEffectText()
        {
            string effectText = "Create a " + trap.name + ". When an enemy walks on it, ";

            foreach (var effect in trap.GetEffects())
            {
                effectText += effect.GetEffectText();
            }
            
            return effectText;
        }
    }
}
