using FTS.Characters;
using UnityEngine;

namespace FTS.Core
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/DamageObjective", fileName = "DamageObjective.asset")]
    public class DamageObjective : Objective
    {
        [SerializeField] int[] damageThreshholds;
        int damageThreshhold = 0;
        int totalDamage;

        public override void EnableObjective()
        {
            totalDamage = 0;
            damageThreshhold = damageThreshholds[RunController.Instance.GetDifficultyScale()];
            UpdateObjective(totalDamage);
        }

        public override void EnableEncounter()
        {
            damageThreshhold = damageThreshholds[RunController.Instance.GetDifficultyScale()];
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

