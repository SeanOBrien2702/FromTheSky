using FTS.Characters;
using UnityEngine;

namespace FTS.Core
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/KillObjective", fileName = "KillObjective.asset")]
    public class KillObjective : Objective
    {
        [SerializeField] int enemiesToKill;
        [SerializeField] Enemy enemy = null;
        int currentKills = 0;

        public override void EnableObjective()
        {
            currentKills = 0;
        }

        public override void UpdateObjective(Enemy enemyKilled)
        {
            if (enemy)
            {
                if (enemy.CharacterClass == enemyKilled.CharacterClass)
                {
                    currentKills++;
                }
            }
            else
            {
                currentKills++;
            }
            Debug.Log("currentKills " + currentKills + " enemiesToKill " + enemiesToKill);
            if (currentKills >= enemiesToKill)
            {
                currentKills = enemiesToKill;
                isComplete = true;
            }
        }

        public override string SetDescription(bool isEncounter = false)
        {
            Debug.Log("currentKills " + currentKills + " enemiesToKill " + enemiesToKill);
            string description = "Kill " + enemiesToKill;
            if (enemy)
            {
                description += " " + enemy.name;
            }
            else
            {
                description += " enemies";
            }
            if (!isEncounter)
            {
                description += " (" + currentKills + "/" + enemiesToKill + ")";
            }
            return description;
        }
    }
}

