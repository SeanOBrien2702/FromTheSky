using UnityEngine;
using FTS.Grid;
using FTS.Cards;
using FTS.Characters;

namespace FTS.Core
{
    //available
    [System.Serializable]
    [CreateAssetMenu(menuName = "Objectives/KillObjective", fileName = "KillObjective.asset")]
    public class KillObjective : Objective
    {
        [SerializeField] int enemiesToKill;
        [SerializeField] Enemy enemy = null;
        ///Character enemyKilled;
        Enemy lastEnemyKilled = null;
        int currentKills = 0;

        //HexCoordinates location;
        //private void OnEnable()
        //{
        //    UnitController.OnEnemyKilled += UnitController_OnEnemyKilled;
        //}

        //private void UnitController_OnEnemyKilled(Character enemyKilled)
        //{
        //    if (enemy)
        //    {
        //        if (enemy.CharacterClass == lastEnemyKilled.CharacterClass)
        //        {
        //            currentKills++;
        //        }
        //    }
        //    else
        //    {
        //        currentKills++;
        //    }
        //    Debug.Log("currentKills " + currentKills + " enemiesToKill " + enemiesToKill);
        //    if (currentKills >= enemiesToKill)
        //    {
        //        currentKills = enemiesToKill;
        //        isComplete = true;
        //    }
        //}

        //public void SetLastEnemyKilled(Enemy enemyKilled)
        //{
        //    lastEnemyKilled = enemyKilled;
        //}

        public override void EnableObjective()
        {
            currentKills = 0;
        }


        public override void UpdateObjective(Enemy enemyKilled)
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

