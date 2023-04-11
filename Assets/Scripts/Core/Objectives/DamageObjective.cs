using FTS.Characters;
using UnityEngine;

namespace FTS.Core
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/DamageObjective", fileName = "DamageObjective.asset")]
    public class DamageObjective : Objective
    {
        [SerializeField] int damageThreshhold;
        int totalDamage;

        public override void EnableObjective()
        {
            totalDamage = 0;
            UpdateObjective(totalDamage);
        }

        public override void UpdateObjective(int damage)
        {
            totalDamage += damage;
            if (totalDamage < damageThreshhold)
            {
                isComplete = true;
            }
            else
            {
                isComplete = false;
            }
        }

        public override string SetDescription(bool isEncounter = false)
        {
            string description = "Take less than " + damageThreshhold + " damage";
            if (!isEncounter)
            {
                description += " (" + totalDamage + "/" + damageThreshhold + ")";
            }
            return description;
        }
    }
}

