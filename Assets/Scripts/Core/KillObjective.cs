using UnityEngine;
using SP.Grid;
using SP.Cards;
using SP.Characters;

namespace SP.Core
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/KillObjective", fileName = "KillObjective.asset")]
    public class KillObjective : Objective
    {
        [SerializeField] int enemiesToKill;
        [SerializeField] Enemy enemy = null;
        int currentKills = 0;
        
        //HexCoordinates location;

        public override void UpdateObjective(Character enemyKilled)
        {
            if(enemy)
            {
                if(enemy.CharacterClass == enemyKilled.CharacterClass)
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


        public override string SetDescription()
        {
            string description = "Kill " + enemiesToKill;
            if (enemy)
            {
                description += " " + enemy.name;
            }
            else
            {
                description += " enemies";
            }

            description += " (" + currentKills + "/" + enemiesToKill + ")";
            return description;
        }
    }
}

