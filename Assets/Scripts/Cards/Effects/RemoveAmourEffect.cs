using UnityEngine;
using FTS.Characters;
using FTS.Grid;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/RemoveArmour", fileName = "RemoveArmourEffect.asset")]
    public class RemoveAmourEffect : Effect
    {
        [SerializeField] int armourAmount;

        public override void ActivateEffect(Unit target)
        {
            target.Armour = 0;
        }

        public override void ActivateEffect(HexCell target)
        {
            if (target.Unit)
            {
                target.Unit.Armour = 0;
            }
        }

        public override void ActivateEffect()
        {
            Debug.Log("armour effect?");
            
        }

        public override string GetEffectText()
        {
            return "Remove <link =armour><color=\"red\">armour</color></link>";
        }
    }
}
