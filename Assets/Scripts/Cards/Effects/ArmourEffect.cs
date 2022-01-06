using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Characters;

namespace SP.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Armour", fileName = "ArmourEffect.asset")]
    public class ArmourEffect : Effect
    {
        CardController cc;
        [SerializeField] int armourAmount;

        public override void ActivateEffect(Character target)
        {
            target.Armour += armourAmount;
            Debug.Log("armour effect played");
        }

        public override string GetEffectText(Player player)
        {
            return "Gain " + armourAmount + " <link =armour><color=\"red\">armour</color></link>";
        }
    }
}
